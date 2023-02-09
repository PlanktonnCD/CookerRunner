using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling
{
    public class DataController
    {
        private static readonly string DATAPATH = (Application.persistentDataPath + "/PlayerData.json");
        public static readonly string MESHDATAPATH = Application.persistentDataPath + "/mesh.txt";

        public static void SaveIntoJson(UserProfileData userProfileData)
        {
            var output = JsonConvert.SerializeObject(userProfileData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
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

        // private static MeshData LoadFromFile(string filepath = null)
        // {
        //     var formatter = new BinaryFormatter();
        //
        //     if (filepath == null)
        //     {
        //         filepath = MESHDATAPATH;
        //     }
        //     
        //     MeshData data = null;
        //     using (FileStream filestream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //     {
        //         data = (MeshData)formatter.Deserialize(filestream);
        //
        //         filestream.Close();
        //     }
        //     return data;
        // }
        //
        // public void SaveToFile(string filepath)
        // {
        //     var formatter = new BinaryFormatter();
        //
        //     using (FileStream filestream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        //     {
        //         formatter.Serialize(filestream, this);
        //         filestream.Close();
        //     }
        // }

        public static void ResetProgress()
        {
            File.Delete(DATAPATH);
        }
    }
}