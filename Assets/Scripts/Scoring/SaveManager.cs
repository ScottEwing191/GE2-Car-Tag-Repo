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


        public static void Save(PlayerScore data) {
            string dir = Application.persistentDataPath + directory;
            //string dir = Application.dataPath + directory;

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
        }

        internal static void SaveTelemetryData(List<PlayerScore> playerScores) {
            SaveManager.SaveToNewFile(playerScores[0]);
            for (int i = 1; i < playerScores.Count; i++) {
                SaveManager.Save(playerScores[i]);
            }
        }

        private static void SaveToNewFile(PlayerScore data) {
            string dir = Application.persistentDataPath + directory;
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
    }
}
