using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace View.Managers {
    public class Dialog : MonoBehaviour, IAnswerButtonListener {
        [SerializeField] private RectTransform _container;
        [SerializeField] private Text _characterName;
        [SerializeField] private Text _message;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _addToHeight;
        [SerializeField] private AnswerButton[] _answerButtons;
        private RectTransform _rectTransform;
        private Action<int> _choseAnswer;
        private bool _needResize = false;
        private float _resizeStep = 1f;
        private float _speed = 0.5f;
        private int _showAtOnceDisplay = 0;


        public RectTransform Position {
            get {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
            set {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                _rectTransform = value;
            }
        }

        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            Init();
            HideAllButtons();
            Display(false);
        }
        
        public void SetName(string text) {
            if (_characterName != null)
                _characterName.text = text;
            else {
                Debug.LogWarningFormat("Нет ссылки на поле с именем в класе {0}", this.ToString());
            }
        }
        
        public void SetMessage(string text) {
            if (_message != null) {
                _message.text = text;
                _message.color = Color.clear;
                if (_showAtOnceDisplay == 1) {
                    //первое отображение текста за показ
                    _container.sizeDelta = new Vector2(
                        _container.sizeDelta.x,
                        _minHeight
                    );
                }
                _needResize = true;
                
            }else {
                Debug.LogWarningFormat("Нет ссылки на поле сообщения в класе {0}", this.ToString());
            }    
        }

        public void SetAnswers(string[] answers) {
            DisplayButton(answers);
        }

        public void Show() {
            Display();
        }
        
        public void Show(string characterName, string message, string[] answers = null, Action<int> choseAnswer = null) {
            HideAllButtons();
            _showAtOnceDisplay++;
            Display();
            SetName(characterName);
            SetMessage(message);
            if (answers != null) {
                _choseAnswer = choseAnswer;
                SetAnswers(answers);
            }
        }
        
        public void Hide() {
            _showAtOnceDisplay = 0;
            Display(false);
        }

        public void PlayerChooseAnswer(int answerId) {
            Debug.LogFormat("Игрок выбрал вариант № {0}",answerId);
            if (_choseAnswer != null) {
                _choseAnswer.Invoke(answerId);
                _choseAnswer = null;
            }
        }

        private void OnGUI() {
            if (_needResize) {
                var to = _message.rectTransform.rect.height + _addToHeight;

                if (_container.sizeDelta.y - _resizeStep < to && _container.sizeDelta.y + _resizeStep > to) {
                    _message.DOColor(Color.black, _speed);
                    _needResize = false;
                }else{
                    if (_container.sizeDelta.y < to) {
                        _container.sizeDelta = new Vector2(
                            _container.sizeDelta.x,
                            _container.sizeDelta.y + _resizeStep
                        );
                    }else if (_container.sizeDelta.y > to) {
                        _container.sizeDelta = new Vector2(
                            _container.sizeDelta.x,
                            _container.sizeDelta.y - _resizeStep
                        );
                    }
                }
            }
        }

        private void Init() {
            for (var i = 0; i < _answerButtons.Length; i++) {
                _answerButtons[i].Init(i,this);
            }
        }
        
        private void Display(bool value = true) {
            _container.gameObject.SetActive(value);
        }
        
        private void HideAllButtons() {
            DisplayButton(false,_answerButtons.Length);
        }
        private void DisplayButton(string[] answers) {
            for (var i = 0; i < _answerButtons.Length; i++) {
                if (i <= answers.Length - 1) {
                    _answerButtons[i].gameObject.SetActive(true);
                    _answerButtons[i].SetText(answers[i]);
                }else {
                    break;
                }
            }
        }
        private void DisplayButton(bool status = true, int to = GameParams.MaxAnswerButton) {
            if (to > _answerButtons.Length)
                to = _answerButtons.Length;
            for (var i = 0; i < to; i++) {
               _answerButtons[i].gameObject.SetActive(status);
            }
        }

        
    }
}
