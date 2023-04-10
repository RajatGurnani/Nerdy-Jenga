using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPopup : MonoBehaviour
{
    public GameObject doneButton;
    public float delay = 5f;

    private void OnEnable()
    {
        doneButton.SetActive(false);
        StartCoroutine(nameof(DoneDelay));
    }

    IEnumerator DoneDelay()
    {
        yield return new WaitForSeconds(delay);
        doneButton.SetActive(true);
    }

    private void OnDisable()
    {
        doneButton.SetActive(false);
    }
}
