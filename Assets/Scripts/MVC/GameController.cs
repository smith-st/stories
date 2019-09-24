using System;
using System.Linq;
using Audio;
using UnityEngine;

namespace MVC {
    public class GameController : MonoBehaviour,IGameViewListener {

//        [SerializeField] private GameObject[] _prefabs;
        
        private IGameModel _model;
        private IGameView _view;
        private SceneData _currentScene;

        void Start() {
            _model = new GameModel();
            var v = FindObjectsOfType<MonoBehaviour>().OfType<IGameView>();
            var gameViews = v.ToList();
            if (gameViews.Any()) {
                _view = gameViews[0];
            }

            if (_view == null)
                throw new Exception("Не найден объект IGameView");

            Instantiate(_model.GetBackgroundManager(), Vector3.zero, Quaternion.identity, _view.GetTransform);
            Instantiate(_model.GetCharacterManager(), Vector3.zero, Quaternion.identity, _view.GetTransform);
            Instantiate(_model.GetDialogManager(), Vector3.zero, Quaternion.identity, _view.GetTransform);

//            foreach (var prefab in _prefabs) {
//                Instantiate(prefab, Vector3.zero, Quaternion.identity, _view.GetTransform);
//            }

            _view.Init(this);
            ShowFirstScene();
        }

        public void ShowFirstScene() {
            _currentScene = _model.FirstScene();
            ShowScene(_currentScene);
        }

        public void ShowNextScene() {
            _currentScene = _model.NextScene(_currentScene);
            if (_currentScene.Key == "end") {
                _view.FinalScene();
                SoundManager.instance.Stop();
            }else {
                ShowScene(_currentScene);    
            }
        }

        public void PlayerChoseAnswer(int answerId) {
            var key = _currentScene.SceneKeys[answerId];
            _currentScene = _model.SceneByKey(key);
            ShowScene(_currentScene);
        }

        private void ShowScene(SceneData data) {
            _view.ShowScene(data);
            SoundManager.instance.Play(data.MusicKey,_model.AudioClipByKey(data.MusicKey));
        }
    }
}
