using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace CarTag.ScoreSystem {
    public static class SaveManager {
        public static string directory = "/SaveData/";
        public static string fileName = "Game ";
        public static int fileNumber = 1;
        public static string fileExt = ".json";
        public static string fileNameFull = fileName + fileNumber + fileExt;

        //--ORIGINAL
        /*internal static void SaveTelemetryData(List<PlayerScore> playerScores) {
            SaveManager.SaveToNewFile(playerScores[0]);
            for (int i = 1; i < playerScores.Count; i++) {
                SaveManager.Save(playerScores[i]);
            }
        }*/

        internal static void SaveTelemetryData(List<PlayerScore> playerScores) {
            //--Save Each Player To Separate File
            for (int i = 0; i < playerScores.Count; i++) {
                playerScores[i].CalculateTotalAbilityUses();
                SetFileName(playerScores[i]);
                SaveSinglePlayer(playerScores[i]);
            }
        }

        private static void SaveSinglePlayer(PlayerScore playerScore) {
            //string dir = Application.persistentDataPath + directory;
            string dir = Application.dataPath + directory;

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(playerScore);

            json = FormatJSONString(json);

            while (File.Exists(dir + fileNameFull)) {
                Debug.LogError("Already Exists");
                //File.AppendAllText(dir + fileNameFull, json);
                fileNumber++;
                fileNameFull = fileName + fileNumber + fileExt;
            }
            File.WriteAllText(dir + fileNameFull, json);

        }

        private static string FormatJSONString(string json) {
            json = "{\"\": [" + json;
            int index = json.IndexOf(",\"Round_Data");
            json = json.Insert(index, "}]");
            return json;
        }

        private static void SetFileName(PlayerScore playerScore) {
            fileName = $"{playerScore.Player_Name} {playerScore.Test_Type} ";
            fileNameFull = fileName + fileNumber + fileExt;
            //return  $"{playerScore.PlayerName} {playerScore.TestType}";
        }

        static string BuildJSONString(List<PlayerScore> playerScores) {
            string jsonString = "{\" \": [";
            for (int i = 1; i < playerScores.Count; i++) {
                jsonString += JsonUtility.ToJson(playerScores[i]);
                if (i != playerScores.Count - 1) {      // if this is no the last player score in the list
                    jsonString += ",";
                }
            }
            jsonString += ",";

            return jsonString;

        }


        public static void Save(PlayerScore data) {
            //string dir = Application.persistentDataPath + directory;
            string dir = Application.dataPath + directory;
            //Debug.Log("Data Path" + Application.dataPath);
            //Debug.Log("Persistant Data Path" + Application.persistentDataPath);


            //string dir = Application.dataPath + directory;
            Debug.Log("dir" + dir);

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(data);
            if (File.Exists(dir + fileNameFull)) {
                Debug.LogError("Already Exists");
                File.AppendAllText(dir + fileNameFull, json);
            }
            else {

                File.WriteAllText(dir + fileNameFull, json);
            }
            Debug.Log("dir 2" + dir);

        }

        /*internal static void SaveTelemetryData(ScoreManager scoreManager) {
            SaveManager.SaveToNewFile(scoreManager);

        }*/

        private static void SaveToNewFile(PlayerScore data) {
            //string dir = Application.persistentDataPath + directory;
            string dir = Application.dataPath + directory;

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(data);
            while (File.Exists(dir + fileNameFull)) {
                Debug.LogError("Already Exists");
                File.AppendAllText(dir + fileNameFull, json);
                fileNumber++;
                fileNameFull = fileName + fileNumber + fileExt;
            }

            File.WriteAllText(dir + fileNameFull, json);
        }


        /*//-- DOESNT WORK
        private static void SaveToNewFile(ScoreManager data) {
            //string dir = Application.persistentDataPath + directory;
            string dir = Application.dataPath + directory;
            data.playerScoresArray = data.PlayerScores.ToArray();
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(data);
            while (File.Exists(dir + fileNameFull)) {
                Debug.LogError("Already Exists");
                //File.AppendAllText(dir + fileNameFull, json);
                fileNumber++;
                fileNameFull = fileName + fileNumber + fileExt;
            }

            File.WriteAllText(dir + fileNameFull, json);
        }*/
    }
}
