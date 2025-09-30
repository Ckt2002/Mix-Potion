using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillSystem
{
    private static Dictionary<Vector3, Transform> potionsToMove = new();

    public static IEnumerator RefillBoard(TileController[,] tiles, int width, int height)
    {
        for (int w = 0; w < width; w++)
            for (int h = 0; h < height; h++)
            {
                TileController tile = tiles[w, h];

                if (!CheckValidSystem.TileActivated(tile) ||
                    CheckValidSystem.ContainCurrentPotion(tile))
                    continue;

                for (int hTemp = h + 1; hTemp < height; hTemp++)
                {
                    TileController tileTemp = tiles[w, hTemp];

                    if (!CheckValidSystem.TileActivated(tileTemp) ||
                    !CheckValidSystem.ContainCurrentPotion(tileTemp))
                        continue;

                    PotionController potion = tileTemp.currentPotion;

                    tileTemp.SetCurrentPotion(null);
                    tile.SetCurrentPotion(potion);

                    potionsToMove.Add(tile.transform.localPosition, potion.transform);
                    break;
                }
            }

        if (potionsToMove.Count > 0)
        {
            yield return PotionMovementSystem.MoveStraightDown(potionsToMove);
            potionsToMove.Clear();
        }

        yield break;
    }

    private static void CheckStraightDown()
    {
    }
}