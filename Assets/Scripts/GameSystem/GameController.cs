using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Scriptable Object")] [SerializeField]
    SOTilePoolSetting tilePoolSetting;

    [SerializeField] SOPotionPoolSetting normalPotionPoolSetting;
    [SerializeField] SOPotionPoolSetting specialPotionPoolSetting;
    [SerializeField] SOEffectPoolSetting effectPoolSetting;
    [SerializeField] SOParticleColor particleColor;
    [SerializeField] SOGameSetting gameSetting;

    [Header("Parent Transform")] public Transform tileParent;
    public Transform potionParent;
    public Transform effectParent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        var tiles = new TileSpawner().SpawnTile(tilePoolSetting.TilePrefab,
            gameSetting.Width, gameSetting.Height, tileParent);

        var poolController = new PoolController(normalPotionPoolSetting,
            specialPotionPoolSetting, effectPoolSetting,
            particleColor, potionParent, effectParent);

        BoardController.Instance.SetupBoard(tiles, gameSetting, poolController);
    }
}