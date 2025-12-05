using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if(countdownNumber != previousCountdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlaytCountDownSound();

        }
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
