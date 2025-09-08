using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Pool Setting", menuName = "My Asset/Create Potion Pool Setting")]
public class SOPotionPoolSetting : ScriptableObject
{
    public PotionController[] PotionPrefab;
    public int SpawnNumber;
}
