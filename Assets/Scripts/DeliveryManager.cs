using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance {  get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO =  recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.name);
                waitingRecipeSOList.Add(waitingRecipeSO);
                Debug.Log(waitingRecipeSOList);

                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {// Cycle trough ALL RECIPES
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if(waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectsSOList().Count)
            {// Has the same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectsSO recipeKitchenObejctSO in waitingRecipeSO.kitchenObjectsSOList)
                {// Cycling trough all ingredients in RECIPE
                    bool ingredientFound = false;
                    foreach(KitchenObjectsSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectsSOList())
                    {// Cycling trough all ingredients in PLATE
                        if(plateKitchenObjectSO == recipeKitchenObejctSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {// The recipe ingredients was not found on the plate
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe)
                {// Player delivered the correct recipe
                    Debug.Log("Player delivered the correct recipe");
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    return;
                }
            }
        }

        //No mathes fount
        //Player did not deliver a correct recipe
        Debug.Log("Player did not deliver a correct recipe");
    }
    
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    } 
}
