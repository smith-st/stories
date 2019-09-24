using System;
using UnityEngine;

namespace View.Managers {
    public class DialogManager:MonoBehaviour,IDialogManager {
        [Serializable]
        public struct DialogItem {
            public DialogType Type;
            public Dialog Dialog;
        }
        
        [SerializeField] private DialogItem[] _dialogs;
        public void Show(DialogType dialogType, string characterName, string message, string[] answers, Action<int> choseAnswer) {
            foreach (var dialog in _dialogs) {
                if (dialog.Type == dialogType) {
                    dialog.Dialog.Show(characterName,message,answers,choseAnswer);
                }
            }
            
        }

        public void Hide() {
            foreach (var dialog in _dialogs) {
                dialog.Dialog.Hide();
            }
        }
    }
}