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

            foreach (PotionMatch match in batch)
            {
                EActionType actionType = match.ActionType;

                if (actionType == EActionType.NormalDestroy)
                {
                    match.TargetsIndex.Add(match.SourceIndex);

                    // Find and create special potions
                    yield return GenerateSpecialPotion.DetectSpecialPotions(match.TargetsIndex, tiles, specialToSpawn);
                }

                // foreach ((int w, int h) in match.TargetsIndex)
                // {
                //     TileController tile = tiles[w, h];
                //     PotionController potion = tile.currentPotion;
                //     EPotionType type = tile.currentPotion.getPotionSetting.PotionType;

                //     if (type != EPotionType.Normal && CheckValidSystem.PotionIsActivated((SpecialPotion)potion))
                //     {
                //         // Create new batch and add to queue
                //         continue;
                //     }
                // }

                // Run effect
            }

            yield return DestroySystem.Destroy(poolController, tiles, batch);

            yield return GenerateSpecialPotion.Generate(tiles, poolController, specialToSpawn);

            yield return new WaitForSeconds(0.5f);
        }

        specialToSpawn.Clear();
        yield break;
    }
}