using System.Collections;
using UnityEngine;

public class SwapSystem
{
    TileController[,] tiles;

    public SwapSystem(TileController[,] tiles)
    {
        this.tiles = tiles;
    }

    public IEnumerator SwapPotion(int w, int h, int swappedW, int swappedH)
    {
        if (!ValidIndex(w, h, swappedW, swappedH))
            yield break;

        yield return MovePotion(tiles[w, h], tiles[swappedW, swappedH]);

        PotionController swappedPotion = tiles[swappedW, swappedH].currentPotion;

        tiles[swappedW, swappedH].SetCurrentPotion(tiles[w, h].currentPotion);
        tiles[w, h].SetCurrentPotion(swappedPotion);
    }

    private IEnumerator MovePotion(TileController tile, TileController swappedTile)
    {
        Vector3 tile1Pos = tile.transform.localPosition;
        Vector3 tile2Pos = swappedTile.transform.localPosition;

        float duration = 0.15f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            tile.currentPotion.transform.position =
                Vector3.Lerp(tile1Pos, tile2Pos, t);
            swappedTile.currentPotion.transform.position =
                Vector3.Lerp(tile2Pos, tile1Pos, t);
            yield return null;
        }
    }

    private bool ValidIndex(int w, int h, int swappedW, int swappedH)
    {
        bool validW = w > 0 || w < tiles.GetLength(0);
        bool validH = h > 0 || h < tiles.GetLength(1);
        bool validSwappedW = swappedW > 0 || swappedW < tiles.GetLength(0);
        bool validSwappedH = swappedH > 0 || swappedH < tiles.GetLength(1);
        return validW && validH && validSwappedW && validSwappedH;
    }
}