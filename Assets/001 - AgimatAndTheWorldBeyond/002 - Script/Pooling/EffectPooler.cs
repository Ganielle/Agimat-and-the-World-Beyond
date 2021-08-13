using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooler : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    [Header("Debugger")]
    [ReadOnly] public GameObject currentObjSelectedOnPool;

    private void Awake()
    {
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(effectPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        currentObjSelectedOnPool = instance;
        instance.SetActive(true);

        return instance;
    }
}
