using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Pool Setting", menuName = "My Asset/Create Effect Pool Setting")]
public class SOEffectPoolSetting : ScriptableObject
{
    public GameObject[] Prefab;
    public int SpawnNumber;
}