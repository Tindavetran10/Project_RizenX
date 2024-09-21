using UnityEngine;

namespace World_Manager
{
    public class WorldSoundFxManager : MonoBehaviour
    {
        public static WorldSoundFxManager instance;

        [Header("Damage SFX")]
        public AudioClip[] physicalDamageSfx;
    
        [Header("Action SFX")]
        public AudioClip rollSfx;
    
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else Destroy(gameObject);
        }

        private void Start() => DontDestroyOnLoad(gameObject);

        public static AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            var index = Random.Range(0, array.Length);
            return array[index];
        }
    }
}
