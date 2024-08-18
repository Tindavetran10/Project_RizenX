using UnityEngine;

public class WorldSoundFxManager : MonoBehaviour
{
    public static WorldSoundFxManager instance;

    [Header("Action SFX")]
    public AudioClip rollSfx;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    private void Start() => DontDestroyOnLoad(gameObject);
}
