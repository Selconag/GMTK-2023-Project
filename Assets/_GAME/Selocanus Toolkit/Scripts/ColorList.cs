using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ColorGroup", menuName = "ColorObjects/ColorGroups", order = 1)]
public class ColorList : ScriptableObject
{
    [SerializeField] public List<ColorObjects> ColorObjects;
    //[SerializeField] ColorGroup Black, Red, None, Green, Yellow, Blue, Magenta, None2, Cyan, White, None3;
}

public enum ColorIndex { Black, Red, None, Green, Yellow, Blue, Magenta, None2, Cyan, White, None3 }
[System.Serializable]
public class ColorObjects
{
    public string ColorIndex;
    public Color Color;
}
