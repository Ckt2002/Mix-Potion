using UnityEngine;

public class PotionController : MonoBehaviour
{
    [SerializeField] Potion potionSetting;

    public Potion potion => potionSetting;
}