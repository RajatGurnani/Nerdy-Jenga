using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DineliftNavigation : MonoBehaviour
{
    public const string RATE_US_UNLOCKED = "RATE_US_UNLOCKED";
    public Button rateUsButton;
    public bool rateUsUnlocked = false;

    public HotelInformation hotelInformation;
    public float transitionDuration = 1f;

    public GameObject gameSelectionPanel;
    public GameObject selectionPanel;
    public GameObject reviewMenuPopup;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(RATE_US_UNLOCKED))
        {
            PlayerPrefs.SetInt(RATE_US_UNLOCKED, 0);
            PlayerPrefs.Save();
        }
        rateUsUnlocked = PlayerPrefs.GetInt(RATE_US_UNLOCKED, 0) == 1;
        if (!rateUsUnlocked)
        {
            rateUsButton.interactable = false;
            rateUsButton.gameObject.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        }
    }

    public void MakeRateUsInteractable()
    {
        if (!rateUsUnlocked)
        {
            rateUsUnlocked = true;
            PlayerPrefs.SetInt(RATE_US_UNLOCKED, 1);
            PlayerPrefs.Save();
            rateUsButton.interactable = true;
            rateUsButton.gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public void OpenSelectionMenu()
    {
        selectionPanel.SetActive(true);
        gameSelectionPanel.SetActive(false);
    }

    public void OpenGameSelectionMenu()
    {
        Invoke(nameof(MakeRateUsInteractable), 1f);
        gameSelectionPanel.SetActive(true);
        selectionPanel.SetActive(false);
    }

    public void OpenTallTales()
    {
        transform.DOMoveY(-Screen.height, transitionDuration).OnComplete(() => gameObject.SetActive(false)).OnComplete(() => reviewMenuPopup.SetActive(true));
    }

    public void OpenMenu()
    {
        Invoke(nameof(MakeRateUsInteractable), 1f);
        Application.OpenURL(hotelInformation.url);
    }

    public void OpenReview() => Application.OpenURL(hotelInformation.reviewUrl);
}
