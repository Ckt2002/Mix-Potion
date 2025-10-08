using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionTransformChangeSystem
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

    public static IEnumerator MoveStraightDown(List<(Vector3, Transform)> potionsToMove)
    {
        int arrMax = potionsToMove.Count;
        Vector3[] potionsOriginalPos = new Vector3[arrMax];

        int i = 0;

        foreach ((Vector3 targetPos, Transform potionTransform) in potionsToMove)
        {
            potionsOriginalPos[i] = potionTransform.localPosition;
            i++;
        }

        float duration = 0.15f;
        float t = 0f;

        while (t < 1f)
        {
            i = 0;
            t += Time.deltaTime / duration;

            foreach ((Vector3 targetPos, Transform potionTransform) in potionsToMove)
            {
                potionTransform.localPosition = Vector3.Lerp(potionsOriginalPos[i], targetPos, t);
                i++;
            }

            yield return null;
        }

        yield break;
    }
}