using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController Instance;

    TileController[,] tiles;
    int width, height;

    PoolController poolController;
    SOGameSetting gameSetting;

    Queue<List<PotionMatch>> matchBatches;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        matchBatches = new();
    }

    public void SetupBoard(TileController[,] tiles, SOGameSetting gameSetting, PoolController poolController)
    {
        this.tiles = tiles;

        width = tiles.GetLength(0);
        height = tiles.GetLength(1);

        this.poolController = poolController;
        this.gameSetting = gameSetting;

        BoardArrangerSystem.ArrangeTiles(tiles, gameSetting, poolController);
    }

    public IEnumerator ProcessMove(Vector2 clickedPos, Vector2 releasePos, int w, int h)
    {
        bool swapped = true;
        (int w, int h) swappedIndex = (0, 0);

        yield return SwapSystem.Swap(clickedPos, releasePos, gameSetting.DragThreshold, w, h, width, height, tiles,
            (value) => { swappedIndex.w = value.Item1; swappedIndex.h = value.Item2; }, (value) => swapped = value);

        if (!swapped)
            yield break;

        yield return ComboSystem.DetectCombo(tiles, w, h, swappedIndex.w, swappedIndex.h, matchBatches, poolController);

        if (matchBatches.Count == 0)
            yield return SwapSystem.Swap(clickedPos, releasePos, gameSetting.DragThreshold,
                w, h, width, height, tiles);
        else
        {
            yield return ExecuteSystem.ExecuteMatchPotions(tiles, w, h, swappedIndex.w, swappedIndex.h, matchBatches, poolController);

            yield return RefillSystem.RefillBoard(tiles, width, height, poolController);
        }

        yield return null;
    }
}