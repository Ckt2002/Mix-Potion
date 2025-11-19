using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteSystem
{
    private static Dictionary<(int, int), (EPotionColor, EPotionType)> specialToSpawn = new();

    public static IEnumerator ExecuteMatchPotions(TileController[,] tiles, Queue<List<PotionMatch>> queueBatches,
        PoolController poolController)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);
        bool runDestroyPotionImmediately = false;

        while (queueBatches.Count > 0)
        {
            List<PotionMatch> lstBatch = queueBatches.Dequeue();

            int batchIndex = 0;

            while (batchIndex < lstBatch.Count)
            {
                runDestroyPotionImmediately = false;
                PotionMatch match = lstBatch[batchIndex];

                EActionType actionType = match.ActionType;

                switch (actionType)
                {
                    case EActionType.NormalDestroy:
                        {
                            match.TargetsIndex.Add(match.SourceIndex);
                            // Find and create special potions
                            yield return GenerateSpecialPotion.DetectSpecialPotions(match.TargetsIndex, tiles, specialToSpawn);
                            ExecuteSpecialPotions(tiles, width, height, match, queueBatches, lstBatch, true);
                            break;
                        }

                    case EActionType.NormalSwipe:
                        {
                            ExecuteSpecialPotions(tiles, width, height, match, queueBatches, lstBatch, true);
                            break;
                        }

                    case EActionType.NormalExplode:
                        ExecuteSpecialPotions(tiles, width, height, match, queueBatches, lstBatch, true);
                        break;

                    case EActionType.NormalLightning:
                        {
                            Vector3 sourcePos = tiles[match.SourceIndex.w, match.SourceIndex.h].transform.position;
                            foreach ((int w, int h) in match.TargetsIndex)
                            {
                                Vector3 targetPos = tiles[w, h].transform.position;
                                GameObject effect = poolController.GetSpecialEffect(EEffectType.Lightning);
                                LineRenderer line = effect.GetComponent<LineRenderer>();
                                line.SetPosition(0, sourcePos);
                                line.SetPosition(1, targetPos);
                                effect.SetActive(true);

                                EffectSystem.instance.AddSpawnedEffect(effect);
                            }

                            yield return CoroutineWaitTimes.Wait_1;

                            ExecuteSpecialPotions(tiles, width, height, match, queueBatches, lstBatch, true);
                            break;
                        }

                    case EActionType.ClearBoard:
                        {
                            // Will only if there are any clear board action exist in batch
                            // Run effects on lightning 1
                            PotionMatch match1 = match;
                            Vector3 sourcePos = tiles[match1.SourceIndex.w, match1.SourceIndex.h].transform.position;
                            foreach ((int w, int h) in match1.TargetsIndex)
                            {
                                Vector3 targetPos = tiles[w, h].transform.position;
                                GameObject effect = poolController.GetSpecialEffect(EEffectType.Lightning);
                                LineRenderer line = effect.GetComponent<LineRenderer>();
                                line.SetPosition(0, sourcePos);
                                line.SetPosition(1, targetPos);
                                effect.SetActive(true);

                                EffectSystem.instance.AddSpawnedEffect(effect);
                            }

                            // Run effects on lightning 2
                            ++batchIndex;
                            PotionMatch match2 = lstBatch[batchIndex];

                            sourcePos = tiles[match2.SourceIndex.w, match2.SourceIndex.h].transform.position;
                            foreach ((int w, int h) in match2.TargetsIndex)
                            {
                                Vector3 targetPos = tiles[w, h].transform.position;
                                GameObject effect = poolController.GetSpecialEffect(EEffectType.Lightning);
                                LineRenderer line = effect.GetComponent<LineRenderer>();
                                line.SetPosition(0, sourcePos);
                                line.SetPosition(1, targetPos);
                                effect.SetActive(true);

                                EffectSystem.instance.AddSpawnedEffect(effect);
                            }
                            runDestroyPotionImmediately = true;

                            yield return CoroutineWaitTimes.Wait_1;

                            // Destroy potions after completed effect
                            yield return DestroySystem.ClearBoard(poolController, tiles, match1, match2);

                            break;
                        }
                }

                ++batchIndex;
            }

            if (!runDestroyPotionImmediately)
                yield return DestroySystem.Destroy(poolController, tiles, lstBatch);

            yield return GenerateSpecialPotion.Generate(tiles, poolController, specialToSpawn);

            yield return CoroutineWaitTimes.Wait_0_5;

            // End current queue
        }

        specialToSpawn.Clear();
        yield break;
    }

    private static void ExecuteSpecialPotions(TileController[,] tiles, int width, int height, PotionMatch match,
        Queue<List<PotionMatch>> matchBatches, List<PotionMatch> currentBatch, bool executeImmediate = false)
    {
        foreach ((int w, int h) in match.TargetsIndex)
        {
            TileController tile = tiles[w, h];
            PotionController potion = tile.currentPotion;

            if (potion == null)
                continue;

            EPotionType type = tile.currentPotion.getPotionSetting.PotionType;

            if (type != EPotionType.Normal && !CheckValidSystem.PotionIsActivated((SpecialPotion)potion))
            {
                PotionMatch newMatch = new PotionMatch
                {
                    SourceIndex = (w, h),
                    TargetsIndex = new()
                };

                SpecialPotion specialPotion = (SpecialPotion)tiles[w, h].currentPotion;
                specialPotion.ActivateSpecial();

                switch (type)
                {
                    case EPotionType.Bomb:
                        newMatch.ActionType = EActionType.NormalExplode;
                        Wipe(tiles, w - 1, w + 1, h - 1, h + 1, width, height, newMatch.TargetsIndex);
                        break;

                    case EPotionType.Row:
                        newMatch.ActionType = EActionType.NormalSwipe;
                        Wipe(tiles, 0, width - 1, h, h, width, height, newMatch.TargetsIndex);
                        break;

                    case EPotionType.Column:
                        newMatch.ActionType = EActionType.NormalSwipe;
                        Wipe(tiles, w, w, 0, height - 1, width, height, newMatch.TargetsIndex);
                        break;

                    default:
                        newMatch.ActionType = EActionType.NormalLightning;
                        DestroyRandomly(tiles, width, height, newMatch.TargetsIndex);
                        break;
                }

                if (executeImmediate)
                    currentBatch.Add(newMatch);
                else
                    matchBatches.Enqueue(new List<PotionMatch> { newMatch });
            }
        }
    }

    private static void Wipe(TileController[,] tiles, int startW, int endW, int startH, int endH, int width, int height,
        HashSet<(int w, int h)> targetsIndex)
    {
        for (int wTemp = startW; wTemp <= endW; wTemp++)
        {
            if (!CheckValidSystem.ValidIndex(wTemp, width))
                continue;

            for (int hTemp = startH; hTemp <= endH; hTemp++)
            {
                if (!CheckValidSystem.ValidIndex(hTemp, height))
                    continue;

                TileController tile = tiles[wTemp, hTemp];
                if (!CheckValidSystem.ValidTile(tile))
                    continue;

                PotionController potion = tile.currentPotion;
                if (potion.getPotionSetting.PotionType != EPotionType.Normal &&
                    CheckValidSystem.PotionIsActivated((SpecialPotion)potion))
                    continue;

                targetsIndex.Add((wTemp, hTemp));
            }
        }
    }

    private static void DestroyRandomly(TileController[,] tiles, int width, int height,
        HashSet<(int w, int h)> targetsIndex)
    {
        EPotionColor color = (EPotionColor)Random.Range((int)EPotionColor.Blue, (int)EPotionColor.Orange + 1);

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                TileController tile = tiles[w, h];
                if (!CheckValidSystem.ValidTile(tile))
                    continue;
                PotionController potion = tile.currentPotion;
                if (potion.getPotionSetting.PotionColor != color)
                    continue;

                targetsIndex.Add((w, h));
            }
        }
    }
}