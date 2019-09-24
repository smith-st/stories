using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace View.Managers {
    public class BackgroundManager : MonoBehaviour, IBackgroundManager {
        private float _speed = 1f;
        private string _currentKey = "";
        [Serializable]
        public struct BackgroundItem {
            public string Key;
            public Image Image;
            public Boolean Default;
        }
        [SerializeField] private BackgroundItem[] _backgrounds;

        private void Awake() {
            foreach (var background in _backgrounds) {
                background.Image.gameObject.SetActive(false);
            }
        }

        public void ChangeBackground(string key) {
            if (_currentKey == key) return;
            _currentKey = key;
            ShowImage(key);
        }

        public void HideAll() {
            _currentKey = "";
            foreach (var background in _backgrounds) {
                if (background.Key == _currentKey) {
                    background.Image.DOColor(Color.clear, _speed);
                }else {
                    background.Image.gameObject.SetActive(false);
                }
            }
        }

        private Image FindImageByKey(string key) {
            return (from background in _backgrounds where background.Key == key select background.Image).FirstOrDefault();
        }
        private Image FindDefaultImage() {
            return (from background in _backgrounds where background.Default select background.Image).FirstOrDefault();
        }
        
        private void ShowImage(string key) {
            var image = FindImageByKey(key);
            if (image == null) {
                image = FindDefaultImage();
            }

            if (image != null) {
                image.color = Color.clear;
                image.gameObject.SetActive(true);
                image.transform.SetSiblingIndex(image.transform.parent.childCount);
                image.DOColor(Color.white, _speed);
            }
        }
    }
}
