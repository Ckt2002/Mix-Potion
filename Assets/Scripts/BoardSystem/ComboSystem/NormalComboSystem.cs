using System.Collections;
using System.Collections.Generic;

public class NormalComboSystem
{
    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH
        , Queue<List<PotionMatch>> matches)
    {
        List<PotionMatch> matchesList = new List<PotionMatch>();

        EPotionColor color1 = tiles[w, h].currentPotion.getPotionSetting.PotionColor;
        EPotionColor color2 = tiles[swappedW, swappedH].currentPotion.getPotionSetting.PotionColor;

        PotionMatch match1 = CreateMatches(tiles, w, h, color1);
        PotionMatch match2 = CreateMatches(tiles, swappedW, swappedH, color2);

        if (match1 != null)
            matchesList.Add(match1);

        if (match2 != null)
            matchesList.Add(match2);

        if (matchesList.Count > 0)
            matches.Enqueue(matchesList);

        yield break;
    }

    private static PotionMatch CreateMatches(TileController[,] tiles, int w, int h, EPotionColor color)
    {
        PotionMatch newMatches = new PotionMatch();
        newMatches.ActionType = EActionType.NormalDestroy;
        newMatches.SourceIndex = (w, h);
        newMatches.TargetsIndex = new();

        var row = FindRowMatches(tiles, w, h, color);
        var col = FindColMatches(tiles, w, h, color);

        if (row.Count >= 3)
            newMatches.TargetsIndex.UnionWith(row);
        if (col.Count >= 3)
            newMatches.TargetsIndex.UnionWith(col);

        if (newMatches.TargetsIndex.Count < 3)
            return null;

        return newMatches;
    }

    private static HashSet<(int, int)> FindRowMatches(TileController[,] tiles, int w, int h, EPotionColor color)
    {
        HashSet<(int, int)> matches = new() { (w, h) };

        for (int wTemp = w - 1; wTemp >= 0; wTemp--)
        {
            TileController tile = tiles[wTemp, h];

            if (!CheckValidSystem.ValidTile(tile))
                break;

            EPotionColor colorTemp = tile.currentPotion.getPotionSetting.PotionColor;

            if (color != colorTemp)
                break;

            matches.Add((wTemp, h));
        }

        for (int wTemp = w + 1; wTemp < tiles.GetLength(0); wTemp++)
        {
            TileController tile = tiles[wTemp, h];

            if (!CheckValidSystem.ValidTile(tile))
                break;

            EPotionColor colorTemp = tile.currentPotion.getPotionSetting.PotionColor;

            if (color != colorTemp)
                break;

            matches.Add((wTemp, h));
        }

        if (matches.Count < 2)
            matches.Clear();

        return matches;
    }

    private static HashSet<(int, int)> FindColMatches(TileController[,] tiles, int w, int h, EPotionColor color)
    {
        HashSet<(int, int)> matches = new() { (w, h) };

        for (int hTemp = h - 1; hTemp >= 0; hTemp--)
        {
            TileController tile = tiles[w, hTemp];

            if (!CheckValidSystem.ValidTile(tile))
                break;

            EPotionColor colorTemp = tile.currentPotion.getPotionSetting.PotionColor;

            if (color != colorTemp)
                break;

            matches.Add((w, hTemp));
        }

        for (int hTemp = h + 1; hTemp < tiles.GetLength(1); hTemp++)
        {
            TileController tile = tiles[w, hTemp];

            if (!CheckValidSystem.ValidTile(tile))
                break;

            EPotionColor colorTemp = tile.currentPotion.getPotionSetting.PotionColor;

            if (color != colorTemp)
                break;

            matches.Add((w, hTemp));
        }

        if (matches.Count < 2)
            matches.Clear();

        matches.Add((w, h));

        return matches;
    }
}