using UnityEngine;
using UnityEngine.UI;

namespace View {
    [RequireComponent(typeof(Button))]
    public class AnswerButton : MonoBehaviour {
        [SerializeField] private Text _text;
        private int _answerId;
        private IAnswerButtonListener _listener;

        private void Awake(){
            if (_text == null) {
                _text = GetComponentInChildren<Text>();
            }
        }

        public void Init(int answerId, IAnswerButtonListener listener) {
            _answerId = answerId;
            _listener = listener;
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        
        private void OnClick() {
            _listener?.PlayerChooseAnswer(_answerId);
        }

        public void SetText(string text) {
            _text.text = text;
        }


    }
}
