using System;
using System.Text;
using UnityEngine;

public class PoolController
{
    PotionPool normalPotionPool;
    PotionPool specialPotionPool;

    public PoolController(SOPotionPoolSetting normalPotionPoolSetting,
        SOPotionPoolSetting specialPotionPoolSetting, Transform potionParent)
    {
        normalPotionPool = new PotionPool(normalPotionPoolSetting.PotionPrefab,
            normalPotionPoolSetting.SpawnNumber, potionParent);

        specialPotionPool = new PotionPool(specialPotionPoolSetting.PotionPrefab,
            specialPotionPoolSetting.SpawnNumber, potionParent);
    }

    public PotionController GetNormalPotion(int index)
        => normalPotionPool.Get(index);

    public PotionController GetRandomNormalPotion()
        => normalPotionPool.GetRandomly();

    public void ReturnNormalPotion(int index, PotionController potion)
        => normalPotionPool.ReturnPotionToQueue(index, potion);

    public PotionController GetSpecialPotion(EPotionColor potionColor, EPotionType potionType)
    {
        StringBuilder name = new StringBuilder();
        if (potionType == EPotionType.Bomb || potionType == EPotionType.Lightning)
            name.Append(potionType.ToString());
        else if (potionType == EPotionType.Column || potionType == EPotionType.Row)
        {
            name.Append(potionColor.ToString());
            name.Append(potionType.ToString());
        }
        Enum.TryParse<ESpecialPotion>(name.ToString(), out ESpecialPotion resultEnum);
        return specialPotionPool.Get((int)resultEnum);
    }

    public void ReturnSpecialPotion(EPotionColor potionColor, EPotionType potionType, PotionController potion)
    {
        StringBuilder name = new StringBuilder();

        if (potionType == EPotionType.Bomb || potionType == EPotionType.Lightning)
            name.Append(potionType.ToString());
        else if (potionType == EPotionType.Column || potionType == EPotionType.Row)
        {
            name.Append(potionColor.ToString());
            name.Append(potionType.ToString());
        }

        Enum.TryParse<ESpecialPotion>(name.ToString(), out ESpecialPotion resultEnum);
        specialPotionPool.ReturnPotionToQueue((int)resultEnum, potion);
    }
}