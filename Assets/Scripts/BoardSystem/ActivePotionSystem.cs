using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePotionSystem
{
    private TileController[,] tiles;
    int width, height;

    public ActivePotionSystem(TileController[,] tiles)
    {
        this.tiles = tiles;
        width = tiles.GetLength(0);
        height = tiles.GetLength(1);
    }

    public IEnumerator ActiveAllPotions(HashSet<(int, int, PotionController)> visitedTiles)
    {
        HashSet<(int, int, PotionController)> temp = new HashSet<(int, int, PotionController)>(visitedTiles);

        foreach (var visited in visitedTiles)
            yield return DectectPotion(temp, visited);

        visitedTiles.UnionWith(temp);
    }

    private IEnumerator DectectPotion(HashSet<(int, int, PotionController)> visitedTiles,
        (int, int, PotionController) visited)
    {
        switch (visited.Item3.getPotionSetting.PotionType)
        {
            case EPotionType.Row:
                yield return ClearRow(visitedTiles, visited);
                break;

            case EPotionType.Column:
                yield return ClearColumn(visitedTiles, visited);
                break;

            case EPotionType.Bomb:
                yield return Clear3x3(visitedTiles, visited);
                break;

            case EPotionType.Lightning:
                yield return ClearRandomly(visitedTiles, visited);
                break;

            default:
                break;
        }
    }

    private IEnumerator ClearRow(HashSet<(int, int, PotionController)> visitedTiles,
        (int, int, PotionController) visited)
    {
        int h = visited.Item2;

        for (int w = 0; w < width; w++)
        {
            if (!tiles[w, h].gameObject.activeSelf ||
                tiles[w, h].currentPotion == null)
                continue;

            PotionController potion = tiles[w, h].currentPotion;

            var vistedTemp = (w, h, potion);

            if (visitedTiles.Contains(vistedTemp))
                continue;

            visitedTiles.Add(vistedTemp);
            yield return DectectPotion(visitedTiles, vistedTemp);
        }

        yield return null;
    }

    private IEnumerator ClearColumn(HashSet<(int, int, PotionController)> visitedTiles,
        (int, int, PotionController) visited)
    {
        int w = visited.Item1;

        for (int h = 0; h < height; h++)
        {
            if (!tiles[w, h].gameObject.activeSelf ||
                tiles[w, h].currentPotion == null)
                continue;

            PotionController potion = tiles[w, h].currentPotion;

            var vistedTemp = (w, h, potion);

            if (visitedTiles.Contains(vistedTemp))
                continue;

            visitedTiles.Add(vistedTemp);
            yield return DectectPotion(visitedTiles, vistedTemp);
        }

        yield return null;
    }

    private IEnumerator Clear3x3(HashSet<(int, int, PotionController)> visitedTiles,
        (int, int, PotionController) visited)
    {
        Debug.Log("Bomb");
        int wStart = visited.Item1 - 1;
        int wEnd = visited.Item1 + 1;
        int hStart = visited.Item2 - 1;
        int hEnd = visited.Item2 + 1;

        for (int w = wStart; w <= wEnd; w++)
            for (int h = hStart; h <= hEnd; h++)
            {
                if (!ValidIndex(w, h))
                    continue;

                if (!tiles[w, h].gameObject.activeSelf ||
                tiles[w, h].currentPotion == null)
                    continue;

                var vistedTemp = (w, h, tiles[w, h].currentPotion);
                if (visitedTiles.Contains(vistedTemp))
                    continue;

                visitedTiles.Add(vistedTemp);
                yield return DectectPotion(visitedTiles, vistedTemp);
            }

        yield return null;
    }

    private IEnumerator ClearRandomly(HashSet<(int, int, PotionController)> visitedTiles, (int, int, PotionController) visited)
    {
        EPotionColor color = (EPotionColor)UnityEngine.Random.Range((int)EPotionColor.Blue, (int)EPotionColor.Yellow);

        for (int w = 0; w < width; w++)
            for (int h = 0; h < height; h++)
            {
                if (!tiles[w, h].gameObject.activeSelf ||
                    tiles[w, h].currentPotion == null)
                    continue;

                PotionController potion = tiles[w, h].currentPotion;
                var set = (w, h, potion);
                if (potion.getPotionSetting.PotionColor == color && !visitedTiles.Contains(set))
                    visitedTiles.Add(set);
            }

        yield return null;
    }

    private bool ValidIndex(int w, int h)
    {
        return w >= 0 && w < width && h >= 0 && h < height;
    }
}