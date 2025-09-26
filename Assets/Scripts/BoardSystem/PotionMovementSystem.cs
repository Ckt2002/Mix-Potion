using System.Collections;
using UnityEngine;

public class PotionMovementSystem
{
    public static IEnumerator SwapPotion(TileController tile1, TileController tile2,
        PotionController potion1, PotionController potion2)
    {
        Vector3 tile1Pos = tile1.transform.localPosition;
        Vector3 tile2Pos = tile2.transform.localPosition;

        float duration = 0.15f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            potion1.transform.localPosition =
                Vector3.Lerp(tile1Pos, tile2Pos, t);
            potion2.transform.localPosition =
                Vector3.Lerp(tile2Pos, tile1Pos, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
    }
}