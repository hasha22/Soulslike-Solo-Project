using StartGame;
using UnityEngine;

public class TitleScreenInputManager : MonoBehaviour
{
    InputControls inputControls;
    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteCharacterSaveSlot = false;

    private void Update()
    {
        if (deleteCharacterSaveSlot)
        {
            deleteCharacterSaveSlot = false;
            TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
        }
    }
    private void OnEnable()
    {
        if (inputControls == null)
        {
            inputControls = new InputControls();
            inputControls.UI.Backspace.performed += i => deleteCharacterSaveSlot = true;
        }
        inputControls.Enable();
    }
    private void OnDisable()
    {
        inputControls.Disable();
    }
}
