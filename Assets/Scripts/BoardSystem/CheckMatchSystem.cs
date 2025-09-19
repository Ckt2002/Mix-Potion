using System.Collections.Generic;

public class CheckMatchSystem
{

    HashSet<(int, int, PotionController)> visitedTemp = new();

    public void CheckMatchAfterSwap(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        HashSet<(int, int, PotionController)> visited)
    {
        CheckHorizontalMatch(tiles, w, h, visitedTemp);
        visited.UnionWith(visitedTemp);
        CheckVerticalMatch(tiles, w, h, visitedTemp);
        visited.UnionWith(visitedTemp);

        CheckHorizontalMatch(tiles, swappedW, swappedH, visitedTemp);
        visited.UnionWith(visitedTemp);
        CheckVerticalMatch(tiles, swappedW, swappedH, visitedTemp);
        visited.UnionWith(visitedTemp);
    }

    private void CheckHorizontalMatch(TileController[,] tiles, int w, int h,
        HashSet<(int, int, PotionController)> visited)
    {
        visited.Clear();
        visited.Add((w, h, tiles[w, h].currentPotion));
        EPotionColor color = tiles[w, h].currentPotion.getPotionSetting.PotionColor;
        for (int wTemp = w + 1; wTemp < tiles.GetLength(0); wTemp++)
        {
            if (tiles[wTemp, h].currentPotion == null)
                break;
            if (tiles[wTemp, h].currentPotion.getPotionSetting.PotionColor != color)
                break;

            visited.Add((wTemp, h, tiles[wTemp, h].currentPotion));
        }

        for (int wTemp = w - 1; wTemp >= 0; wTemp--)
        {
            if (tiles[wTemp, h].currentPotion == null)
                break;
            if (tiles[wTemp, h].currentPotion.getPotionSetting.PotionColor != color)
                break;

            visited.Add((wTemp, h, tiles[wTemp, h].currentPotion));
        }

        if (visited.Count < 3)
            visited.Clear();
    }

    private void CheckVerticalMatch(TileController[,] tiles, int w, int h,
        HashSet<(int, int, PotionController)> visited)
    {
        visited.Clear();
        visited.Add((w, h, tiles[w, h].currentPotion));
        EPotionColor color = tiles[w, h].currentPotion.getPotionSetting.PotionColor;
        for (int hTemp = h + 1; hTemp < tiles.GetLength(0); hTemp++)
        {
            if (tiles[w, hTemp].currentPotion == null)
                break;
            if (tiles[w, hTemp].currentPotion.getPotionSetting.PotionColor != color)
                break;

            visited.Add((w, hTemp, tiles[w, hTemp].currentPotion));
        }

        for (int hTemp = h - 1; hTemp >= 0; hTemp--)
        {
            if (tiles[w, hTemp].currentPotion == null)
                break;
            if (tiles[w, hTemp].currentPotion.getPotionSetting.PotionColor != color)
                break;

            visited.Add((w, hTemp, tiles[w, hTemp].currentPotion));
        }

        if (visited.Count < 3)
            visited.Clear();
    }
}