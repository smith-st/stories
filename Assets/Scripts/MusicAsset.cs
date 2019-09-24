using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "music", menuName = "Music Asset")]
public class MusicAsset:ScriptableObject {
    [Serializable]
    private struct MusicItem {
        public string Key;
        public AudioClip Clip;
    }

    [SerializeField] private MusicItem[] _musics;

    public AudioClip GetClipByKey(string key) {
        return (from item in _musics where item.Key == key select item.Clip).FirstOrDefault();
    }
}