using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils.CSV;

namespace MVC {
    public class GameModel:IGameModel {
        private readonly Dictionary<string, AssetBundle> _bundles;
        private ScenarioStorage _scenarionStorage;
        private MusicAsset _musics;
        private string[] _bundlesToLoad = new[] {
            "background",
            "character",
            "dialog",
            "scenario",
            "music"
        };
        
        public GameModel() {
            _bundles = new Dictionary<string, AssetBundle>();
            LoadBundles();
        }
        
        public GameObject GetBackgroundManager() {
            return _bundles["background"].LoadAsset<GameObject>("BackgroundManager");
        }

        public GameObject GetCharacterManager() {
            return _bundles["character"].LoadAsset<GameObject>("CharacterManager");
        }

        public GameObject GetDialogManager() {
            return _bundles["dialog"].LoadAsset<GameObject>("DialogManager");
        }

        public SceneData FirstScene() {
            return SceneByKey(_scenarionStorage.FirstKey);
        }

        public SceneData NextScene(SceneData currentScene) {
            if (_scenarionStorage.GetRow(currentScene.Key)["next"] == "end") {
                return EndScene();  
            }
            if (_scenarionStorage.GetRow(currentScene.Key)["next"] != string.Empty) {
                return SceneByKey(_scenarionStorage.GetRow(currentScene.Key)["next"]);
            }
            var key = _scenarionStorage.NextKeyAfter(currentScene.Key);
            return key == "" ? EndScene() : SceneByKey(key);
        }

        public SceneData SceneByKey(string key) {
            var row = _scenarionStorage.GetRow(key);
            var scene = new SceneData();
            if (row!=null) {
                scene.Key = row["key"];
                scene.BackgroundKey = row["background"];
                scene.MusicKey = row["music"];
                scene.CharacterKey = row["character"];
                scene.EmotionKey = row["emotion"];
                scene.Message = row["message"];
                
                if (row["answer_1"] != string.Empty) {
                    var answers = new List<string>();
                    var keys = new List<string>();
                    for (var i = 1; i <= GameParams.MaxAnswerButton; i++) {
                        if (row["answer_" + i.ToString()] != string.Empty) {
                            answers.Add(row["answer_" + i.ToString()]);
                            keys.Add(row["answer_" + i.ToString() + "_out"]);
                        }else {
                            break;   
                        }
                    }
                    scene.Answers = answers.ToArray();
                    scene.SceneKeys = keys.ToArray();
                }
            }

            return scene;
        }

        public AudioClip AudioClipByKey(string key) {
            return _musics.GetClipByKey(key);
        }

        private SceneData EndScene() {
            var scene = new SceneData();
            scene.Key = "end";
            return scene;
        }
        
        private TextAsset GetScenario() {
            return _bundles["scenario"].LoadAsset<TextAsset>("scenario");
        }
         
        
        private void LoadBundles() {
            string platform = "Windows";
#if PLATFORM_ANDROID
            platform = "Android";
#endif
            foreach (var bundleName in _bundlesToLoad) {
                var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, platform, bundleName));
                if (bundle == null) {
                    Debug.Log("Failed to load AssetBundle!");
                    continue;
                }
                _bundles.Add(bundleName,bundle);
            }
            ParseScenario();
            ParseMusic();
        }

        private void ParseScenario() {
            var csvDataTable = new CSVDataTable();
            csvDataTable.ReadFromTextAsset(GetScenario());

            _scenarionStorage = new ScenarioStorage();
            _scenarionStorage.RegisterHeader(csvDataTable.Header);
            for (var i = 0; i < csvDataTable.RowCount; i++) {
                _scenarionStorage.RegisterRow(csvDataTable.GetRow(i));
            }
        }
        
        private void ParseMusic() {
            _musics = _bundles["music"].LoadAsset<MusicAsset>("music");
        }
    }
}
