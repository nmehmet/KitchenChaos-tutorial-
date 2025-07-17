using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectsSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {//there is no kitchen object here 
            if (player.HasKitchenObject())
            {//player has a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {//there is a kitchen object here
            if(!player.HasKitchenObject())
            {//player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            } 
            else
            {//player caries something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player has a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    } 
                }
                else
                {//player holding something different from plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {//counter holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectsSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            }
            
        }
    }
}
