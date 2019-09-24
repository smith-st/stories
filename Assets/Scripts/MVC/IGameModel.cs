using UnityEngine;

namespace MVC {
    public interface IGameModel {
        GameObject GetBackgroundManager();
        GameObject GetCharacterManager();
        GameObject GetDialogManager();
        SceneData FirstScene();
        SceneData NextScene(SceneData currentScene);
        SceneData SceneByKey(string key);
        AudioClip AudioClipByKey(string key);
    }
}