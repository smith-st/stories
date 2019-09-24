using System;
using UnityEngine;

namespace View.Managers {
    public interface ICharacterManager {
        bool CharacterOnScene { get; }
        string ActiveCharacterName { get; }
        string ActiveCharacterKey { get; }
        DialogType ActiveCharacterDialog { get; }
        Vector2 ActiveCharacterPosition { get; }

        void Show(string key, string emotion, Action finishCallback);
        void Hide(Action finishCallback);
    }
}