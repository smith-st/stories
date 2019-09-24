using System;
using System.Linq;
using UnityEngine;

namespace View.Managers {
    public class CharacterManager : MonoBehaviour,ICharacterManager
    {
        
        private string _currentKey = "";
        private Character _currentCharacter ;
        private bool _characterOnScene = false;
       
        [SerializeField] private Character[] _characters;

        public bool CharacterOnScene {
            get { return _characterOnScene; }
        }
        public string ActiveCharacterName {
            get { return _currentCharacter.CharacterName; }
        }

        public string ActiveCharacterKey {
            get { return _currentKey; }
        }

        public DialogType ActiveCharacterDialog {
            get { return _currentCharacter.DialogType; }
        }

        public Vector2 ActiveCharacterPosition {
            get { return _currentCharacter.DefaultPosition; }
        }

        public void Show(string key, string emotion, Action finishCallback) {
            var character = (from item in _characters where item.CharacterKey == key select item).FirstOrDefault();
            if (character != null) {
                if (_currentKey != key) {
                    character.Show(finishCallback);
                }else {
                    finishCallback.Invoke();
                }
                character.Emotion(emotion);
                _currentKey = key;
                _currentCharacter = character;
                _characterOnScene = true;
            }
        }

        public void Hide(Action finishCallback) {
            if (_currentCharacter != null) {
                _currentCharacter.Hide(finishCallback);
            }else {
                finishCallback.Invoke();   
            }
        }
    }
}
