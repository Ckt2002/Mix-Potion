using UnityEngine;

public class ArrangerSystem
{
    public static void ArrangeTile(TileController[,] tiles, int width, int height, float spacing)
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                Vector3 newPosition = new Vector3(w * spacing, h * spacing, 0f);

                tiles[w, h].transform.localPosition = newPosition;
            }
        }
    }
}
