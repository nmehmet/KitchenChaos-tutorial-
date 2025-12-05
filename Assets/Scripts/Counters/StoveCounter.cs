using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgresChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = 1f });
    }
    private void Update()
    {
       
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });
                    }
                    break;

                case State.Burned:
                    break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {//There is not a kithen Object
            if (player.HasKitchenObject())
            {//Player has an kitchen object
                if (HasFryingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                }
                else if (HasBurningRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    burningRecipeSO = GetBurningRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    state = State.Fried;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    burningTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });
                }
            }
        }
        else
        {//There is a kitchen object
            if (!player.HasKitchenObject())
            {//player has not got a kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = 1f });
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
            else
            {//player has a kitchen object
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player has a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgresChangedEventArgs { progressNormalized = 1f });
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                }
            }
        }
    }
    private bool HasFryingRecipeWithInput(KitchenObjectsSO input)
    {
        return GetFryingRecipeWithInput(input) != null; 
    }
    private FryingRecipeSO GetFryingRecipeWithInput(KitchenObjectsSO input)
    {
        foreach (FryingRecipeSO recipe in fryingRecipeSOArray)
        {
            if (recipe.input == input) return recipe;
        }
        return null;
    }
    private bool HasBurningRecipeWithInput(KitchenObjectsSO input)
    {
        return GetBurningRecipeWithInput(input) != null;
    }
    private BurningRecipeSO GetBurningRecipeWithInput(KitchenObjectsSO input)
    {
        foreach (BurningRecipeSO recipe in burningRecipeSOArray)
        {
            if (recipe.input == input) return recipe;
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
