using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBombComboSystem
{
    private static List<TileController> tilesToChange = new();

    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches, Potion potionSetting2, PoolController poolController)
    {

        PotionMatch match = new PotionMatch()
        {
            ActionType = EActionType.None,
            SourceIndex = (w, h),
            TargetsIndex = new()
        };

        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        match.TargetsIndex.Add((w, h));
        match.TargetsIndex.Add((swappedW, swappedH));

        ((SpecialPotion)tiles[w, h].currentPotion).ActivateSpecial();
        ((SpecialPotion)tiles[swappedW, swappedH].currentPotion).ActivateSpecial();

        if (potionSetting2.PotionType == EPotionType.Lightning)
            match.SourceIndex = (swappedW, swappedH);

        EPotionColor color = (EPotionColor)Random.Range((int)EPotionColor.Blue, (int)EPotionColor.Orange + 1);

        for (int wTemp = 0; wTemp < width; wTemp++)
        {
            for (int hTemp = 0; hTemp < height; hTemp++)
            {
                TileController tile = tiles[wTemp, hTemp];
                if (!CheckValidSystem.ValidTile(tile))
                    continue;

                Potion potionSetting = tile.currentPotion.getPotionSetting;
                if (potionSetting.PotionColor != color || potionSetting.PotionType != EPotionType.Normal)
                    continue;

                match.TargetsIndex.Add((wTemp, hTemp));
                tilesToChange.Add(tile);
            }
        }

        foreach (TileController tile in tilesToChange)
        {
            PotionController potion = tile.currentPotion;
            poolController.ReturnNormalPotion((int)potion.getPotionSetting.PotionColor, potion);
            tile.SetCurrentPotion(null);

            PotionController newPotion = poolController.GetSpecialPotion(EPotionColor.None, EPotionType.Bomb);
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