using System;
using UnityEngine;
using UnityEngine.Events;

namespace SelocanusToolkit
{
    [System.Serializable]
    public class NotifierInstance : UnityEvent { }
    //The BAse Notifier Class, It notifies a target object(Script's) target method
    [Serializable]
    public class NotifierBase : MonoBehaviour
    {
        [SerializeField] NotifierInstance m_NotifierTarget;
        public virtual void NotifyTarget()
        {
            m_NotifierTarget.Invoke();
        }

    }
}
