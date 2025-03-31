using Unity.Netcode;
using UnityEngine;
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] public bool startGameAsClient;

    [HideInInspector] public PlayerUIHUDManager playerUIHUDManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}
