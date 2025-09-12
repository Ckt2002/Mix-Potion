using System.Collections.Generic;

public class CheckMatchSystem
{
    public void CheckMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH, out HashSet<(int, int)> visited)
    {
        HashSet<(int, int)> visitedTemp1 = new();
        HashSet<(int, int)> visitedTemp2 = new();

        CheckHorizontalMatch(tiles, w, h, visitedTemp1);
        CheckVerticalMatch(tiles, w, h, visitedTemp1);

        if (visitedTemp1.Count >= 2)
            visitedTemp1.Add((w, h));

        CheckHorizontalMatch(tiles, swappedW, swappedH, visitedTemp2);
        CheckVerticalMatch(tiles, swappedW, swappedH, visitedTemp2);

        if (visitedTemp2.Count >= 2)
            visitedTemp2.Add((swappedW, swappedH));

        visitedTemp1.UnionWith(visitedTemp2);
        visited = visitedTemp1;
    }

    private void CheckHorizontalMatch(TileController[,] tiles, int w, int h, HashSet<(int, int)> visited)
    {
        HashSet<(int, int)> visitedTemp = new();

        EPotionColor color = tiles[w, h].currentPotion.potion.PotionColor;
        for (int wTemp = w + 1; wTemp < tiles.GetLength(0); wTemp++)
        {
            if (tiles[wTemp, h].currentPotion.potion.PotionColor != color)
                break;

            visitedTemp.Add((wTemp, h));
        }

        for (int wTemp = w - 1; wTemp >= 0; wTemp--)
        {
            if (tiles[wTemp, h].currentPotion.potion.PotionColor != color)
                break;

            visitedTemp.Add((wTemp, h));
        }

        if (visitedTemp.Count < 2)
            visitedTemp.Clear();
        else
            visited.UnionWith(visitedTemp);
    }

    private void CheckVerticalMatch(TileController[,] tiles, int w, int h, HashSet<(int, int)> visited)
    {
        HashSet<(int, int)> visitedTemp = new();

        EPotionColor color = tiles[w, h].currentPotion.potion.PotionColor;
        for (int hTemp = h + 1; hTemp < tiles.GetLength(0); hTemp++)
        {
            if (tiles[w, hTemp].currentPotion.potion.PotionColor != color)
                break;

            visitedTemp.Add((w, hTemp));
        }

        for (int hTemp = h - 1; hTemp >= 0; hTemp--)
        {
            if (tiles[w, hTemp].currentPotion.potion.PotionColor != color)
                break;

            visitedTemp.Add((w, hTemp));
        }

        if (visitedTemp.Count < 2)
            visitedTemp.Clear();
        else
            visited.UnionWith(visitedTemp);
    }
}