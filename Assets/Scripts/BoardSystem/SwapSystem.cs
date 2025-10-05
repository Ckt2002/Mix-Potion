using System;
using System.Collections;
using UnityEngine;

public static class SwapSystem
{
    public static IEnumerator Swap(Vector2 clickedPos, Vector2 releasePos, float dragThreshold,
        int w, int h, int width, int height, TileController[,] tiles,
        Action<(int, int)> swappedIndex = null, Action<bool> swappedAction = null)
    {
        Vector2 dir = releasePos - clickedPos;

        if (dir.magnitude < dragThreshold)
        {
            swappedAction?.Invoke(false);
            yield break;
        }

        int offsetW = 0, offsetH = 0;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            offsetW = (dir.x > 0) ? 1 : -1;
        else
            offsetH = (dir.y > 0) ? 1 : -1;

        int targetW = w + offsetW;
        int targetH = h + offsetH;

        if (!CheckValidSystem.ValidIndex(targetW, width) ||
            !CheckValidSystem.ValidIndex(targetH, height))
        {
            swappedAction?.Invoke(false);
            yield break;
        }

        TileController tile1 = tiles[w, h];
        TileController tile2 = tiles[targetW, targetH];

        if (!CheckValidSystem.ValidTile(tile1) ||
            !CheckValidSystem.ValidTile(tile2))
        {
            swappedAction?.Invoke(false);
            yield break;
        }

        PotionController potion1 = tile1.currentPotion;
        PotionController potion2 = tile2.currentPotion;

        yield return PotionTransformChangeSystem.SwapPotion(tile1, tile2, potion1, potion2);

        tile1.SetCurrentPotion(potion2);
        tile2.SetCurrentPotion(potion1);

        swappedIndex?.Invoke((targetW, targetH));
        swappedAction?.Invoke(true);
    }
}