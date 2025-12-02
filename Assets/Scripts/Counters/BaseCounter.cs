using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    public static event EventHandler OnObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnObjectPlacedHere = null;
    }

    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(Player player)
    {

    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) OnObjectPlacedHere?.Invoke(this, EventArgs.Empty);   
    }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
