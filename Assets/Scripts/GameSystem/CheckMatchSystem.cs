public class CheckMatchSystem
{
    public int CheckMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH)
    {
        int count = 0;

        int countH = CheckHorizontalMatch(tiles, w, h);
        int countV = CheckVerticalMatch(tiles, w, h);

        if (countH >= 2)
            count += countH;
        if (countV >= 2)
            count += countV;

        countH = CheckHorizontalMatch(tiles, swappedW, swappedH);
        countV = CheckVerticalMatch(tiles, swappedW, swappedH);

        if (countH >= 2)
            count += countH;
        if (countV >= 2)
            count += countV;

        return count;
    }

    private int CheckHorizontalMatch(TileController[,] tiles, int w, int h)
    {
        int count = 0;

        EPotionColor color = tiles[w, h].currentPotion.potion.PotionColor;
        for (int wTemp = w + 1; wTemp < tiles.GetLength(0); wTemp++)
        {
            if (tiles[wTemp, h].currentPotion.potion.PotionColor != color)
                break;

            count++;
        }

        for (int wTemp = w - 1; wTemp >= 0; wTemp--)
        {
            if (tiles[wTemp, h].currentPotion.potion.PotionColor != color)
                break;

            count++;
        }

        return count;
    }

    private int CheckVerticalMatch(TileController[,] tiles, int w, int h)
    {
        int count = 0;

        EPotionColor color = tiles[w, h].currentPotion.potion.PotionColor;
        for (int hTemp = h + 1; hTemp < tiles.GetLength(0); hTemp++)
        {
            if (tiles[w, hTemp].currentPotion.potion.PotionColor != color)
                break;

            count++;
        }

        for (int hTemp = h - 1; hTemp >= 0; hTemp--)
        {
            if (tiles[w, hTemp].currentPotion.potion.PotionColor != color)
                break;

            count++;
        }

        return count;
    }
}