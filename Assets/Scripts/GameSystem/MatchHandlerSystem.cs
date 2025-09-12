using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandlerSystem
{
    public IEnumerator MatchHandle(TileController[,] tiles, HashSet<(int, int)> visited)
    {
        List<PotionController> a = new();
        foreach ((int w, int h) in visited)
            a.Add(tiles[w, h].currentPotion);

        yield return DestroyPotion(a);
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
            {
                potion.transform.localScale = Vector3.Lerp(startScales[potion], Vector3.zero, t);
            }
            yield return null;
        }
    }
}