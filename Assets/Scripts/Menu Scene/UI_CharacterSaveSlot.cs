using SaveGameManager;
using StartGame;
using TMPro;
using UnityEngine;

public class UI_CharacterSaveSlot : MonoBehaviour
{
    SaveFileDataWriter saveFileDataWriter;
    [Header("Game Slot")]
    public CharacterSlots characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }
    public void LoadGameFromSaveSlot()
    {
        WorldSaveGameManager.instance.currentCharacterSlot = characterSlot;
        WorldSaveGameManager.instance.LoadGame();
    }
    public void LoadSaveSlots()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        if (characterSlot == CharacterSlots.CharacterSlot_01)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot01 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_02)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot02 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_03)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot03 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_04)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot04 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_05)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot05 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_06)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot06 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_07)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot07 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_08)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot08 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_09)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot09 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (characterSlot == CharacterSlots.CharacterSlot_10)
        {
            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                WorldSaveGameManager.instance.characterSlot10 = saveFileDataWriter.LoadSaveFile();
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }
    public void SelectCurrentSlot()
    {
        TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
    }
}
