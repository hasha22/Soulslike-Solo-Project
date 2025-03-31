using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayer;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Resources")]
    public float currentHealth;
    public float currentStamina;
    public float currentFocusPoints;

    [Header("Stats")]
    public int endurance;
    public int vigor;
    public int mind;
}
