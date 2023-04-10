using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomizeGame : MonoBehaviour
{
    public HotelInformation hotelInfo;
    public TMP_Text hotelName;

    private void Start()
    {
        
    }

    private void Awake()
    {
        hotelName.text = hotelInfo.hotelName;
    }
}
