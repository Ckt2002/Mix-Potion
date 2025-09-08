using System.Collections.Generic;
using UnityEngine;

public class PotionPool
{
    PotionController[] potionPrefabs;
    int spawnNumber;
    Transform parent;
    Dictionary<int, Queue<PotionController>> potionDict;

    public PotionPool(PotionController[] potionPrefabs, int spawnNumber, Transform parent)
    {
        this.potionPrefabs = potionPrefabs;
        this.spawnNumber = spawnNumber;
        this.parent = parent;
        potionDict = new();

        for (int i = 0; i < potionPrefabs.Length; i++)
        {
            potionDict.Add(i, CreatePotionQueue(potionPrefabs[i]));
        }
    }

    public Queue<PotionController> CreatePotionQueue(PotionController potionPrefab)
    {
        Queue<PotionController> spawnedPotion = new();
        for (int i = 0; i < spawnNumber; i++)
            spawnedPotion.Enqueue(SpawnNewPotion(potionPrefab));
        return spawnedPotion;
    }

    public PotionController SpawnNewPotion(PotionController potionPrefab)
    {
        PotionController newPotion = GameObject.Instantiate(potionPrefab, parent);
        newPotion.gameObject.SetActive(false);
        return newPotion;
    }

    public PotionController Get(int index)
    {
        PotionController potion = null;

        if (potionDict[index].Count > 0)
            potion = potionDict[index].Dequeue();
        else
            potion = SpawnNewPotion(potionPrefabs[index]);

        potion.gameObject.SetActive(true);
        return potion;
    }

    public PotionController GetRandomly()
    {
        PotionController potion = null;
        int index = Random.Range(0, potionPrefabs.Length);

        if (potionDict[index].Count > 0)
            potion = potionDict[index].Dequeue();
        else
            potion = SpawnNewPotion(potionPrefabs[index]);

        potion.gameObject.SetActive(true);
        return potion;
    }

    public void ReturnPotionToQueue(int index, PotionController potion)
    {
        potion.gameObject.SetActive(false);
        potionDict[index].Enqueue(potion);
    }
}