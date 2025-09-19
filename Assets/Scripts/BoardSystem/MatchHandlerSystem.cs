using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchHandlerSystem
{
    private TileController[,] tiles;
    private PoolController poolController;
    private RefillBoardSystem refillBoardSystem;
    private DestroyPotionSystem destroyPotionSystem;
    private ActivePotionSystem activePotionSystem;
    private List<(int, int, EPotionType, PotionController)> specialPoints;

    public MatchHandlerSystem(TileController[,] tiles, PoolController poolController)
    {
        this.tiles = tiles;
        this.poolController = poolController;
        specialPoints = new();
        refillBoardSystem = new RefillBoardSystem();
        destroyPotionSystem = new DestroyPotionSystem(tiles, poolController);
        activePotionSystem = new ActivePotionSystem(tiles);
    }

    public IEnumerator MatchHandle(HashSet<(int, int, PotionController)> visitedTiles, bool comboSwapped)
    {
        if (!comboSwapped)
            yield return DetectSpecialPieces(visitedTiles);

        yield return activePotionSystem.ActiveAllPotions(visitedTiles);

        List<PotionController> potions = new List<PotionController>();
        foreach (var (w, h, potion) in visitedTiles)
        {
            potions.Add(potion);
            tiles[w, h].SetCurrentPotion(null);
        }

        yield return destroyPotionSystem.DestroyPotion(potions);

        foreach (var (w, h, potionType, potion) in specialPoints)
        {
            var spawnedPotion = poolController.GetSpecialPotion(potion.getPotionSetting.PotionColor, potionType);
            spawnedPotion.transform.localPosition = tiles[w, h].transform.localPosition;
            tiles[w, h].SetCurrentPotion(spawnedPotion);
        }

        yield return new WaitForSeconds(0.1f);

        yield return refillBoardSystem.RefillBoard(tiles, poolController);

        specialPoints.Clear();
    }

    private IEnumerator DetectSpecialPieces(HashSet<(int, int, PotionController)> visitedTiles)
    {
        List<(int, int, PotionController)> checkedLines = new();
        var rowMatches = visitedTiles.GroupBy(t => new { t.Item1, t.Item3.getPotionSetting.PotionColor });
        var colMatches = visitedTiles.GroupBy(t => new { t.Item2, t.Item3.getPotionSetting.PotionColor });

        foreach (var row in rowMatches.Where(g => g.Count() >= 3))
            foreach (var col in colMatches.Where(g => g.Count() >= 3))
                if (row.Intersect(col).Any() && !col.Any(checkedLines.Contains) && !row.Any(checkedLines.Contains))
                {
                    checkedLines.AddRange(row);
                    checkedLines.AddRange(col);

                    var a = row.Intersect(col).First();
                    specialPoints.Add((a.Item1, a.Item2, EPotionType.Bomb, a.Item3));
                }

        foreach (var row in rowMatches.Where(g => g.Count() >= 5))
            if (!row.Any(checkedLines.Contains))
            {
                checkedLines.AddRange(row);
                var middlePotion = row.OrderBy(t => t.Item1).ElementAt(row.Count() / 2);
                specialPoints.Add((middlePotion.Item1, middlePotion.Item2, EPotionType.Lightning, middlePotion.Item3));
            }

        foreach (var col in colMatches.Where(g => g.Count() >= 5))
            if (!col.Any(checkedLines.Contains))
            {
                checkedLines.AddRange(col);
                var middlePotion = col.OrderBy(t => t.Item2).ElementAt(col.Count() / 2);
                specialPoints.Add((middlePotion.Item1, middlePotion.Item2, EPotionType.Lightning, middlePotion.Item3));
            }

        foreach (var row in rowMatches.Where(g => g.Count() == 4))
            if (!row.Any(checkedLines.Contains))
            {
                checkedLines.AddRange(row);
                var middlePotion = row.OrderBy(t => t.Item1).ElementAt(row.Count() / 2);
                EPotionType type = (EPotionType)Random.Range(1, 3);
                specialPoints.Add((middlePotion.Item1, middlePotion.Item2, type, middlePotion.Item3));
            }

        foreach (var col in colMatches.Where(g => g.Count() == 4))
            if (!col.Any(checkedLines.Contains))
            {
                checkedLines.AddRange(col);
                var middlePotion = col.OrderBy(t => t.Item2).ElementAt(col.Count() / 2);
                EPotionType type = (EPotionType)Random.Range(1, 3);
                specialPoints.Add((middlePotion.Item1, middlePotion.Item2, type, middlePotion.Item3));
            }
        yield return null;
    }
}