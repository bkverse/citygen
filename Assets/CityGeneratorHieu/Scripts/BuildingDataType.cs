using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingDataType
{
    [SerializeField]
    private GameObject[] prefabs;
    public int sizeRequiredX;
    public int sizeRequiredY;
    public int quantity;
    private int quantityAlreadyPlaced;

    public GameObject GetPrefab()
    {
        quantityAlreadyPlaced++;
        // TODO: apply rules get random building here
        if (prefabs.Length > 1)
        {
            var random = UnityEngine.Random.Range(0, prefabs.Length);
            return prefabs[random];
        }
        return prefabs[0];
    }

    public bool IsBuildingAvailable()
    {
        return quantityAlreadyPlaced < quantity;
    }

    public void Reset()
    {
        quantityAlreadyPlaced = 0;
    }
}
