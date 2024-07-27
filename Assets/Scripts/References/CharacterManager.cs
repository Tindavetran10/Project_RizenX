using UnityEngine;

namespace References
{
    public class CharacterManager : MonoBehaviour
    {
        protected virtual void Awake() => DontDestroyOnLoad(this);

        protected virtual void Update()
        {
            
        }
    }
}
