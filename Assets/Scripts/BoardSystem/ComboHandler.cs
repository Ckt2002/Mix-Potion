using System.Collections;
using System.Collections.Generic;

public class ComboHandler
{
    HashSet<(int, int, PotionController)> visitedTiles;
    PoolController poolController;
    TileController[,] tiles;
    int width, height;

    public ComboHandler(TileController[,] tiles, HashSet<(int, int, PotionController)> visitedTiles,
        PoolController poolController)
    {
        this.visitedTiles = visitedTiles;
        this.poolController = poolController;
        this.tiles = tiles;
        width = tiles.GetLength(0);
        height = tiles.GetLength(1);
    }

    private bool ValidIndex(int w, int h) => w >= 0 && w < width && h >= 0 && h < height;

    public IEnumerator ClearBoard()
    {
        foreach (var tile in tiles)
        {
            if (!tile.gameObject.activeSelf ||
                tile.currentPotion == null)
                continue;

            var set = (tile.w, tile.h, tile.currentPotion);
            if (!visitedTiles.Contains(set))
                visitedTiles.Add(set);
        }
        yield return null;
    }

    public IEnumerator BigBang(int w, int h, int swappedW, int swappedH)
    {
        int startW1 = w - 2;
        int endW1 = w + 2;
        int startH1 = h - 2;
        int endH1 = h + 2;

        for (int wTemp = startW1; wTemp < endW1; wTemp++)
        {
            for (int hTemp = startH1; hTemp < endH1; hTemp++)
            {
                if (!ValidIndex(wTemp, hTemp))
                    continue;

                if (!tiles[wTemp, hTemp].gameObject.activeSelf ||
                tiles[wTemp, hTemp].currentPotion == null)
                    continue;

                var vistedTemp = (w, h, tiles[w, h].currentPotion);
                if (visitedTiles.Contains(vistedTemp))
                    continue;

                visitedTiles.Add(vistedTemp);
            }
        }

        yield return null;
    }

    public IEnumerator Plus(int w, int h, int swappedW, int swappedH, bool isSame)
    {
        TileController tile1 = tiles[w, h];
        TileController tile2 = tiles[swappedW, swappedH];

        // If both have the same Row or Col, change the first one to other type
        if (isSame)
        {
            // Hide old potion
            PotionController potion = tile1.currentPotion;
            poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);

            // Get potion from pool
            PotionController spawnedPotion;
            if (potion.getPotionSetting.PotionType == EPotionType.Row)
                spawnedPotion = poolController.GetSpecialPotion(potion.getPotionSetting.PotionColor,
                potion.getPotionSetting.PotionType);
            else
                spawnedPotion = poolController.GetSpecialPotion(potion.getPotionSetting.PotionColor,
                potion.getPotionSetting.PotionType);

            spawnedPotion.transform.localPosition = tile1.transform.localPosition;
            tile1.SetCurrentPotion(spawnedPotion);
        }

        visitedTiles.Add((w, h, tile1.currentPotion));
        visitedTiles.Add((swappedW, swappedH, tile2.currentPotion));

        yield return null;
    }

    public IEnumerator ExplodeRandomly()
    {
        EPotionColor color = (EPotionColor)UnityEngine.Random.Range(
            (int)EPotionColor.Blue, (int)EPotionColor.Yellow + 1);

        foreach (var tile in tiles)
        {
            PotionController potion = tile.currentPotion;
            if (potion.getPotionSetting.PotionColor != color ||
                potion.getPotionSetting.PotionType != EPotionType.Normal)
                continue;

            poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);

            potion = poolController.GetSpecialPotion(color, EPotionType.Bomb);
            potion.transform.localPosition = tile.transform.localPosition;
            tile.SetCurrentPotion(potion);
            visitedTiles.Add((tile.w, tile.h, potion));
        }
        yield return null;
    }

    public IEnumerator SwipeRandomly(EPotionColor potionColor)
    {
        EPotionType type = (EPotionType)UnityEngine.Random.Range(1, 3);

        foreach (var tile in tiles)
        {
            PotionController potion = tile.currentPotion;
            if (potion.getPotionSetting.PotionColor != potionColor ||
                potion.getPotionSetting.PotionType != EPotionType.Normal)
                continue;

            poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);

            potion = poolController.GetSpecialPotion(potionColor, type);
            potion.transform.localPosition = tile.transform.localPosition;
            tile.SetCurrentPotion(potion);
            visitedTiles.Add((tile.w, tile.h, potion));
        }

        yield return null;
    }

    public IEnumerator DestroyRandomly(EPotionColor potionColor)
    {
        EPotionType type = (EPotionType)UnityEngine.Random.Range(1, 3);

        foreach (var tile in tiles)
        {
            PotionController potion = tile.currentPotion;
            if (potion.getPotionSetting.PotionColor != potionColor ||
                potion.getPotionSetting.PotionType != EPotionType.Normal)
                continue;

            visitedTiles.Add((tile.w, tile.h, potion));
        }

        yield return null;
    }

    public IEnumerator TripleSwipe(int w, int h, EPotionType swipeType)
    {
        if (swipeType == EPotionType.Row)
        {
            for (int hTemp = h - 1; hTemp <= h + 1; hTemp++)
            {
                if (hTemp < 0 || hTemp >= height)
                    continue;
                for (int wTemp = 0; wTemp < width; wTemp++)
                {
                    if (wTemp < 0 || wTemp >= width)
                        continue;
                    if (!tiles[wTemp, hTemp].gameObject.activeSelf ||
                        tiles[wTemp, hTemp].currentPotion == null)
                        continue;

                    visitedTiles.Add((wTemp, hTemp, tiles[wTemp, hTemp].currentPotion));
                }

            }
        }
        else
        {
            for (int wTemp = w - 1; wTemp <= w + 1; wTemp++)
            {
                if (wTemp < 0 || wTemp >= width)
                    continue;
                for (int hTemp = 0; hTemp < height; hTemp++)
                {
                    if (hTemp < 0 || hTemp >= height)
                        continue;
                    if (!tiles[wTemp, hTemp].gameObject.activeSelf ||
                        tiles[wTemp, hTemp].currentPotion == null)
                        continue;

                    visitedTiles.Add((wTemp, hTemp, tiles[wTemp, hTemp].currentPotion));
                }

            }
        }
        yield return null;
    }
}