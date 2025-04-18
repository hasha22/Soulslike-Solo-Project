using StartGame;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveGameManager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance { get; private set; }
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerManager player;
        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSaveData currentCharacterData;
        public CharacterSlots currentCharacterSlot;
        public string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;
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
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            player = GetComponent<PlayerManager>();
        }
        private void Update()
        {

            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }
        public string DecideCharacterFileName(CharacterSlots characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlots.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlots.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlots.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlots.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlots.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlots.CharacterSlot_06:
                    fileName = "CharacterSlot_06";
                    break;
                case CharacterSlots.CharacterSlot_07:
                    fileName = "CharacterSlot_07";
                    break;
                case CharacterSlots.CharacterSlot_08:
                    fileName = "CharacterSlot_08";
                    break;
                case CharacterSlots.CharacterSlot_09:
                    fileName = "CharacterSlot_09";
                    break;
                case CharacterSlots.CharacterSlot_10:
                    fileName = "CharacterSlot_10";
                    break;
            }
            return fileName;
        }
        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_01);
            saveFileDataWriter.saveFileName = saveFileName;

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 1 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_02);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 2 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_03);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 3 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_04);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 4 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_05);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 5 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_06);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 6 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_07);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 7 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_08);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 8 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_09);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 9 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_10);
            saveFileDataWriter.saveFileName = saveFileName;
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                Debug.Log("File 10 doesn't exist. Creating character file...");
                currentCharacterSlot = CharacterSlots.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            TitleScreenManager.instance.DisplayPopUp();
        }
        public void LoadGame()
        {
            saveFileName = DecideCharacterFileName(currentCharacterSlot);
            saveFileDataWriter = new SaveFileDataWriter();

            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            currentCharacterData = saveFileDataWriter.LoadSaveFile();
            StartCoroutine(LoadWorldScene());
        }
        public void SaveGame()
        {
            saveFileName = DecideCharacterFileName(currentCharacterSlot);
            // Check if this is the local player
            PlayerManager localPlayer = null;
            PlayerManager[] allPlayers = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);

            foreach (var playerManager in allPlayers)
            {
                Debug.Log(allPlayers.Count());
                PlayerNetworkManager networkManager = playerManager.GetComponent<PlayerNetworkManager>();
                if (networkManager.IsServer)
                {
                    localPlayer = playerManager;
                    break;
                }
            }
            if (localPlayer == null)
            {
                Debug.Log("Cannot save as client. You must return to your world first.");
                return;
            }

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            localPlayer.SavePlayerData(ref currentCharacterData);

            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }
        public void DeleteGame(CharacterSlots characterSlot)
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = DecideCharacterFileName(characterSlot);

            saveFileDataWriter.DeleteSaveFile();
        }
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_02);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_04);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_05);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_06);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_07);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_08);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_09);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlots.CharacterSlot_10);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
        }
        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            //This is a temporary fix. If the player starts a new game, currentCharacterData's values are null.
            if (currentCharacterData.vigor != 0)
            {
                Debug.Log("Loading Character...");
                player.LoadPlayerData(ref currentCharacterData);
            }
            else
            {
                player.CreatePlayerData(ref currentCharacterData);
            }

            yield return null;
        }
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

