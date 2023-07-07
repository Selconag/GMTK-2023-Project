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

        [SerializeField] AudioSource AudioSource;
        [SerializeField] AudioClip SuccessSound;
        [SerializeField] AudioClip FailureSound;
        [SerializeField] AudioClip MergeSound;
        [SerializeField] AudioClip WrongMergeSound;
        [SerializeField] AudioClip LevelCompleteSound;
        [SerializeField] AudioClip UIClickSound;

        public void PlayCustomSoundSound(AudioClip customClip)
        {
            AudioSource.clip = customClip;
            AudioSource.Play();
        }

        public void PlaySuccessSound()
        {
            AudioSource.clip = SuccessSound;
            AudioSource.Play();
        }
        public void PlayFailureSound()
        {
            AudioSource.clip = FailureSound;
            AudioSource.Play();
        }
        public void PlayMergeSound()
        {
            AudioSource.clip = MergeSound;
            AudioSource.Play();
        }
        public void PlayWrongMergeSound()
        {
            AudioSource.clip = WrongMergeSound;
            AudioSource.Play();
        }
        public void PlayLevelCompleteSound()
        {
            AudioSource.clip = LevelCompleteSound;
            AudioSource.Play();
        }

        public void PlayUIClickSound()
        {
            AudioSource.clip = UIClickSound;
            AudioSource.Play();
        }
    }
}


