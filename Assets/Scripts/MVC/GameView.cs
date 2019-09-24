using System;
using UnityEngine;
using UnityEngine.UI;
using View.Managers;

namespace MVC {
    public class GameView : MonoBehaviour,IGameView {
        private IBackgroundManager _backgroundManager;
        private ICharacterManager _characterManager;
        private IDialogManager _dialogManager;
        private SceneData _sceneData;
        private IGameViewListener _listener;
        private float _delay = 2;
        [SerializeField] private Button _nextSceneButton;
    
    
        public Transform GetTransform {
            get { return this.transform; }
        }
    
        public void Init(IGameViewListener listener) {
            _backgroundManager = GetComponentInChildren<IBackgroundManager>();
            _characterManager = GetComponentInChildren<ICharacterManager>();
            _dialogManager = GetComponentInChildren<IDialogManager>();
        
            if (_backgroundManager == null)
                throw new Exception("Не найден менеджер IBackgroundManager");
        
            if (_characterManager == null)
                throw new Exception("Не найден менеджер ICharacterManager");
        
            if (_dialogManager == null)
                throw new Exception("Не найден менеджер IDialogManager");

            _listener = listener;
            _nextSceneButton.onClick.AddListener(ShowNextScene);
            _nextSceneButton.transform.SetSiblingIndex(1);
            DisplayButton(false);
        }

        private void DisplayButton(bool value) {
            _nextSceneButton.gameObject.SetActive(value);
        }
        /// <summary>
        /// for invoke
        /// </summary>
        private void DisplayButton() {
            DisplayButton(true);
        }

        private void ShowNextScene() {
            if (_listener != null)
                _listener.ShowNextScene();
        }
        
        private void ShowFirstScene() {
            if (_listener != null) {
                _nextSceneButton.onClick.RemoveAllListeners();
                _nextSceneButton.onClick.AddListener(ShowNextScene);
                _listener.ShowFirstScene();
            }
        }
    
        private void PlayerChoseAnswer(int answerId) {
            if (_listener != null)
                _listener.PlayerChoseAnswer(answerId);
        }

        public void ShowScene(SceneData data) {
            DisplayButton(false);
            if (_characterManager.CharacterOnScene && _characterManager.ActiveCharacterKey != data.CharacterKey) {
                _dialogManager.Hide();
                _characterManager.Hide( ()=> { CharacterOutScene(data); });
                return;
            }
            MoveCharacterOnScene(data);
        }

        public void FinalScene() {
            _dialogManager.Hide();
            _characterManager.Hide(null);
            _backgroundManager.HideAll();
            DisplayButton(false);
            _nextSceneButton.onClick.RemoveAllListeners();
            _nextSceneButton.onClick.AddListener(ShowFirstScene);
            Invoke(nameof(DisplayButton),_delay*2f);
        }

        public void Restart() {
            
        }

        private void MoveCharacterOnScene(SceneData data) {
            _backgroundManager.ChangeBackground(data.BackgroundKey);
            _characterManager.Show(data.CharacterKey, data.EmotionKey, ()=> { CharacterOnScene(data); });
            if (data.Answers == null || data.Answers.Length<0)
                Invoke(nameof(DisplayButton),_delay);
        }
    
    
    
        private void CharacterOnScene(SceneData data) {
            _dialogManager.Show(
                _characterManager.ActiveCharacterDialog,
                _characterManager.ActiveCharacterName,
                data.Message,
                data.Answers,
                PlayerChoseAnswer
            );
        }
    
        private void CharacterOutScene(SceneData data) {
            MoveCharacterOnScene(data);
        }
    }
}
