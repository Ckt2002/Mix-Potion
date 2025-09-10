using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] SOTilePoolSetting tilePoolSetting;
    [SerializeField] SOPotionPoolSetting potionPoolSetting;
    [SerializeField] SOGameSetting gameSetting;

    public Transform tileParent;
    public Transform potionParent;

    TileController[,] tiles;
    PoolController poolController;
    DragSystem dragSystem;

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
        dragSystem = new DragSystem(tiles, gameSetting.DragDistance);
        ArrangeSystem.ArrangeBoard(tiles, in gameSetting, poolController);
    }

    public void DragPotion(Vector2 pressedPos, Vector2 releasePos, int w, int h)
    {
        StartCoroutine(dragSystem.Drag(pressedPos, releasePos, w, h));
    }
}