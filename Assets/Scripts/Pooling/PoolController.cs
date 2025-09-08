using UnityEngine;

public class PoolController
{
    PotionPool potionPool;

    public PoolController(SOPotionPoolSetting potionPoolSetting, Transform potionParent)
    {
        potionPool = new PotionPool(potionPoolSetting.PotionPrefab, potionPoolSetting.SpawnNumber, potionParent);
    }
}