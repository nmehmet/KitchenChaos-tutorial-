using System;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter , IHasProgress
{
    public event EventHandler<IHasProgress.OnProgresChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {//There is not any kitchen object
            if (player.HasKitchenObject())
            {//player has a kitchen object
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                { 
                    player.GetKitchenObject().SetKitchenObjectParent(this); 
                    cuttingProgress = 0;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = 0f });
                }
            }
        }
        else
        {//there is a kitchen object
            if (!player.HasKitchenObject())
            {//player does not carry a kitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = 1f });
            }
            else
            {//player carries a KitchenObejct
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player has a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = 1f });
                    }
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
            if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO()))
            {
                cuttingProgress++;
                OnCut?.Invoke(this, EventArgs.Empty);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = (float)cuttingProgress / GetCuttingProgressMax(GetKitchenObject().GetKitchenObjectsSO()) });
                
            if (cuttingProgress >= GetCuttingProgressMax(GetKitchenObject().GetKitchenObjectsSO()))
                {
                    KitchenObjectsSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectsSO());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(output, this);
                }
            }
    }

    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO input)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.input == input) return recipe.output;
        }
        return null;
    }
    private bool HasRecipeWithInput(KitchenObjectsSO input)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.input == input) return true;
        }
        return false;
    }
    private int GetCuttingProgressMax(KitchenObjectsSO input)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.input == input) return recipe.cuttingProgressMax;
        }
        return 0;
    }
}
