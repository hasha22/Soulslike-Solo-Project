using SaveGameManager;
using Unity.Netcode;
using UnityEngine;

namespace StartGame
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.NewGame();
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

