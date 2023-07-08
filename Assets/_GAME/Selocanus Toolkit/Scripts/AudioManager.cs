using UnityEngine;

namespace SelocanusToolkit
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }
        [Header("References")]
        [SerializeField] AudioSource m_AudioSource;
        [SerializeField] AudioClip m_SuccessSound;
        [SerializeField] AudioClip m_FailureSound;
        [SerializeField] AudioClip m_ConnectSound;
        [SerializeField] AudioClip m_BreakingSound;
        [SerializeField] AudioClip m_LevelCompleteSound;
        [SerializeField] AudioClip m_UIClickSound;

        public void PlayCustomSoundSound(AudioClip customClip)
        {
            m_AudioSource.clip = customClip;
            m_AudioSource.Play();
        }

        public void PlaySuccessSound()
        {
            m_AudioSource.clip = m_SuccessSound;
            m_AudioSource.Play();
        }
        public void PlayFailureSound()
        {
            m_AudioSource.clip = m_FailureSound;
            m_AudioSource.Play();
        }
        public void PlayConnectSound()
        {
            m_AudioSource.clip = m_ConnectSound;
            m_AudioSource.Play();
        }
        public void PlayBreakingSound()
        {
            m_AudioSource.clip = m_BreakingSound;
            m_AudioSource.Play();
        }
        public void PlayLevelCompleteSound()
        {
            m_AudioSource.clip = m_LevelCompleteSound;
            m_AudioSource.Play();
        }

        public void PlayUIClickSound()
        {
            m_AudioSource.clip = m_UIClickSound;
            m_AudioSource.Play();
        }
    }
}


