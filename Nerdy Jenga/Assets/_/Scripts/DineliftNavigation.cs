using DG.Tweening;
using UnityEngine;

public class DineliftNavigation : MonoBehaviour
{
    public HotelInformation hotelInformation;
    public float transitionDuration = 1f;

    public GameObject gameSelectionPanel;
    public GameObject selectionPanel;
    public GameObject reviewMenuPopup;

    public void OpenSelectionMenu()
    {
        selectionPanel.SetActive(true);
        gameSelectionPanel.SetActive(false);
    }

    public void OpenGameSelectionMenu()
    {
        gameSelectionPanel.SetActive(true);
        selectionPanel.SetActive(false);
    }

    public void OpenTallTales()
    {
        transform.DOMoveY(-Screen.height, transitionDuration).OnComplete(() => gameObject.SetActive(false)).OnComplete(() => reviewMenuPopup.SetActive(true));
    }

    public void OpenMenu() => Application.OpenURL(hotelInformation.url);
    public void OpenReview() => Application.OpenURL(hotelInformation.reviewUrl);
}
