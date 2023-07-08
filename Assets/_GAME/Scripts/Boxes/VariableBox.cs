using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableBox : Box, IBoxes
{
    public enum Abilities { canJump, canPull }

    public Abilities abilities;
    public void onAttach() { }
    public void onDetach() { }
    /*
        public void onAttach()
        {
            switch (abilities)
            {
                case Abilities.canJump:
                    Player.instance.canJump = true;
                    break;
                case Abilities.canPull:
                    Player.instance.canPull = true;
                    break;
                default:
                    break;
            }
        }

        public void onDetach()
        {
            switch (abilities)
            {
                case Abilities.canJump:
                    Player.instance.canJump = false;
                    break;
                case Abilities.canPull:
                    Player.instance.canPull = false;
                    break;
                default:
                    break;
            }
        }
    */
    //When boxes connect together, apply effect here
    public override void ApplyBoxEffect()
    {

    }
    
}
