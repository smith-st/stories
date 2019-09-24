using System;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct KeyTransformItem {
    public string Key;
    public Transform Transform;
}

public struct SceneData {
    public string Key;
    public string CharacterKey;
    public string EmotionKey;
    public string Message;
    public string BackgroundKey;
    public string MusicKey;
    public string[] Answers;
    public string[] SceneKeys;

    public SceneData(
        string key,
        string characterKey,
        string message,
        string emotionKey = "",
        string backgroundKey = "",
        string musicKey = "", 
        string[] answers = null,
        string[] sceneKeys = null
        ) {
        Key = key;
        CharacterKey = characterKey;
        EmotionKey = emotionKey;
        Message = message;
        BackgroundKey = backgroundKey;
        MusicKey = musicKey;
        Answers = answers;
        SceneKeys = sceneKeys;
        
    }
    
}
