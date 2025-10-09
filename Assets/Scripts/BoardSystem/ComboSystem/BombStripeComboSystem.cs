using System.Collections;
using System.Collections.Generic;

public class BombStripeComboSystem
{
    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches, EPotionType type2)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        int sourceW = w;
        int sourceH = h;

        if (type2 == EPotionType.Row || type2 == EPotionType.Column)
        {
            sourceW = swappedW;
            sourceH = swappedH;
        }

        int startW = sourceW - 1;
        int endW = sourceW + 1;
        int startH = sourceH - 1;
        int endH = sourceH + 1;

        // wipe row first
        Wipe(tiles, 0, width - 1, startH, endH, width, height, matches, sourceW, sourceH);
        // wipe col
        Wipe(tiles, startW, endW, 0, height - 1, width, height, matches, sourceW, sourceH);

        yield break;
    }

    private static void Wipe(TileController[,] tiles, int startW, int endW, int startH, int endH,
        int width, int height, Queue<List<PotionMatch>> matches, int sourceW, int sourceH)
    {
        PotionMatch match = new PotionMatch
        {
            ActionType = EActionType.Swipe,
            SourceIndex = (sourceW, sourceH),
            TargetsIndex = new()
        };

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

                match.TargetsIndex.Add((wTemp, hTemp));
            }
        }

        matches.Enqueue(new() { match });
    }
}