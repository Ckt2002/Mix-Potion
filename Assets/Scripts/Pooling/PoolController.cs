using UnityEngine;

public class PoolController
{
    PotionPool potionPool;

    public PoolController(SOPotionPoolSetting potionPoolSetting, Transform potionParent)
    {
        potionPool = new PotionPool(potionPoolSetting.PotionPrefab, potionPoolSetting.SpawnNumber, potionParent);
    }

    public PotionController GetPotion(int index) => potionPool.Get(index);

    public PotionController GetRandomPotion() => potionPool.GetRandomly();

    public void ReturnPotion(int index, PotionController potion) => potionPool.ReturnPotionToQueue(index, potion);
}