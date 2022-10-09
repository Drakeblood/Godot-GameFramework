using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Cryptography;

using Godot;
using Godot.Collections;

using GB.Core;

namespace GB.Statics
{
    public static class ProjectStatics
    {
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

        public static void OpenLevel(Node ContextNode, string ResourcePath)
        {
            SceneTree SceneTreeObject = ContextNode.GetTree();

            Level CurrentLevel = GetLevel(ContextNode);
            if (CurrentLevel != null)
            {
                CurrentLevel.QueueFree();
            }

            PackedScene LevelPackedScene = ResourceLoader.Load<PackedScene>(ResourcePath);
            if (LevelPackedScene != null)
            {
                Level NewLevel = LevelPackedScene.Instantiate() as Level;
                if (NewLevel != null)
                {
                    NewLevel.InitLevel();
                    SceneTreeObject.Root.AddChild(NewLevel);
                }
            }
        }

        public static Level GetLevel(Node ContextNode)
        {
            if (ContextNode != null)
            {
                for(Node OuterNode = ContextNode.GetParent(); OuterNode != null; OuterNode = OuterNode.GetParent())
                {
                    if (OuterNode is Level)
                    {
                        return OuterNode as Level;
                    }
                }
            }
            return null;
        }

        public static T GetLevel<T>(Node ContextNode) where T : Level => GetLevel(ContextNode) as T;

        public static Level GetLevel(SceneTree SceneTreeObject)
        {
            Array<Node> RootChildren = SceneTreeObject.Root.GetChildren();

            foreach (Node NodeObject in RootChildren)
            {
                if (NodeObject is Level)
                {
                    return NodeObject as Level;
                }
            }
            return null;
        }

        public static T GetLevel<T>(SceneTree SceneTreeObject) where T : Level => GetLevel(SceneTreeObject) as T;
    }
}