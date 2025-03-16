using System;
using System.IO;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool CheckToSeeIfFileExists()
    {
        return File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName));
    }
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating save file at save path:" + savePath);

            string dataToStore = JsonUtility.ToJson(characterData, true);
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error while trying to save to" + savePath + "\n" + ex);
        }
    }
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.Log("Error while trying to load file:" + loadPath + "\n" + ex);
            }
        }

        return characterData;
    }
}
