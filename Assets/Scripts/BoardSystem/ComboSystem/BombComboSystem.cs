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
            ActionType = EActionType.NormalExplode,
            SourceIndex = (w, h),
            TargetsIndex = new()
        };

        PotionMatch match2 = new PotionMatch
        {
            ActionType = EActionType.NormalExplode,
            SourceIndex = (swappedW, swappedH),
            TargetsIndex = new()
        };

        if (radius >= 2)
        {
            match1.TargetsIndex.Add((w, h));
            match2.TargetsIndex.Add((swappedW, swappedH));

            ((SpecialPotion)tiles[w, h].currentPotion).ActivateSpecial();
            ((SpecialPotion)tiles[swappedW, swappedH].currentPotion).ActivateSpecial();
        }

        Explode(tiles, w, h, width, height, radius, match1, swappedW, swappedH);
        Explode(tiles, swappedW, swappedH, width, height, radius, match2, w, h);

        matches.Enqueue(new() { match1, match2 });
    }

    private static void Explode(TileController[,] tiles, int centerW, int centerH, int width, int height,
        int radius, PotionMatch match, int avoidW, int avoidH)
    {
        int startW = centerW - radius;
        int endW = centerW + radius;
        int startH = centerH - radius;
        int endH = centerH + radius;

        for (int wTemp = startW; wTemp <= endW; wTemp++)
        {
            if (!CheckValidSystem.ValidIndex(wTemp, width))
                continue;

            for (int hTemp = startH; hTemp <= endH; hTemp++)
            {
                if (!CheckValidSystem.ValidIndex(hTemp, height)
                || NotOuter(wTemp, hTemp, centerW, centerH, radius))
                    continue;

                TileController tile = tiles[wTemp, hTemp];

                if (!CheckValidSystem.ValidTile(tile) ||
                    wTemp == avoidW && hTemp == avoidH)
                    continue;

                match.TargetsIndex.Add((wTemp, hTemp));
            }
        }
    }

    private static bool NotOuter(int w, int h, int centerW, int centerH, int radius)
        => Mathf.Abs(w - centerW) < radius && Mathf.Abs(h - centerH) < radius;
}