using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayer;

    // Can't use vector3, data can only be saved from 'basic' variable types
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
