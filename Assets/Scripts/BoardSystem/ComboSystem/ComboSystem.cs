using System.Collections;
using System.Collections.Generic;

public class ComboSystem
{
    public static IEnumerator DetectCombo(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matchBatches)
    {
        TileController tile1 = tiles[w, h];
        TileController tile2 = tiles[swappedW, swappedH];
        Potion potionSetting1 = tile1.currentPotion.getPotionSetting;
        Potion potionSetting2 = tile2.currentPotion.getPotionSetting;
        EPotionType type1 = potionSetting1.PotionType;
        EPotionType type2 = potionSetting2.PotionType;

        if (BothNormal(type1, type2))
        {
            yield return NormalComboSystem.FindMatch(tiles, w, h, swappedW, swappedH, matchBatches);
            yield break;
        }

        yield break;
    }

    private static bool BothNormal(EPotionType type1, EPotionType potion2)
        => type1 == EPotionType.Normal && potion2 == EPotionType.Normal;
}