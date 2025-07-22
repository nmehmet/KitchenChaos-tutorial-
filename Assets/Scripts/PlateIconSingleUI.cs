using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{

    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjectsSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
