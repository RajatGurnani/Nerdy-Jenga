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
    public Color highlightColor;

    public enum Choice { Food, Drink, FoodNDrink };
    public Choice choice;
    public List<string> drinks = new List<string>();
    public List<string> drinkMessage = new List<string>();
    public List<string> food = new List<string>();
    public List<string> foodMessage = new List<string>();

    public string GiveMessage()
    {
        string item = string.Empty;
        string message = string.Empty;
        System.Random rand = new System.Random();
        switch (choice)
        {
            case Choice.Food:
                item = food[rand.Next(food.Count)];
                message = foodMessage[rand.Next(foodMessage.Count)];
                break;
            case Choice.Drink:
                item = drinks[rand.Next(drinks.Count)];
                message = drinkMessage[rand.Next(drinkMessage.Count)];
                break;
            case Choice.FoodNDrink:
                if (Random.Range(0, 2) == 1)
                {
                    item = food[rand.Next(food.Count)];
                    message = foodMessage[rand.Next(foodMessage.Count)];
                }
                else
                {
                    item = drinks[rand.Next(drinks.Count)];
                    message = drinkMessage[rand.Next(drinkMessage.Count)];
                }
                break;
        }
        item = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{item}</color>";
        return string.Format(message, item);
    }
}
