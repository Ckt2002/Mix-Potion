using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Scriptable Object")]
    [SerializeField] SOTilePoolSetting tilePoolSetting;
    [SerializeField] SOGameSetting gameSetting;

    [Header("Parent Transform")]
    public Transform tileParent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        TileController[,] tiles = new TileSpawner().SpawnTile(tilePoolSetting.TilePrefab,
            gameSetting.Width, gameSetting.Height, tileParent);

        PoolController poolController = PoolController.instance;

        BoardController.Instance.SetupBoard(tiles, gameSetting, poolController);
    }
}