using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip uiClick;

    public void PlayUIClickSound() => audioSource.PlayOneShot(uiClick);
}
