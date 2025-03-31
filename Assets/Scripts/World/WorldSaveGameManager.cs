using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveGameManager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
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
        private string saveFileName;

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
            switch (characterSlot)
            {
                case CharacterSlots.CharacterSlot_01:
                    saveFileName = "CharacterSlot_01";
                    break;
                case CharacterSlots.CharacterSlot_02:
                    saveFileName = "CharacterSlot_02";
                    break;
                case CharacterSlots.CharacterSlot_03:
                    saveFileName = "CharacterSlot_03";
                    break;
                case CharacterSlots.CharacterSlot_04:
                    saveFileName = "CharacterSlot_04";
                    break;
                case CharacterSlots.CharacterSlot_05:
                    saveFileName = "CharacterSlot_05";
                    break;
                case CharacterSlots.CharacterSlot_06:
                    saveFileName = "CharacterSlot_06";
                    break;
                case CharacterSlots.CharacterSlot_07:
                    saveFileName = "CharacterSlot_07";
                    break;
                case CharacterSlots.CharacterSlot_08:
                    saveFileName = "CharacterSlot_08";
                    break;
                case CharacterSlots.CharacterSlot_09:
                    saveFileName = "CharacterSlot_09";
                    break;
                case CharacterSlots.CharacterSlot_10:
                    saveFileName = "CharacterSlot_10";
                    break;
            }
            return saveFileName;
        }

        public void NewGame()
        {
            saveFileName = DecideCharacterFileName(currentCharacterSlot);
            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileName(currentCharacterSlot);
            saveFileDataWriter = new SaveFileDataWriter();
            //works on multiple machine types
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

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

