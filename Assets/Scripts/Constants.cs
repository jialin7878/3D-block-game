using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Constants")]
public class Constants : ScriptableObject
{
    public string PlayfabID;

    public static Color inactiveColor;
    public static Color activeColor;
}
