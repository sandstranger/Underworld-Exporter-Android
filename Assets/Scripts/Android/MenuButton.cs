using TMPro;
using UnityEngine;

namespace UnderworldExporter.Game
{
    public abstract class MenuButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _slotNumberText;
        [SerializeField] protected OptionsMenuControl _optionsMenuControl;
        protected int _saveSlot;

        public void Initialize(int saveSlot, OptionsMenuControl optionsMenuControl)
        {
            _saveSlot = saveSlot;
            _slotNumberText.text = saveSlot.ToString();
            _optionsMenuControl = optionsMenuControl;
        }

        public abstract void OnSaveButtonClicked();
    }
}