using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableBox : Box, IBoxes
{
    public enum Abilities { canJump, canPull }

    public Abilities abilities;
    
        public void onAttach()
        {
            switch (abilities)
            {
                case Abilities.canJump:
                    Player.Instance.CanJump = true;
                    break;
                case Abilities.canPull:
                    Player.Instance.CanPull = true;
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
                    Player.Instance.CanJump = false;
                    break;
                case Abilities.canPull:
                    Player.Instance.CanPull = false;
                    break;
                default:
                    break;
            }
        }
    
    //When boxes connect together, apply effect here
    public override void ApplyBoxEffect()
    {

    }
    
}
