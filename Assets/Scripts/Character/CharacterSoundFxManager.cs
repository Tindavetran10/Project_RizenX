using UnityEngine;

namespace Character
{
    public class CharacterSoundFxManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlayRollSoundFX()
        {
            _audioSource.PlayOneShot(WorldSoundFxManager.instance.rollSfx);
        }
    }
}
