using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteSystem
{
    private static Dictionary<(int, int), (EPotionColor, EPotionType)> specialToSpawn = new();

    public static IEnumerator ExecuteMatchPotions(TileController[,] tiles, Queue<List<PotionMatch>> matchBatches,
        PoolController poolController)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        while (matchBatches.Count > 0)
        {
            List<PotionMatch> batch = matchBatches.Dequeue();
            Debug.Log(batch.Count);

            foreach (PotionMatch match in batch)
            {
                EActionType actionType = match.ActionType;

                if (actionType == EActionType.NormalDestroy)
                {
                    match.TargetsIndex.Add(match.SourceIndex);

                    // Find and create special potions
                    yield return GenerateSpecialPotion.DetectSpecialPotions(match.TargetsIndex, tiles, specialToSpawn);

                    ExecuteSpecialPotions(tiles, width, height, match, matchBatches);
                }

                // Run effect
            }

            yield return DestroySystem.Destroy(poolController, tiles, batch);

            yield return GenerateSpecialPotion.Generate(tiles, poolController, specialToSpawn);

            yield return new WaitForSeconds(0.5f);
        }

        specialToSpawn.Clear();
        yield break;
    }

    private static void ExecuteSpecialPotions(TileController[,] tiles, int width, int height, PotionMatch match, Queue<List<PotionMatch>> matchBatches)
    {
        foreach ((int w, int h) in match.TargetsIndex)
        {
            TileController tile = tiles[w, h];
            PotionController potion = tile.currentPotion;
            EPotionType type = tile.currentPotion.getPotionSetting.PotionType;

            if (type != EPotionType.Normal && !CheckValidSystem.PotionIsActivated((SpecialPotion)potion))
            {
                // Create new batch and add to queue

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
                        newMatch.ActionType = EActionType.Explode;
                        Wipe(tiles, w - 1, w + 1, h - 1, h + 1, width, height, newMatch.TargetsIndex);
                        break;

                    case EPotionType.Row:
                        Debug.Log("Row");
                        newMatch.ActionType = EActionType.Swipe;
                        Wipe(tiles, 0, width - 1, h, h, width, height, newMatch.TargetsIndex);
                        break;

                    case EPotionType.Column:
                        Debug.Log("Col");
                        newMatch.ActionType = EActionType.Swipe;
                        Wipe(tiles, w, w, 0, height - 1, width, height, newMatch.TargetsIndex);
                        break;

                    default:
                        newMatch.ActionType = EActionType.Lightning;
                        break;
                }

                matchBatches.Enqueue(new() { newMatch });

                continue;
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

                targetsIndex.Add((wTemp, hTemp));
            }
        }
    }
}