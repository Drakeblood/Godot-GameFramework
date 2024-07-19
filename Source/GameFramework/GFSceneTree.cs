using Godot;
using Godot.Collections;

namespace GameFramework.System
{
    [GlobalClass]
    public partial class GFSceneTree : SceneTree
    {
        private GameInstance gameInstance;
        public GameInstance GameInstance
        {
            get => gameInstance;
            private set { gameInstance = value; }
        }

        private GameMode gameMode;
        public GameMode GameMode
        {
            get => gameMode;
            private set { gameMode = value; } 
        }

        private Level currentLevel;
        public Level CurrentLevel
        {
            get => currentLevel;
            private set { currentLevel = value; }
        }

        public override void _Initialize()
        {
            {
                string gameInstanceScriptPath = ProjectSettings.GetSetting("application/game_framework/game_instance_script").AsString();
                Script gameInstanceScript = GD.Load<Script>(gameInstanceScriptPath);

                if (gameInstanceScript == null)
                {
                    GD.PrintErr($"Load script from {gameInstanceScriptPath} path failed. Please update \"application/game_framework/game_instance_script\" option in project settings.");
                    return;
                }

                GodotObject gameInstanceGO = new GodotObject();
                ulong gameInstanceId = gameInstanceGO.GetInstanceId();
                gameInstanceGO.SetScript(gameInstanceScript);

                GameInstance = InstanceFromId(gameInstanceId) as GameInstance;
                GameInstance.Init(this);
            }

            CurrentLevel = FindLevel();
            CreateGameMode();
            if (CurrentLevel != null) CurrentLevel.InitLevel(this);
        }

        public Level FindLevel()
        {
            Array<Node> rootChildren = Root.GetChildren();

            for (int i = 0; i < rootChildren.Count; i++)
            {
                Node node = rootChildren[i];
                if (node is Level)
                {
                    return node as Level;
                }
            }
            return null;
        }

        public T FindLevel<T>() where T : Level => FindLevel() as T;

        public Level FindLevel(Node contextNode)
        {
            if (contextNode != null)
            {
                for (Node outerNode = contextNode; outerNode != null; outerNode = outerNode.GetParent())
                {
                    if (outerNode is Level)
                    {
                        return outerNode as Level;
                    }
                }
            }
            return null;
        }

        public T FindLevel<T>(Node contextNode) where T : Level => FindLevel(contextNode) as T;

        public void OpenLevel(string resourcePath)
        {
            if (CurrentLevel != null)
            {
                CurrentLevel.QueueFree();
            }

            PackedScene levelPackedScene = ResourceLoader.Load<PackedScene>(resourcePath);
            if (levelPackedScene != null)
            {
                Level newLevel = levelPackedScene.Instantiate() as Level;
                if (newLevel != null)
                {
                    CurrentLevel = newLevel;

                    CreateGameMode();
                    CurrentLevel.InitLevel(this);

                    Root.AddChild(newLevel);
                }
            }
        }

        public void CreateGameMode()
        {
            GameModeSettings gameModeSettings;
            if (CurrentLevel != null && CurrentLevel.GameModeSettingsOverride != null)
            {
                gameModeSettings = CurrentLevel.GameModeSettingsOverride;
            }
            else
            {
                string gameModeScriptPath = ProjectSettings.GetSetting("application/game_framework/default_game_mode_settings").AsString();
                gameModeSettings = GD.Load<GameModeSettings>(gameModeScriptPath);
            }

            if (gameModeSettings == null)
            {
                GD.PrintErr($"Load Game Mode Settings failed. Please update \"application/game_framework/default_game_mode_settings\" option in project settings.");
                return;
            }

            Node gameModeNode = new Node();
            gameModeNode.Name = "GameMode";
            ulong gameModeNodeId = gameModeNode.GetInstanceId();
            gameModeNode.SetScript(gameModeSettings.GameModeScript);

            if (GameMode != null)
            {
                GameMode.Free();
            }

            GameMode = InstanceFromId(gameModeNodeId) as GameMode;
            Root.AddChild(GameMode);
            GameMode.GameModeSettings = gameModeSettings;
            GameMode.InitGame(this);
        }
    }
}