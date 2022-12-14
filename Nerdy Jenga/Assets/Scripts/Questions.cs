using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Questions Bank", fileName = "Generic Question Bank")]
public class Questions : ScriptableObject
{
    public List<string> question;
}