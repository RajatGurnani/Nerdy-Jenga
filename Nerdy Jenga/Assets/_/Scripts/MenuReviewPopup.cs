using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class MenuReviewPopup : MonoBehaviour
{
    public HotelInformation hotelInformation;
    public GameObject circlePopup;

    public Image popupButtonImage;
    public Sprite buttonNormal;
    public Sprite buttonPress;

    public Vector3 finalSize;
    public float duration;
    public float idleDuration = 3f;

    Coroutine coroutine;

    private void Awake() => DontDestroyOnLoad(gameObject);
    public void ToggleMenu()
    {
        if (circlePopup.transform.localScale == finalSize)
        {
            popupButtonImage.sprite = buttonNormal;
            circlePopup.transform.DOKill();
            circlePopup.transform.DOScale(Vector3.zero, duration).SetUpdate(true);
            StartShrinkMenu();
        }
        else
        {
            popupButtonImage.sprite = buttonPress;
            circlePopup.transform.DOKill();
            circlePopup.transform.DOScale(finalSize, duration).SetUpdate(true);
            StartShrinkMenu();
        }
    }

    IEnumerator ShrinkMenu()
    {
        yield return new WaitForSeconds(idleDuration);
        popupButtonImage.sprite = buttonNormal;
        circlePopup.transform.DOKill();
        circlePopup.transform.DOScale(Vector3.zero, duration).SetUpdate(true);
    }

    public void OpenMenu()
    {
        StartShrinkMenu();
        Application.OpenURL(hotelInformation.url);
        ToggleMenu();
    }

    public void OpenReview()
    {
        StartShrinkMenu();
        Application.OpenURL(hotelInformation.reviewUrl);
        ToggleMenu();
    }

    public void StartShrinkMenu()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(nameof(ShrinkMenu));
    }
}
