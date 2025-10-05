using System.Collections;
using System.Collections.Generic;

public class ComboSystem
{
    public static IEnumerator DetectCombo(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matchBatches, PoolController poolController)
    {
        TileController tile1 = tiles[w, h];
        TileController tile2 = tiles[swappedW, swappedH];

        Potion potionSetting1 = tile1.currentPotion.getPotionSetting;
        Potion potionSetting2 = tile2.currentPotion.getPotionSetting;

        EPotionType type1 = potionSetting1.PotionType;
        EPotionType type2 = potionSetting2.PotionType;

        if (BothNormal(type1, type2) || NormalStripe(type1, type2) || NormalBomb(type1, type2))
        {
            yield return NormalComboSystem.FindMatch(tiles, w, h, swappedW, swappedH, matchBatches);
            yield break;
        }

        if (BothStripe(type1, type2))
        {
            yield return StripeComboSystem.FindMatch(tiles, w, h, swappedW, swappedH, matchBatches,
                poolController, tile1, tile2, potionSetting1, potionSetting2);
            yield break;
        }

        yield break;
    }

    #region Combo Conditions
    private static bool BothNormal(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Normal && type2 == EPotionType.Normal;

    private static bool NormalStripe(EPotionType type1, EPotionType type2)
    {
        bool norRow = type1 == EPotionType.Normal && type2 == EPotionType.Row ||
            type1 == EPotionType.Row && type2 == EPotionType.Normal;
        bool norCol = type1 == EPotionType.Normal && type2 == EPotionType.Column ||
            type1 == EPotionType.Column && type2 == EPotionType.Normal;

        return norRow || norCol;
    }

    private static bool NormalBomb(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Normal && type2 == EPotionType.Bomb ||
            type1 == EPotionType.Bomb && type2 == EPotionType.Normal;

    private static bool BothBomb(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Bomb && type2 == EPotionType.Bomb;

    private static bool BothStripe(EPotionType type1, EPotionType type2)
    {
        bool type1Stripe = type1 == EPotionType.Row || type1 == EPotionType.Column;
        bool type2Stripe = type2 == EPotionType.Row || type2 == EPotionType.Column;

        return type1Stripe && type2Stripe;
    }
    #endregion
}