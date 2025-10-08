using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStripeComboSystem
{
    private static List<TileController> tilesToChange = new();

    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches, Potion potionSetting1, Potion potionSetting2, PoolController poolController)
    {

        PotionMatch match = new PotionMatch()
        {
            ActionType = EActionType.Lightning,
            SourceIndex = (swappedW, swappedH),
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
                if (potionSetting.PotionType == EPotionType.Normal)
                    tilesToChange.Add(tile);
            }
        }

        foreach (TileController tile in tilesToChange)
        {
            EPotionType swipeType = (EPotionType)Random.Range((int)EPotionType.Row, (int)EPotionType.Column + 1);

            PotionController potion = tile.currentPotion;
            poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);
            tile.SetCurrentPotion(null);

            PotionController newPotion = poolController.GetSpecialPotion(colorToGenerate, swipeType);
            newPotion.transform.localPosition = tile.transform.localPosition;
            tile.SetCurrentPotion(newPotion);

            // Spawn and run effects
        }

        yield return new WaitForSeconds(1.5f);

        // Wait for effects complete
        matches.Enqueue(new() { match });
        tilesToChange.Clear();

        yield return null;
    }
}