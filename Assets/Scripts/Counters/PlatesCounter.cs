using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;
    [SerializeField] private float spawnPlateTimerMax;
    [SerializeField] private int platesSpawnedAmountMax;
    private float spawnPlateTimer;
    private int platesSpawnedAmount;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax )
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if(platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            }
        }
    }
}
