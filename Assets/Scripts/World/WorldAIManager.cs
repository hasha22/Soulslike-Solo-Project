using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance { get; private set; }

    [Header("Debug")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool spawnCharacters = false;

    [Header("Characters")]
    [SerializeField] GameObject[] AICharacters;
    [SerializeField] List<GameObject> spawnedCharacters;
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

    }
    private void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            StartCoroutine(WaitForSceneToLoad());
        }
    }
    private void Update()
    {
        if (despawnCharacters)
        {
            despawnCharacters = false;
            DespawnAllCharacters();
        }
        if (spawnCharacters)
        {
            spawnCharacters = false;
            SpawnAllCharacters();
        }
    }
    private IEnumerator WaitForSceneToLoad()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }
    }
    private void SpawnAllCharacters()
    {
        foreach (var character in AICharacters)
        {
            GameObject instantiatedCharacter = Instantiate(character);
            instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            spawnedCharacters.Add(instantiatedCharacter);
        }
    }
    private void DespawnAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }
        spawnedCharacters.Clear();
    }
    private void DisableAllCharacters()
    {

    }
}
