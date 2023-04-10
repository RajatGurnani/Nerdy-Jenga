using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuReviewPopup : MonoBehaviour
{
    public HotelInformation hotelInformation;
    public GameObject circlePopup;

    public Image popupButtonImage;
    public Sprite buttonNormal;
    public Sprite buttonPress;

    public Vector3 finalSize;
    public float duration;

    private void Awake() => DontDestroyOnLoad(gameObject);
    public void ToggleMenu()
    {
        if (circlePopup.transform.localScale == finalSize)
        {
            popupButtonImage.sprite = buttonNormal;
            circlePopup.transform.DOKill();
            circlePopup.transform.DOScale(Vector3.zero, duration).SetUpdate(true);
        }
        else
        {
            popupButtonImage.sprite = buttonPress;
            circlePopup.transform.DOKill();
            circlePopup.transform.DOScale(finalSize, duration).SetUpdate(true);
        }
    }


    public void OpenMenu() => Application.OpenURL(hotelInformation.url);
    public void OpenReview() => Application.OpenURL(hotelInformation.reviewUrl);
}
