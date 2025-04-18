using SaveGameManager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace StartGame
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance { get; private set; }

        [Header("Buttons")]
        [SerializeField] Button noCharacterSlotsOKButton;
        [SerializeField] Button deleteCharacterSaveSlotConfirmButton;

        [Header("Menus")]
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        [Header("Character Slots")]
        public CharacterSlots currentCharacterSlot = CharacterSlots.NO_SLOT;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        public void DisplayPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOKButton.Select();
        }
        public void SelectCharacterSlot(CharacterSlots characterSlot)
        {
            currentCharacterSlot = characterSlot;
        }
        public void SelectNoSlot()
        {
            currentCharacterSlot = CharacterSlots.NO_SLOT;
        }
        public void AttemptToDeleteCharacterSlot()
        {
            if (currentCharacterSlot != CharacterSlots.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterSaveSlotConfirmButton.Select();
            }
        }
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentCharacterSlot);

            //refreshes load menu
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
        }
    }
}

