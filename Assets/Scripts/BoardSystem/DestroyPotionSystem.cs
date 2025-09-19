using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPotionSystem
{
    private TileController[,] tiles;
    private PoolController poolController;

    public DestroyPotionSystem(TileController[,] tiles, PoolController poolController)
    {
        this.tiles = tiles;
        this.poolController = poolController;
    }

    public IEnumerator DestroyPotion(List<PotionController> potions)
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
            if (potion.getPotionSetting.PotionType == EPotionType.Normal)
                poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);
            else
                poolController.ReturnSpecialPotion(potion.getPotionSetting.PotionColor,
                    potion.getPotionSetting.PotionType, potion);
            potion.gameObject.SetActive(false);
        }
    }
}