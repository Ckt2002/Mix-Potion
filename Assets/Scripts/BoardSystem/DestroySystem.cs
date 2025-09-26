using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySystem
{
    private static List<PotionController> potionsToDestroy = new();

    public static IEnumerator Destroy(PoolController poolController, TileController[,] tiles,
        List<PotionMatch> batch)
    {
        foreach (PotionMatch match in batch)
            foreach ((int w, int h) in match.TargetIndex)
            {
                potionsToDestroy.Add(tiles[w, h].currentPotion);
                tiles[w, h].SetCurrentPotion(null);
            }

        float duration = 0.15f;
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime / duration;

            foreach (PotionController potion in potionsToDestroy)
                potion.transform.localScale = Vector3.Lerp(potion.transform.localScale, Vector3.zero, timer);

            yield return null;
        }

        // Update to return special and normal potions
        foreach (PotionController potion in potionsToDestroy)
        {
            poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);
        }

        potionsToDestroy.Clear();
        yield break;
    }
}