using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripeComboSystem
{
    public static IEnumerator FindMatch(TileController[,] tiles, int w, int h, int swappedW, int swappedH,
        Queue<List<PotionMatch>> matches,
        PoolController poolController, TileController tile1, TileController tile2, Potion potionSetting1,
        Potion potionSetting2)
    {
        List<PotionMatch> matchesList = new List<PotionMatch>();

        if (potionSetting1.PotionType == potionSetting2.PotionType)
        {
            PotionController potion = tile1.currentPotion;
            EPotionType typeToChange = EPotionType.Row;
            if (potionSetting1.PotionType == EPotionType.Row)
                typeToChange = EPotionType.Column;

            float duration = 0.1f;
            float timer = 0f;

            while (timer < 1f)
            {
                timer += Time.deltaTime / duration;
                potion.transform.localScale = Vector3.Lerp(potion.transform.localScale, Vector3.zero, timer);
                yield return null;
            }

            poolController.ReturnSpecialPotion(potionSetting1.PotionColor, potionSetting1.PotionType, potion);

            PotionController newPotion = poolController.GetSpecialPotion(potionSetting1.PotionColor, typeToChange);
            newPotion.transform.localPosition = tile1.transform.localPosition;
            tile1.SetCurrentPotion(newPotion);

            yield return new WaitForSeconds(1.5f);
        }

        if (potionSetting2.PotionType == EPotionType.Row)
        {
            matchesList.Add(FindWipeRow(tiles, swappedW, swappedH, tiles.GetLength(0), w, h));
            matchesList.Add(FindWipeCol(tiles, w, h, tiles.GetLength(1), swappedW, swappedH));
        }
        else
        {
            matchesList.Add(FindWipeRow(tiles, w, h, tiles.GetLength(0), w, h));
            matchesList.Add(FindWipeCol(tiles, swappedW, swappedH, tiles.GetLength(1), swappedW, swappedH));
        }

        matches.Enqueue(matchesList);

        yield break;
    }

    private static PotionMatch FindWipeRow(TileController[,] tiles, int w, int h, int width, int avoidW, int avoidH)
    {
        SpecialPotion potion = (SpecialPotion)tiles[w, h].currentPotion;
        potion.ActivateSpecial();

        PotionMatch match = new PotionMatch
        {
            ActionType = EActionType.Swipe,
            SourceIndex = (w, h),
            TargetsIndex = new(),
        };

        for (int wTemp = 0; wTemp < width; wTemp++)
        {
            TileController tile = tiles[wTemp, h];
            if (!CheckValidSystem.ValidTile(tile) ||
                wTemp == avoidW && h == avoidH)
                continue;

            match.TargetsIndex.Add((wTemp, h));
        }

        return match;
    }

    private static PotionMatch FindWipeCol(TileController[,] tiles, int w, int h, int height, int avoidW, int avoidH)
    {
        SpecialPotion potion = (SpecialPotion)tiles[w, h].currentPotion;
        potion.ActivateSpecial();

        PotionMatch match = new PotionMatch
        {
            ActionType = EActionType.Swipe,
            SourceIndex = (w, h),
            TargetsIndex = new(),
        };

        for (int hTemp = 0; hTemp < height; hTemp++)
        {
            TileController tile = tiles[w, hTemp];
            if (!CheckValidSystem.ValidTile(tile) ||
                w == avoidW && hTemp == avoidH)
                continue;

            match.TargetsIndex.Add((w, hTemp));
        }

        return match;
    }
}