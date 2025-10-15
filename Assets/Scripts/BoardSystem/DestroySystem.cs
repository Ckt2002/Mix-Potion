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
        {
            foreach ((int w, int h) in match.TargetsIndex)
            {
                if (tiles[w, h].currentPotion == null)
                    continue;

                potionsToDestroy.Add(tiles[w, h].currentPotion);
                tiles[w, h].SetCurrentPotion(null);
            }
        }

        const float duration = 0.15f;
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime / duration;

            foreach (PotionController potion in potionsToDestroy)
                potion.transform.localScale = Vector3.Lerp(potion.transform.localScale, Vector3.zero, timer);

            yield return null;
        }

        // Update to return special and normal potions to Pool
        foreach (PotionController potion in potionsToDestroy)
        {
            if (potion.getPotionSetting.PotionType == EPotionType.Normal)
            {
                poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);
                continue;
            }

            EPotionColor potionColor = potion.getPotionSetting.PotionColor;
            EPotionType potionType = potion.getPotionSetting.PotionType;
            poolController.ReturnSpecialPotion(potionColor, potionType, potion);
        }

        potionsToDestroy.Clear();
        yield break;
    }
}