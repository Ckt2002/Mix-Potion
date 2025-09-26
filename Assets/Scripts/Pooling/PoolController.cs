using System;
using System.Text;
using UnityEngine;

public class PoolController
{
    PotionPool normalPotionPool;
    PotionPool specialPotionPool;
    EffectPool effectPool;

    public PoolController(SOPotionPoolSetting normalPotionPoolSetting,
        SOPotionPoolSetting specialPotionPoolSetting,
        SOEffectPoolSetting effectPoolSetting,
        SOParticleColor effectColor,
        Transform potionParent, Transform effectParent)
    {
        normalPotionPool = new PotionPool(normalPotionPoolSetting.Prefab,
            normalPotionPoolSetting.SpawnNumber, potionParent);

        specialPotionPool = new PotionPool(specialPotionPoolSetting.Prefab,
            specialPotionPoolSetting.SpawnNumber, potionParent);

        effectPool = new EffectPool(effectPoolSetting.Prefab, effectColor.color,
            effectPoolSetting.SpawnNumber, effectParent);
    }

    public PotionController GetNormalPotion(int color)
        => normalPotionPool.Get(color);

    public PotionController GetRandomNormalPotion()
        => normalPotionPool.GetRandomly();

    public void ReturnNormalPotion(int color, PotionController potion)
        => normalPotionPool.ReturnPotionToQueue(color, potion);

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

    public GameObject GetNormalEffect(EEffectType effectType)
        => effectPool.GetNormalEffect((int)effectType);

    public GameObject GetSpecialEffect(EEffectType effectType)
        => effectPool.Get((int)effectType);

    public void ReturnEffect(EEffectType effectType, GameObject effect)
        => effectPool.ReturnEffectToQueue((int)effectType, effect);
}