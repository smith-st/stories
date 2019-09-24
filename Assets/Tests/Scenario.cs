using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using Utils.CSV;


namespace Tests
{
    public class Scenario
    {
        [Test]
        public void ScenarioAnswers()
        {
           
            var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Windows", "scenario"));
            if (bundle == null) {
                Assert.Fail();
            }
            
            var csvDataTable = new CSVDataTable();
            csvDataTable.ReadFromTextAsset(bundle.LoadAsset<TextAsset>("scenario"));

            var scenarionStorage = new ScenarioStorage();
            scenarionStorage.RegisterHeader(csvDataTable.Header);
            var forTest = new List<string>();
            
            for (var i = 0; i < csvDataTable.RowCount; i++) {
                string answer;
                for (var j = 1; j < GameParams.MaxAnswerButton; j++) {
                    answer = csvDataTable.GetRow(i)[scenarionStorage.Header["answer_" + j + "_out"]];
                    if (answer != string.Empty && !forTest.Contains(answer)) {
                        forTest.Add(answer);
                    }     
                }
                answer = csvDataTable.GetRow(i)[scenarionStorage.Header["next"]];
                if (answer != string.Empty && answer != "end" && !forTest.Contains(answer)) {
                    forTest.Add(answer);
                }     
            }

            foreach (var item in forTest) {
                var flag = false;
                for (var i = 0; i < csvDataTable.RowCount; i++) {
                    if (csvDataTable.GetRow(i)[scenarionStorage.Header["key"]] == item) {
                        flag = true;
                    }
                }

                if (!flag) {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }
    }
}
