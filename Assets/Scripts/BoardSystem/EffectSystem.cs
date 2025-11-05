using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public static EffectSystem instance;

    PoolController poolController;
    List<EffectController> effects;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            effects = new List<EffectController>();
        }
        else
            Destroy(this);
    }

    void Start()
    {
        poolController = PoolController.instance;
    }

    void Update()
    {
        if (effects.Count > 0)
            TimeExistCal(Time.deltaTime);
    }

    private void TimeExistCal(float deltaTime)
    {
        int index = 0;

        while (index < effects.Count)
        {
            EffectController effect = effects[index];
            if (effect.ReachMaxExistTime)
            {
                effects.Remove(effect);
                effect.gameObject.SetActive(false);
                poolController.ReturnEffect(effect.effectType, effect.gameObject);
            }
            else
            {
                effect.UpdateExistTime(deltaTime);
                index++;
            }
        }
    }

    public void AddSpawnedEffect(GameObject effect)
    {
        effects.Add(effect.GetComponent<EffectController>());
    }
}