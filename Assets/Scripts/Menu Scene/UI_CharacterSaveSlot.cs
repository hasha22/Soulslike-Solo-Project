using SaveGameManager;
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
    public void LoadSaveSlots()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        string fileName = "";
        Debug.Log("Character name: " + $"{WorldSaveGameManager.instance.characterSlot01.characterName}");
        if (characterSlot == CharacterSlots.CharacterSlot_01)
        {
            fileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
            saveFileDataWriter.saveFileName = fileName;
            if (saveFileDataWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }
}
