using UnityEngine;

public abstract class PotionController : MonoBehaviour
{
    [SerializeField] protected Potion potionSetting;

    public Potion getPotionSetting => potionSetting;

    public abstract void DestroyPotion();
}