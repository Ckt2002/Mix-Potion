using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMatchSystem
{
    public static IEnumerator Execute(TileController[,] tiles, Queue<List<PotionMatch>> matchBatches,
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
                    match.TargetIndex.Add(match.SourceIndex);

                foreach ((int w, int h) in match.TargetIndex)
                {
                    TileController tile = tiles[w, h];
                    PotionController potion = tile.currentPotion;
                    EPotionType type = tile.currentPotion.getPotionSetting.PotionType;

                    if (type != EPotionType.Normal && CheckValidSystem.PotionIsActivated((SpecialPotion)potion))
                    {
                        // Create new batch and add to queue
                        continue;
                    }
                }

                // Run effect
            }

            yield return DestroySystem.Destroy(poolController, tiles, batch);

            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }
}