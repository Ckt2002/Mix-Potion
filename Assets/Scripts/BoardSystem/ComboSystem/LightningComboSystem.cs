using System.Collections;
using System.Collections.Generic;

public class LightningComboSystem
{
    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        for (int wTemp = 0; wTemp < width; wTemp++)
        {
            PotionMatch match1 = new PotionMatch
            {
                ActionType = EActionType.Lightning,
                SourceIndex = (w, h),
                TargetsIndex = new()
            };

            PotionMatch match2 = new PotionMatch
            {
                ActionType = EActionType.Lightning,
                SourceIndex = (swappedW, swappedH),
                TargetsIndex = new()
            };

            if (wTemp == width - 1)
            {
                match1.TargetsIndex.Add((w, h));
                match2.TargetsIndex.Add((swappedW, swappedH));
            }

            for (int hTemp = 0; hTemp < height; hTemp++)
            {
                TileController tile = tiles[wTemp, hTemp];

                if (!CheckValidSystem.ValidTile(tile) ||
                    SameIndex(w, h, wTemp, hTemp) ||
                    SameIndex(swappedW, swappedH, wTemp, hTemp))
                    continue;

                if (hTemp % 2 == 0)
                {
                    match2.TargetsIndex.Add((wTemp, hTemp));
                    continue;
                }

                match1.TargetsIndex.Add((wTemp, hTemp));
            }

            matches.Enqueue(new() { match1, match2 });
        }

        yield break;
    }

    private static bool SameIndex(int w, int h, int wTemp, int hTemp)
        => w == wTemp && h == hTemp;
}