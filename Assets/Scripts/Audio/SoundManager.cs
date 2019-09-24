using UnityEngine;

namespace Audio {
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager:MonoBehaviour,ISoundManager {
        public static ISoundManager instance = null;
        private AudioSource _audioSource;
        private string _currentKey = ""; 
        
        private void Awake() {
            _audioSource = GetComponent<AudioSource>();
        }
    
        private void Start () {
            if (instance == null) {
                instance = this;
            } else if(instance == this){
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }


        public void Play(string key, AudioClip clip) {
            if (_currentKey == key)
                return;
            if (clip == null)
                return;
            _audioSource.clip = clip;
            _audioSource.Play();
            _currentKey = key;
        }

        

        public void Stop() {
            _audioSource.Stop();
        }
    }
}