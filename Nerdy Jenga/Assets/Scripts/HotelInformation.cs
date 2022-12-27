using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hotel Description", fileName = "Hotel Name")]
public class HotelInformation : ScriptableObject
{
    public string hotelName;
    public string hotelDescription;
    public string hotelType;
    public string url;
    public List<string> items = new List<string>();
    public List<string> drinks = new List<string>();
    public List<string> drinkMessage= new List<string>();
}
