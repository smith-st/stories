using UnityEngine;

namespace Audio {
    public interface ISoundManager {
        void Play(string key, AudioClip clip);
        void Stop();
    }
}