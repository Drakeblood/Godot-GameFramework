using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Godot;

using GodotFramework.Core;
using System.Security.Cryptography;
using System.Text;

namespace GodotFramework.Statics
{
    public static class ProjectStatics
    {
        public static string StartupLevelResourcePath = "res://Scenes/MainMenuScene.tscn";
        public static string SaveGamesLocation = "Saves/";
        public static string EncryptionKey = "SuperKey";//8 characters

        public static bool SerializeObjectToXml<T>(T SerializableObject, string FileName, bool Encrypt = true)
        {
            if (typeof(T).IsSerializable || typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
            {
                if (!FileName.EndsWith(".xml"))
                {
                    FileName += ".xml";
                }

                try
                {
                    XmlSerializer Serializer = new XmlSerializer(typeof(T));
                    Stream StreamObject = new FileStream(FileName, FileMode.Create, System.IO.FileAccess.Write);

                    if (Encrypt)
                    {
                        DES Key = DES.Create();
                        using (CryptoStream CryptoStreamObject = new CryptoStream(StreamObject, Key.CreateEncryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(ProjectStatics.EncryptionKey)), CryptoStreamMode.Write))
                        {
                            Serializer.Serialize(CryptoStreamObject, SerializableObject);
                        }
                    }
                    else
                    {
                        Serializer.Serialize(StreamObject, SerializableObject);
                    }

                    StreamObject.Close();
                    return true;
                }
                catch (Exception Ex)
                {
                    GD.Print(Ex.Message);
                    return false;
                }
            }
            GD.Print(typeof(T).Name + " is not serializable");
            return false;
        }

        public static T DeserializeObjectFromXml<T>(string FileName, bool Encrypt = true)
        {
            if (typeof(T).IsSerializable || typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
            {
                if (!FileName.EndsWith(".xml"))
                {
                    FileName += ".xml";
                }

                try
                {
                    XmlSerializer Serializer = new XmlSerializer(typeof(T));
                    Stream StreamObject = new FileStream(FileName, FileMode.Open, System.IO.FileAccess.Read);
                    T DeserializedObject = default;

                    if (Encrypt)
                    {
                        DES Key = DES.Create();
                        using (CryptoStream CryptoStreamObject = new CryptoStream(StreamObject, Key.CreateDecryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(ProjectStatics.EncryptionKey)), CryptoStreamMode.Read))
                        {
                            DeserializedObject = (T)Serializer.Deserialize(CryptoStreamObject);
                        }
                    }
                    else
                    {
                        DeserializedObject = (T)Serializer.Deserialize(StreamObject);
                    }

                    StreamObject.Close();
                    return DeserializedObject;
                }
                catch(Exception Ex)
                {
                    GD.Print(Ex.Message);
                    return default;
                }
            }
            GD.Print(typeof(T).Name + " is not serializable");
            return default;
        }

        public static void SaveGame<T>(T SaveGameObject, string FileName, bool Encrypt = true) where T : SaveGame
        {
            if (!Directory.Exists(SaveGamesLocation))
            {
                Directory.CreateDirectory(SaveGamesLocation);
            }

            string SavePath = SaveGamesLocation + FileName;

            SerializeObjectToXml<T>(SaveGameObject, SavePath, Encrypt);
        }

        public static T LoadGame<T>(string FileName, bool Encrypt = true) where T : SaveGame
        {
            string LoadPath = SaveGamesLocation + FileName;

            return DeserializeObjectFromXml<T>(LoadPath, Encrypt);
        }
    }
}