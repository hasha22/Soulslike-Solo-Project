using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveGameManager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance { get; private set; }

        [SerializeField] int worldSceneIndex = 1;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

