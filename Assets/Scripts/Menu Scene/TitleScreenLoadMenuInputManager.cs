using UnityEngine;

namespace Menu_Scene
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        private PlayerController _playerController;

        [Header("Title Screen Inputs")] [SerializeField]
        private bool deleteCharacterSlot = false;

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if(_playerController == null)
            {
                _playerController = new PlayerController();
                _playerController.UI.X.performed += ctx => deleteCharacterSlot = true;
            }

            _playerController.Enable();
        }
    
        private void OnDisable()
        {
            _playerController.Disable();
        }
    }
}