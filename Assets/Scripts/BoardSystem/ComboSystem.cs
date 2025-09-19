using System.Collections;
using System.Collections.Generic;

public class ComboSystem
{
    TileController[,] tiles;
    ComboHandler comboHandler;

    public ComboSystem(TileController[,] tiles, HashSet<(int, int, PotionController)> visitedTiles,
        PoolController poolController)
    {
        this.tiles = tiles;
        comboHandler = new ComboHandler(tiles, visitedTiles, poolController);
    }

    public IEnumerator CheckCombo(int w, int h, int swappedW, int swappedH)
    {
        PotionController potion1 = tiles[w, h].currentPotion;
        PotionController potion2 = tiles[swappedW, swappedH].currentPotion;
        Potion potionSetting1 = potion1.getPotionSetting;
        Potion potionSetting2 = potion2.getPotionSetting;
        EPotionType potionType1 = potionSetting1.PotionType;
        EPotionType potionType2 = potionSetting2.PotionType;

        if (BothLightning(potionType1, potionType2))
        {
            yield return comboHandler.ClearBoard();
            yield break;
        }

        if (BothBomb(potionType1, potionType2))
        {
            yield return comboHandler.BigBang(w, h, swappedW, swappedH);
            yield break;
        }

        if (BothRowCol(potionType1, potionType2))
        {
            yield return comboHandler.Plus(w, h, swappedW, swappedH, potionType1 == potionType2);
            yield break;
        }

        if (LightningBomb(potionType1, potionType2))
        {
            yield return comboHandler.ExplodeRandomly();
            yield break;
        }

        if (LightningRowCol(potionType1, potionType2))
        {
            if (potionType1 == EPotionType.Lightning)
                yield return comboHandler.SwipeRandomly(potionSetting2.PotionColor);
            else
                yield return comboHandler.SwipeRandomly(potionSetting1.PotionColor);
            yield break;
        }

        if (LightningNormal(potionType1, potionType2))
        {
            if (potionType1 == EPotionType.Lightning)
                yield return comboHandler.DestroyRandomly(potionSetting2.PotionColor);
            else
                yield return comboHandler.DestroyRandomly(potionSetting1.PotionColor);
            yield break;
        }

        if (BombRowCol(potionType1, potionType2))
        {
            if (potionType1 == EPotionType.Bomb)
                yield return comboHandler.TripleSwipe(swappedW, swappedH, potionSetting2.PotionType);
            else
                yield return comboHandler.TripleSwipe(w, h, potionSetting1.PotionType);
            yield break;
        }
    }

    private bool BothLightning(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Lightning && type2 == EPotionType.Lightning;

    private bool BothBomb(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Bomb && type2 == EPotionType.Bomb;

    private bool BothRowCol(EPotionType type1, EPotionType type2)
    {
        bool RowCol = type1 == EPotionType.Row && type2 == EPotionType.Column ||
            type1 == EPotionType.Column && type2 == EPotionType.Row;
        bool RowRow = type1 == EPotionType.Row && type2 == EPotionType.Row ||
            type1 == EPotionType.Row && type2 == EPotionType.Row;
        bool ColCol = type1 == EPotionType.Column && type2 == EPotionType.Column ||
            type1 == EPotionType.Column && type2 == EPotionType.Column;

        return RowCol || ColCol || RowRow;
    }

    private bool LightningBomb(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Lightning && type2 == EPotionType.Bomb ||
        type1 == EPotionType.Bomb && type2 == EPotionType.Lightning;

    private bool LightningRowCol(EPotionType type1, EPotionType type2)
    {
        bool lightningRow = type1 == EPotionType.Lightning && type2 == EPotionType.Row ||
            type1 == EPotionType.Row && type2 == EPotionType.Lightning;
        bool lightningCol = type1 == EPotionType.Lightning && type2 == EPotionType.Column ||
            type1 == EPotionType.Column && type2 == EPotionType.Lightning;

        return lightningRow || lightningCol;
    }

    private bool LightningNormal(EPotionType type1, EPotionType type2)
        => type1 == EPotionType.Lightning && type2 == EPotionType.Normal ||
        type1 == EPotionType.Normal && type2 == EPotionType.Lightning;

    private bool BombRowCol(EPotionType type1, EPotionType type2)
    {
        bool BombRow = type1 == EPotionType.Bomb && type2 == EPotionType.Row ||
            type1 == EPotionType.Row && type2 == EPotionType.Bomb;
        bool BombCol = type1 == EPotionType.Bomb && type2 == EPotionType.Column ||
            type1 == EPotionType.Column && type2 == EPotionType.Bomb;

        return BombRow || BombCol;
    }
}