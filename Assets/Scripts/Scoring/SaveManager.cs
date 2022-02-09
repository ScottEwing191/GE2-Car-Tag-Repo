using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CarTag.ScoreSystem {
    public static class SaveManager {
        public static string directory = "/SaveData/";
        public static string filename = "Game1.txt";

        public static void Save(PlayerScore data) {
            string dir = Application.persistentDataPath + directory;
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string json = JsonUtility.ToJson(data);
            if (File.Exists(dir + filename)) {
                Debug.LogError("Already Exists");
                File.AppendAllText(dir + filename, json);
            }
            else {

                File.WriteAllText(dir + filename, json);
            }
        }



    }
}
