using UnityEngine;

public struct ArrangerSystem
{
    public void ArrangeTile(in int height, in int width, TileController[,] tiles)
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Vector3 newPosition = new Vector3(w * 1, h * 1, 0f);

                tiles[h, w].transform.localPosition = newPosition;
            }
        }
    }
}
