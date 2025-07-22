using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectsSO_GameObject> kitchenObjectsSOGameObjectList;

    [Serializable]
    public struct KitchenObjectsSO_GameObject
    {
        public KitchenObjectsSO kitchenObjectsSO;
        public GameObject gameObject;
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectsSO_GameObject kitchenObjectsSOGameObject in kitchenObjectsSOGameObjectList)
        {
                kitchenObjectsSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectsSO_GameObject kitchenObjectsSOGameObject in kitchenObjectsSOGameObjectList)
        {
            if(kitchenObjectsSOGameObject.kitchenObjectsSO == e.kitchenObjectsSO)
            {
                kitchenObjectsSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
