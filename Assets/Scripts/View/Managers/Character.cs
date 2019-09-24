using System;
using DG.Tweening;
using UnityEngine;

namespace View.Managers {
    public class Character : MonoBehaviour
    {
        [Serializable]
        private class EmotionItem {
            public string Key;
            public Transform Face;
            public Transform Backround;
            public bool Default;
        }

        [SerializeField] private string _charaterKey;
        [SerializeField] private string _characterName;
        [SerializeField] private float _animationOffset;
        [SerializeField] private DialogType _dialogType;
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private EmotionItem[] _emotions;

        
        private RectTransform _rectTransform;
        private Vector2 _defaultPosition;
        private Action _finishCallback;
        
        public RectTransform Position {
            get {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        
        public Vector2 DefaultPosition {
            get { return _defaultPosition; }
        }
        
        public string CharacterName {
            get { return _characterName; }
        }
        
        public DialogType DialogType {
            get { return _dialogType; }
        }
        public string CharacterKey {
            get { return _charaterKey; }
        }
        
        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _defaultPosition = _rectTransform.anchoredPosition;
            Hide(false);
        }

       

        public void Show(Action finishCallback) {
            Show(true,finishCallback);
        }
        
        
        public void Hide(Action finishCallback) {
            Hide(true,finishCallback);
        }

        public void Show(bool anim = true, Action finishCallback = null) {
            if (anim) {
                _rectTransform.DOAnchorPosX(_defaultPosition.x, _moveSpeed).OnComplete(() => {
                    finishCallback?.Invoke();
                });
            }else {
                _rectTransform.anchoredPosition = new Vector2(
                    _defaultPosition.x,
                    _defaultPosition.y
                );
            } 
        }
        
        public void Hide(bool anim = true, Action finishCallback = null) {
            if (anim) {
                _rectTransform.DOAnchorPosX(_defaultPosition.x + _animationOffset, _moveSpeed).OnComplete(() => {
                    finishCallback?.Invoke();
                });
            }else {
                _rectTransform.anchoredPosition = new Vector2(
                    _defaultPosition.x + _animationOffset,
                    _defaultPosition.y
                );
                
            }
        }
        
        public void Emotion(string key) {
            var isPresent = false;
            foreach (var emotion in _emotions) {
                if (emotion.Key == key) {
                    emotion.Face.gameObject.SetActive(true);
                    emotion.Backround.gameObject.SetActive(true);
                    isPresent = true;
                }else {
                    emotion.Face.gameObject.SetActive(false);
                    emotion.Backround.gameObject.SetActive(false);
                }
            }

            if (!isPresent) {
                foreach (var emotion in _emotions) {
                    if (emotion.Default) {
                        emotion.Face.gameObject.SetActive(true);
                        emotion.Backround.gameObject.SetActive(true);
                    }else {
                        emotion.Face.gameObject.SetActive(false);
                        emotion.Backround.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
