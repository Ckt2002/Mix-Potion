using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightNormalComboSystem
{
    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches, Potion potionSetting1, Potion potionSetting2)
    {
        PotionMatch match = new PotionMatch()
        {
            ActionType = EActionType.Lightning,
            SourceIndex = (w, h),
            TargetsIndex = new()
        };

        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        match.TargetsIndex.Add((w, h));
        match.TargetsIndex.Add((swappedW, swappedH));

        EPotionColor colorToGenerate = potionSetting1.PotionColor;

        if (potionSetting2.PotionType != EPotionType.Lightning)
        {
            colorToGenerate = potionSetting2.PotionColor;
            match.SourceIndex = (w, h);
        }

        for (int wTemp = 0; wTemp < width; wTemp++)
        {
            for (int hTemp = 0; hTemp < height; hTemp++)
            {
                TileController tile = tiles[wTemp, hTemp];
                if (!CheckValidSystem.ValidTile(tile))
                    continue;

                Potion potionSetting = tile.currentPotion.getPotionSetting;
                if (potionSetting.PotionColor != colorToGenerate)
                    continue;

                match.TargetsIndex.Add((wTemp, hTemp));
            }
        }

        matches.Enqueue(new() { match });
        yield break;
    }
}