using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombComboSystem
{
    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches)
    {
        // first explode batch
        CreateBatch(tiles, w, h, swappedW, swappedH, matches, 1);

        // second explode batch
        CreateBatch(tiles, w, h, swappedW, swappedH, matches, 2);
        yield break;
    }

    private static void CreateBatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches, int radius)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        PotionMatch match1 = new PotionMatch
        {
            ActionType = EActionType.Explode,
            SourceIndex = (w, h),
            TargetsIndex = new()
        };

        PotionMatch match2 = new PotionMatch
        {
            ActionType = EActionType.Explode,
            SourceIndex = (swappedW, swappedH),
            TargetsIndex = new()
        };

        Explode(tiles, w, h, width, height, radius, match1);
        Explode(tiles, swappedW, swappedH, width, height, radius, match2);

        matches.Enqueue(new() { match1, match2 });
    }

    private static void Explode(TileController[,] tiles, int centerW, int centerH, int width, int height,
        int radius, PotionMatch match)
    {
        int startW = centerW - radius;
        int endW = centerW + radius;
        int startH = centerH - radius;
        int endH = centerH + radius;

        for (int wTemp = startW; wTemp <= endW; wTemp++)
        {
            if (!CheckValidSystem.ValidIndex(wTemp, width) ||
                wTemp == centerW)
                continue;

            for (int hTemp = startH; hTemp <= endH; hTemp++)
            {
                if (!CheckValidSystem.ValidIndex(hTemp, height)
                || NotOuter(wTemp, hTemp, centerW, centerH, radius))
                    continue;

                TileController tile = tiles[wTemp, hTemp];

                if (!CheckValidSystem.ValidTile(tile))
                    continue;

                match.TargetsIndex.Add((wTemp, hTemp));
            }
        }
    }

    private static bool NotOuter(int w, int h, int centerW, int centerH, int radius)
        => Mathf.Abs(w - centerW) < radius && Mathf.Abs(h - centerH) < radius;
}