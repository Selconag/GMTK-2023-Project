using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Box,IBoxes
{
    public GameObject go;
    public void onAttach()
    {
        Player.instance.Equip(go);
    }

    public void onDetach()
    {
        Player.instance.Unequip(go);
    }
    //When boxes connect together, apply effect here
    public override void ApplyBoxEffect()
    {

    }
}
