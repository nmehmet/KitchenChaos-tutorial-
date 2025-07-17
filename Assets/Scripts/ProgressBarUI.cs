using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) Debug.LogError("Game Object " + hasProgressGameObject + "does not have a component that implements IHasProgress");

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgresChangedEventArgs e)
    {
            barImage.fillAmount = e.progressNormalized;
        if (barImage.fillAmount == 1f) Hide(); else Show(); 
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
