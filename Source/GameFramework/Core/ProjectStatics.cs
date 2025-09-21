using System;
using System.Text.Json;

using Godot;

using GameFramework.Assertion;

namespace GameFramework.Core
{
    public static class ProjectStatics
    {
        public const string UserLocation = "user://";
        public const string SavesFolder = "saves/";
        public const string SavesLocation = UserLocation + SavesFolder + "/";
        public const string SaveGameEncryptionKey = "super_secret_password";

        public static void SaveGame<T>(string slotName, T data, bool encrypt = true) where T : SaveGame
        {
            try
            {
                DirAccess dir = DirAccess.Open(UserLocation);
                if (!dir.DirExists(SavesFolder))
                {
                    dir.MakeDir(SavesFolder);
                    GD.Print("Folder 'saves' created.");
                }
                
                string json = JsonSerializer.Serialize(data);

                if (encrypt)
                {
                    using (var file = FileAccess.OpenEncryptedWithPass(SavesLocation + slotName + ".sav", FileAccess.ModeFlags.Write, SaveGameEncryptionKey))
                    {
                        file.StoreString(json);
                    }
                }
                else
                {
                    using (var file = FileAccess.Open(SavesLocation + slotName + ".sav", FileAccess.ModeFlags.Write))
                    {
                        file.StoreString(json);
                    }
                }
                
                GD.Print("Game saved successfully.");
            }
            catch (Exception e)
            {
                GD.PrintErr("Failed to save game: " + e.Message);
            }
        }

        public static T LoadGame<T>(string slotName, bool encrypt = true) where T : SaveGame
        {
            try
            {
                if (!FileAccess.FileExists(SavesLocation + slotName + ".sav"))
                {
                    GD.Print("Save file not found.");
                    return default;
                }

                T data;
                if (encrypt)
                {
                    using (var file = FileAccess.OpenEncryptedWithPass(SavesLocation + slotName + ".sav", FileAccess.ModeFlags.Read, SaveGameEncryptionKey))
                    {
                        string json = file.GetAsText();
                        data = JsonSerializer.Deserialize<T>(json);
                    }
                }
                else
                {
                    using (var file = FileAccess.Open(SavesLocation + slotName + ".sav", FileAccess.ModeFlags.Read))
                    {
                        string json = file.GetAsText();
                        data = JsonSerializer.Deserialize<T>(json);
                    }
                }
                    
                GD.Print("Game loaded successfully.");
                return data;
            }
            catch (Exception e)
            {
                GD.PrintErr("Failed to load game: " + e.Message);
                return null;
            }
        }

        private static GFSceneTree GetGFSceneTree(SceneTree sceneTree)
        {
            GFSceneTree gfSceneTree = sceneTree as GFSceneTree;
            Assert.IsNotNull(gfSceneTree, "GFSceneTree is not valid. Ensure that \"application/run/main_loop_type\" option is set to \"GFSceneTree\".");
            return gfSceneTree;
        }

        public static T GetGameInstance<T>(SceneTree sceneTree) where T : GameInstance => GetGFSceneTree(sceneTree).GameInstance as T;
        public static T GetGameMode<T>(SceneTree sceneTree) where T : GameMode => GetGFSceneTree(sceneTree).GameMode as T;
        public static T GetLevel<T>(SceneTree sceneTree) where T : Level => GetGFSceneTree(sceneTree).CurrentLevel as T;
        public static void OpenLevel(SceneTree sceneTree, string resourcePath) => GetGFSceneTree(sceneTree).OpenLevel(resourcePath);
    }
}