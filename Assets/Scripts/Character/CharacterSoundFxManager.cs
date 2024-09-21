using UnityEngine;
using World_Manager;

namespace Character
{
    public class CharacterSoundFxManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        protected virtual void Awake() => _audioSource = GetComponent<AudioSource>();
        
        public void PlaySoundFX(AudioClip soundFX, float volume = 1, 
            bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            _audioSource.PlayOneShot(soundFX, volume);
            // Reset pitch to 1
            _audioSource.pitch = 1;

            if (randomizePitch) 
                _audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
        
        public void PlayRollSoundFX() => _audioSource.PlayOneShot(WorldSoundFxManager.instance.rollSfx);
    }
}
