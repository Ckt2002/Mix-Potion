using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillSystem
{
    private static List<(Vector3, Transform)> potionsToMove = new();

    public static IEnumerator RefillBoard(TileController[,] tiles, int width, int height,
        PoolController poolController)
    {
        for (int w = 0; w < width; w++)
        {
            int emptyCol = -1;

            for (int h = 0; h < height; h++)
            {
                TileController tile = tiles[w, h];

                if (!CheckValidSystem.TileActivated(tile))
                    continue;

                if (!CheckValidSystem.ContainCurrentPotion(tile))
                {
                    if (emptyCol == -1)
                        emptyCol = h;

                    continue;
                }

                if (emptyCol == -1)
                    continue;

                TileController emptyTile = tiles[w, emptyCol];
                PotionController potion = tile.currentPotion;

                tile.SetCurrentPotion(null);
                emptyTile.SetCurrentPotion(potion);

                potionsToMove.Add((emptyTile.transform.localPosition, potion.transform));

                emptyCol++;
            }

            if (emptyCol != -1)
            {
                for (int h = emptyCol; h < height; h++)
                {
                    TileController tile = tiles[w, h];
                    if (!CheckValidSystem.TileActivated(tile) ||
                        CheckValidSystem.ContainCurrentPotion(tile))
                        continue;

                    PotionController potion = poolController.GetRandomNormalPotion();
                    Vector3 spawnPos = tiles[w, height - 1].transform.localPosition;
                    Vector3 targetPos = tile.transform.localPosition;

                    spawnPos.y += 1f;
                    potion.transform.localPosition = spawnPos;
                    tile.SetCurrentPotion(potion);
                    potionsToMove.Add((targetPos, potion.transform));
                }
            }
        }

        if (potionsToMove.Count > 0)
        {
            //yield return new WaitForSeconds(0.5f);
            yield return PotionMovementSystem.MoveStraightDown(potionsToMove);
            potionsToMove.Clear();
        }
    }
}