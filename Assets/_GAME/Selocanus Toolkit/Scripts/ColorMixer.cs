using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ColorMixer", menuName = "ColorObjects/ColorMixGroup", order = 3)]
public class ColorMixer : ScriptableObject
{
    [SerializeField] public List<ColorMixGroup> colorGroups;
    //[SerializeField] ColorGroup Black, Red, None, Green, Yellow, Blue, Magenta, None2, Cyan, White, None3;
}

[System.Serializable]
public class ColorMixGroup
{
    public string Color_A;
    public string Color_B;
    public string Color_Result_Index;
    public Color Color_Result;
}   
