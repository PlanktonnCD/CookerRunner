using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling
{
    public class DataController
    {
        public static readonly string DATAPATH = (Application.persistentDataPath + "/PlayerData.json");
        public static readonly string LOCALIZATIONPATH = (Application.dataPath + "/Resources/Localization.json");

        public static void SaveIntoJson(UserProfileData userProfileData)
        {
            var output = JsonConvert.SerializeObject(userProfileData);
            File.WriteAllText(DATAPATH, output);
        }

        public static UserProfileData ReadFromJson()
        {
            if (!File.Exists(DATAPATH))
            {
                File.WriteAllText(DATAPATH, String.Empty);
            }
            var input = File.ReadAllText(DATAPATH);
            return JsonConvert.DeserializeObject<UserProfileData>(input);
        }

        public static void ResetProgress()
        {
            File.Delete(DATAPATH);
        }
    }
}