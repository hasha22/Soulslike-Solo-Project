using SaveGameManager;
using Unity.Netcode;
using UnityEngine;
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] public bool startGameAsClient;

    [HideInInspector] public PlayerUIHUDManager playerUIHUDManager;

    //Temporary client data
    private SaveFileDataWriter saveFileDataWriter;
    [HideInInspector] public CharacterSaveData clientCharacterData;

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

            //Temporary fix to have client data loaded properly

            WorldSaveGameManager.instance.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(WorldSaveGameManager.instance.currentCharacterSlot);
            saveFileDataWriter = new SaveFileDataWriter();

            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.saveFileName;

            clientCharacterData = saveFileDataWriter.LoadSaveFile();
            NetworkManager.Singleton.StartClient();
        }
    }
}
