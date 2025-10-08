using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerateSpecialPotion
{
    public static IEnumerator DetectSpecialPotions(HashSet<(int, int)> targetIndexes, TileController[,] tiles,
        Dictionary<(int, int), (EPotionColor, EPotionType)> specialToSpawn)
    {
        var rows = targetIndexes.GroupBy(index => index.Item2);
        var cols = targetIndexes.GroupBy(index => index.Item1);

        foreach (var r in rows.Where(g => g.Count() >= 3))
            foreach (var c in cols.Where(g => g.Count() >= 3))
            {
                var intersect = r.Intersect(c);
                if (intersect.Any())
                {
                    var (w, h) = intersect.First();
                    specialToSpawn.Add((w, h), (EPotionColor.None, EPotionType.Bomb));
                    yield break;
                }
            }

        var rowGroups = rows.Select(group => group.OrderBy(item => item.Item1).ToList()).ToList();
        var colGroups = cols.Select(group => group.OrderBy(item => item.Item1).ToList()).ToList();

        var maxRowGroup = rowGroups.OrderByDescending(g => g.Count).First();
        var maxColGroup = colGroups.OrderByDescending(g => g.Count).First();

        var (wRow, hRow) = rowGroups.First().ElementAt(rowGroups.First().Count() / 2);
        var (wCol, hCol) = colGroups.First().ElementAt(colGroups.First().Count() / 2);

        if (rowGroups.Any(r => r.Count() >= 5))
            specialToSpawn.Add((wRow, hRow), (EPotionColor.None, EPotionType.Lightning));
        else if (rowGroups.Any(r => r.Count() == 4))
        {
            EPotionColor color = tiles[wRow, hRow].currentPotion.getPotionSetting.PotionColor;
            EPotionType type = (EPotionType)UnityEngine.Random.Range((int)EPotionType.Row, (int)EPotionType.Column + 1);
            specialToSpawn.Add((wRow, hRow), (color, type));
        }

        if (colGroups.Any(c => c.Count() >= 5))
            specialToSpawn.Add((wCol, hCol), (EPotionColor.None, EPotionType.Lightning));
        else if (colGroups.Any(c => c.Count() == 4))
        {
            EPotionColor color = tiles[wCol, hCol].currentPotion.getPotionSetting.PotionColor;
            EPotionType type = (EPotionType)UnityEngine.Random.Range((int)EPotionType.Row, (int)EPotionType.Column + 1);
            specialToSpawn.Add((wCol, hCol), (color, type));
        }
    }


    public static IEnumerator Generate(TileController[,] tiles, PoolController poolController,
        Dictionary<(int, int), (EPotionColor, EPotionType)> specialToSpawn)
    {
        if (specialToSpawn.Count == 0)
            yield break;

        foreach (var item in specialToSpawn)
        {
            PotionController potion = poolController.GetSpecialPotion(item.Value.Item1, item.Value.Item2);
            TileController tile = tiles[item.Key.Item1, item.Key.Item2];
            potion.transform.localPosition = tile.transform.localPosition;
            tile.SetCurrentPotion(potion);
        }
    }
}