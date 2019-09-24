using UnityEngine;

namespace MVC {
    public interface IGameView {
        Transform GetTransform { get; }
        void Init(IGameViewListener listener);
        void ShowScene(SceneData data);
        void FinalScene();
    }
}