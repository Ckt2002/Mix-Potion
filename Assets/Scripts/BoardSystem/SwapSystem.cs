using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapSystem
{
    private TileController[,] tiles;
    private int width, height;
    private HashSet<(int, int, PotionController)> visitedTiles;

    private CheckMatchSystem checkMatchSystem;
    private MatchHandlerSystem matchHandlerSystem;
    private ComboSystem comboSystem;

    public SwapSystem(TileController[,] tiles, PoolController poolController)
    {
        this.tiles = tiles;
        width = tiles.GetLength(0);
        height = tiles.GetLength(1);
        visitedTiles = new();
        checkMatchSystem = new CheckMatchSystem();
        matchHandlerSystem = new MatchHandlerSystem(tiles, poolController);
        comboSystem = new ComboSystem(tiles, visitedTiles, poolController);
    }

    public IEnumerator SwapPotion(int w, int h, int swappedW, int swappedH)
    {
        if (!ValidSwap(w, h, swappedW, swappedH))
            yield break;

        GameState.Interactable = false;

        yield return MovePotion(tiles[w, h], tiles[swappedW, swappedH]);
        SwitchTile(w, h, swappedW, swappedH);

        yield return comboSystem.CheckCombo(w, h, swappedW, swappedH);
        bool comboSwapped = true;

        if (visitedTiles.Count == 0)
        {
            checkMatchSystem.CheckMatchAfterSwap(tiles, w, h, swappedW, swappedH, visitedTiles);
            comboSwapped = false;
        }

        if (visitedTiles.Count >= 3)
            yield return matchHandlerSystem.MatchHandle(visitedTiles, comboSwapped);
        else
        {
            yield return MovePotion(tiles[w, h], tiles[swappedW, swappedH]);
            SwitchTile(w, h, swappedW, swappedH);
        }

        visitedTiles.Clear();
        GameState.Interactable = true;
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

        yield return new WaitForSeconds(0.1f);
    }

    private void SwitchTile(int w, int h, int swappedW, int swappedH)
    {
        PotionController swappedPotion = tiles[swappedW, swappedH].currentPotion;
        tiles[swappedW, swappedH].SetCurrentPotion(tiles[w, h].currentPotion);
        tiles[w, h].SetCurrentPotion(swappedPotion);
    }

    private bool ValidIndex(int w, int h, int swappedW, int swappedH)
    {
        bool validW = w > 0 || w < width;
        bool validH = h > 0 || h < height;
        bool validSwappedW = swappedW > 0 || swappedW < width;
        bool validSwappedH = swappedH > 0 || swappedH < height;
        return validW && validH && validSwappedW && validSwappedH;
    }

    private bool ValidSwap(int w, int h, int swappedW, int swappedH)
    {
        return ValidIndex(w, h, swappedW, swappedH) && tiles[w, h].currentPotion != null &&
            tiles[swappedW, swappedH].currentPotion != null;
    }
}