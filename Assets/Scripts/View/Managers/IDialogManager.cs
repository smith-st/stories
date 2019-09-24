using System;

namespace View.Managers {
    public interface IDialogManager {
        void Show(DialogType type, string characterName, string message, string[] answers, Action<int> choseAnswer);
        void Hide();
    }
}