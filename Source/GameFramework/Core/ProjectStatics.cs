using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Cryptography;

using Godot;
using GameFramework.Assertion;

namespace GameFramework.Core
{
    public static class ProjectStatics
    {
        public static string SaveGamesLocation = "Saves/";
        public static string EncryptionKey = "SuperKey";//8 characters

        public static bool SerializeObjectToXml<T>(T serializableObject, string fileName, bool encrypt = true)
        {
            if (typeof(T).IsSerializable || typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
            {
                if (!fileName.EndsWith(".xml"))
                {
                    fileName += ".xml";
                }

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    Stream streamObject = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write);

                    if (encrypt)
                    {
                        DES key = DES.Create();
                        using (CryptoStream cryptoStreamObject = new CryptoStream(streamObject, key.CreateEncryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(EncryptionKey)), CryptoStreamMode.Write))
                        {
                            serializer.Serialize(cryptoStreamObject, serializableObject);
                        }
                    }
                    else
                    {
                        serializer.Serialize(streamObject, serializableObject);
                    }

                    streamObject.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    GD.Print(ex.Message);
                    return false;
                }
            }
            GD.Print(typeof(T).Name + " is not serializable");
            return false;
        }

        public static T DeserializeObjectFromXml<T>(string fileName, bool encrypt = true)
        {
            if (typeof(T).IsSerializable || typeof(T).GetInterfaces().Contains(typeof(ISerializable)))
            {
                if (!fileName.EndsWith(".xml"))
                {
                    fileName += ".xml";
                }

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    Stream streamObject = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read);
                    T deserializedObject = default;

                    if (encrypt)
                    {
                        DES key = DES.Create();
                        using (CryptoStream cryptoStreamObject = new CryptoStream(streamObject, key.CreateDecryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(EncryptionKey)), CryptoStreamMode.Read))
                        {
                            deserializedObject = (T)serializer.Deserialize(cryptoStreamObject);
                        }
                    }
                    else
                    {
                        deserializedObject = (T)serializer.Deserialize(streamObject);
                    }

                    streamObject.Close();
                    return deserializedObject;
                }
                catch (Exception ex)
                {
                    GD.Print(ex.Message);
                    return default;
                }
            }
            GD.Print(typeof(T).Name + " is not serializable");
            return default;
        }

        public static void SaveGame<T>(T saveGameObject, string fileName, bool encrypt = true) where T : SaveGame
        {
            if (!Directory.Exists(SaveGamesLocation))
            {
                Directory.CreateDirectory(SaveGamesLocation);
            }

            string savePath = SaveGamesLocation + fileName;

            SerializeObjectToXml(saveGameObject, savePath, encrypt);
        }

        public static T LoadGame<T>(string fileName, bool encrypt = true) where T : SaveGame
        {
            string loadPath = SaveGamesLocation + fileName;

            return DeserializeObjectFromXml<T>(loadPath, encrypt);
        }

        public static T GetGameInstance<T>(SceneTree sceneTree) where T : GameInstance
        {
            GFSceneTree gfSceneTree = sceneTree as GFSceneTree;
            Assert.IsNotNull(gfSceneTree, "GFSceneTree is not valid. Ensure that \"application/run/main_loop_type\" option is set to \"GFSceneTree\".");

            return gfSceneTree.GameInstance as T;
        }

        public static T GetGameMode<T>(SceneTree sceneTree) where T : GameMode
        {
            GFSceneTree gfSceneTree = sceneTree as GFSceneTree;
            Assert.IsNotNull(gfSceneTree, "GFSceneTree is not valid. Ensure that \"application/run/main_loop_type\" option is set to \"GFSceneTree\".");

            return gfSceneTree.GameMode as T;
        }

        public static T GetLevel<T>(SceneTree sceneTree) where T : Level
        {
            GFSceneTree gfSceneTree = sceneTree as GFSceneTree;
            Assert.IsNotNull(gfSceneTree, "GFSceneTree is not valid. Ensure that \"application/run/main_loop_type\" option is set to \"GFSceneTree\".");

            return gfSceneTree.CurrentLevel as T;
        }

        public static void OpenLevel(SceneTree sceneTree, string resourcePath)
        {
            GFSceneTree gfSceneTree = sceneTree as GFSceneTree;
            Assert.IsNotNull(gfSceneTree, "GFSceneTree is not valid. Ensure that \"application/run/main_loop_type\" option is set to \"GFSceneTree\".");

            gfSceneTree.OpenLevel(resourcePath);
        }
    }
}