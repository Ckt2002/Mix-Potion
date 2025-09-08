using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameController Instance;

    [SerializeField] SOTilePoolSetting tilePoolSetting;
    [SerializeField] SOPotionPoolSetting potionPoolSetting;
    [SerializeField] SOGameSetting gameSetting;

    public Transform tileParent;
    public Transform potionParent;

    TileController[,] tiles;
    PoolController poolController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        tiles = new TileSpawner().SpawnTile(tilePoolSetting.TilePrefab, gameSetting.Width, gameSetting.Height, tileParent);
        poolController = new PoolController(potionPoolSetting, potionParent);
        ArrangerSystem.ArrangeTile(tiles, gameSetting.Width, gameSetting.Height, gameSetting.Spacing);
    }
}