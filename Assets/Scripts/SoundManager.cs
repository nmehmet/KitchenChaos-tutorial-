using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField, Range(0f, 1f)] float volume;

    public static SoundManager Instance { get; private set; }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnObjectPlacedHere += BaseCounter_OnObjectPlacedHere;
        TrashCan.OnAnyObjectTrashed += TrashCan_OnAnyObjectTrashed;
        PlateKitchenObject.OnAnyIngredientAdded += PlateKitchenObject_OnAnyIngredientAdded;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void PlateKitchenObject_OnAnyIngredientAdded(object sender, System.EventArgs e)
    {
        PlateKitchenObject plateKitchenObject = sender as PlateKitchenObject;
        PlaySound(audioClipRefsSO.objectPickup, plateKitchenObject.transform.position);
    }

    private void TrashCan_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCan trashCan = sender as TrashCan;
        PlaySound(audioClipRefsSO.trash, trashCan.transform.position);
    }

    private void BaseCounter_OnObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootstepsSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.footstep, position);
    }
}
