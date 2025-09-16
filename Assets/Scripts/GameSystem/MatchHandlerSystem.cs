using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandlerSystem
{
    private PoolController poolController;
    private RefillBoardSystem refillBoardSystem;

    public MatchHandlerSystem(PoolController poolController)
    {
        this.poolController = poolController;
        refillBoardSystem = new RefillBoardSystem();
    }

    public IEnumerator MatchHandle(TileController[,] tiles, HashSet<(int, int)> visited)
    {
        List<PotionController> a = new();
        foreach ((int w, int h) in visited)
        {
            a.Add(tiles[w, h].currentPotion);
            tiles[w, h].SetCurrentPotion(null);
        }

        yield return DestroyPotion(a);

        // Refill potion
        yield return refillBoardSystem.RefillBoard(tiles, poolController);
    }

    private IEnumerator DestroyPotion(List<PotionController> potions)
    {
        float duration = 0.1f;
        float t = 0f;

        Dictionary<PotionController, Vector3> startScales = new();
        foreach (var potion in potions)
            startScales[potion] = potion.transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            foreach (var potion in potions)
                potion.transform.localScale = Vector3.Lerp(startScales[potion], Vector3.zero, t);

            yield return null;
        }

        foreach (var potion in potions)
        {
            // Return potion to pool
            poolController.ReturnPotion((int)potion.potion.PotionColor, potion);
            potion.gameObject.SetActive(false);
        }
    }
}