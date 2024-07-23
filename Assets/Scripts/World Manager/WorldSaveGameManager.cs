using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World_Manager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        [SerializeField] private int worldScreenIndex;
    
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start() => DontDestroyOnLoad(gameObject);

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldScreenIndex);
            yield return null;
        }
    
        public int GetWorldSceneIndex() => worldScreenIndex;
    }
}
