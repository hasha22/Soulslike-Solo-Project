using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [Header("Layer Masks")]
    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask enviroLayers;
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
    public LayerMask GetCharacterLayers()
    {
        return characterLayers;
    }
    public LayerMask GetEnviroLayers()
    {
        return enviroLayers;
    }
}
