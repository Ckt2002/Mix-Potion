using System.Collections.Generic;
using UnityEngine;

public class EffectPool
{
    GameObject[] prefabs;
    Color[] color;
    int spawnNumber;
    Transform parent;
    Dictionary<int, Queue<GameObject>> effectDict;

    public EffectPool(GameObject[] effectPrefabs, Color[] color, int spawnNumber, Transform parent)
    {
        prefabs = effectPrefabs;
        this.color = color;
        this.spawnNumber = spawnNumber;
        this.parent = parent;
        effectDict = new Dictionary<int, Queue<GameObject>>();

        for (int i = 0; i < prefabs.Length; i++)
            effectDict.Add(i, CreateEffectQueue(prefabs[i]));
    }

    public Queue<GameObject> CreateEffectQueue(GameObject prefab)
    {
        Queue<GameObject> spawned = new();
        for (int i = 0; i < spawnNumber; i++)
            spawned.Enqueue(SpawnNewEffect(prefab));
        return spawned;
    }

    public GameObject SpawnNewEffect(GameObject potionPrefab)
    {
        GameObject newPotion = Object.Instantiate(potionPrefab, parent);
        newPotion.gameObject.SetActive(false);
        return newPotion;
    }

    public GameObject GetNormalEffect(int index)
    {
        GameObject effect = effectDict[index].Count > 0 ? effectDict[index].Dequeue() : SpawnNewEffect(prefabs[index]);

        ParticleSystem particle = effect.GetComponent<ParticleSystem>();
        var main = particle.main;
        main.startColor = color[index];
        return effect;
    }

    public GameObject Get(int index)
    {
        GameObject effect = effectDict[index].Count > 0 ? effectDict[index].Dequeue() : SpawnNewEffect(prefabs[index]);

        return effect;
    }

    public void ReturnEffectToQueue(int index, GameObject effect)
    {
        effect.SetActive(false);
        if (effectDict.ContainsKey(index))
            effectDict[index].Enqueue(effect);
    }
}