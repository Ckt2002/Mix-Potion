using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillBoardSystem
{
    public IEnumerator RefillBoard(TileController[,] tiles, PoolController poolController)
    {
        yield return MovePotionDown(tiles);

        Queue<((int, int), PotionController)> emptyTiles = new();

        for (int w = 0; w < tiles.GetLength(0); w++)
            for (int h = 0; h < tiles.GetLength(1); h++)
            {
                if (tiles[w, h].currentPotion != null || !tiles[w, h].gameObject.activeSelf)
                    continue;

                PotionController potion = poolController.GetRandomNormalPotion();
                potion.transform.localPosition = tiles[w, tiles.GetLength(1) - 1].transform.localPosition;
                emptyTiles.Enqueue(((w, h), potion));
            }

        yield return MovePotion(tiles, emptyTiles);
    }

    public IEnumerator MovePotionDown(TileController[,] tiles)
    {
        Queue<((int, int), PotionController)> emptyTiles = new();
        for (int w = 0; w < tiles.GetLength(0); w++)
            for (int h = 0; h < tiles.GetLength(1); h++)
            {
                if (tiles[w, h].currentPotion != null || !tiles[w, h].gameObject.activeSelf)
                    continue;

                for (int hTemp = h + 1; hTemp < tiles.GetLength(1); hTemp++)
                {
                    if (tiles[w, hTemp].currentPotion == null || !tiles[w, h].gameObject.activeSelf)
                        continue;

                    emptyTiles.Enqueue(((w, h), tiles[w, hTemp].currentPotion));
                    tiles[w, hTemp].SetCurrentPotion(null);
                    break;
                }
            }

        yield return MovePotion(tiles, emptyTiles);

        yield return null;
    }

    public IEnumerator MovePotion(TileController[,] tiles, Queue<((int, int), PotionController)> emptyTiles)
    {
        float duration = 1f;
        float t = 0f;

        foreach (var ((w, h), potion) in emptyTiles)
            tiles[w, h].SetCurrentPotion(potion);

        while (t < duration + 0.5f)
        {
            t += Time.deltaTime / duration;
            foreach (var ((w, h), potion) in emptyTiles)
                potion.transform.localPosition = Vector3.Lerp(potion.transform.localPosition, tiles[w, h].transform.localPosition, t);

            yield return null;
        }
    }
}