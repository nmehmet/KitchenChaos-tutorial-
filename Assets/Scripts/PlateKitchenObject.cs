using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectsList;

    private List<KitchenObjectsSO> kitchenObjectsSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectsSO>();
    }
    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectSO)
    {
        if (!validKitchenObjectsList.Contains(kitchenObjectSO)) return false;
        if (kitchenObjectsSOList.Contains(kitchenObjectSO)) return false;
        
        kitchenObjectsSOList.Add(kitchenObjectSO);
        return true;
    }
}
