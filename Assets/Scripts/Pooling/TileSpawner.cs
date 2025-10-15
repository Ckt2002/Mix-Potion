using UnityEngine;

public class TileSpawner
{
    public TileController[,] SpawnTile(TileController prefab, int width, int height, Transform parent)
    {
        TileController[,] tiles = new TileController[width, height];

        for (int w = 0; w < width; w++)
        for (int h = 0; h < height; h++)
        {
            tiles[w, h] = Object.Instantiate(prefab, parent);
            tiles[w, h].InitTile(w, h);
        }

        return tiles;
    }
}