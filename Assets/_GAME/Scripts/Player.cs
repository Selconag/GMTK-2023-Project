using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the weapon is enabled for the player.
    /// </summary>
    [Header("Variables")]
    [SerializeField] bool m_WeaponEnabled;

    /// <summary>
    /// Indicates whether the references are enabled for the player.
    /// </summary>
    [Header("References")]
    public bool m_References;

    /// <summary>
    /// transform of the weapon slot
    /// </summary>
    [SerializeField] Transform m_ObjectSlot;

    /// <summary>
    /// Character Controller component attached to the player.
    /// </summary>
    public CharacterController m_Controller;

    /// <summary>
    /// An instance for the Player class for Singleton.
    /// </summary>
    public static Player instance;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Equips a game object by assigning its transform to the object slot when ItemBox attaches to CharacterBox.
    /// </summary>
    /// <param name="go">The game object to equip.</param>
    public void Equip(GameObject go)
    {
        m_ObjectSlot = go.transform;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
