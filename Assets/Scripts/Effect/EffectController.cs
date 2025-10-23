using UnityEngine;

public class EffectController : MonoBehaviour
{
    public EEffectType effectType;
    public float maxExistTime = 0.5f;
    private float existTime = 0;

    void OnEnable()
    {
        existTime = 0;
    }

    public void UpdateExistTime(float deltaTime)
    {
        existTime += deltaTime;
    }

    public bool ReachMaxExistTime => existTime >= maxExistTime;
}