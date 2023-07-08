using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoxes
{
    /// <summary>
    /// Called when the box is attached to a container.
    /// </summary>
    public void onAttach();

    /// <summary>
    /// Called when the box is detached from a container.
    /// </summary>
    public void onDetach();

}
