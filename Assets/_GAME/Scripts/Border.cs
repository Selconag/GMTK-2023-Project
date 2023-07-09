using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] bool m_KillerBorder;

    public void KillHittingObject(Collider target)
    {   
        if(target == null) return;
        GameManager.Instance.ReLoadScene();
    }
}
