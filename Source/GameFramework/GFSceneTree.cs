using Godot;
using Godot.Collections;

namespace GameFramework.System
{
    [GlobalClass]
    public sealed partial class GFSceneTree : SceneTree
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

        public override partial void _Initialize();

        public partial Level FindLevel();
        public T FindLevel<T>() where T : Level => FindLevel() as T;

        public partial Level FindLevel(Node contextNode);
        public T FindLevel<T>(Node contextNode) where T : Level => FindLevel(contextNode) as T;

        public partial void OpenLevel(string resourcePath);

        public partial void CreateGameMode();
    }

    public sealed partial class GFSceneTree : SceneTree
    {
        public override partial void _Initialize()
        {
            {
                string gameInstanceScriptPath = ProjectSettings.GetSetting("application/game_framework/game_instance").AsString();
                Script gameInstanceScript = GD.Load<Script>(gameInstanceScriptPath);

                if (gameInstanceScript == null)
                {
                    GD.PrintErr($"Load script from {gameInstanceScriptPath} path failed. Please update \"application/game_framework/game_instance\" option in project settings.");
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

        public partial Level FindLevel()
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

        public partial Level FindLevel(Node contextNode)
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

        public partial void OpenLevel(string resourcePath)
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

        public partial void CreateGameMode()
        {
            Script gameModeScript;
            if (CurrentLevel != null && CurrentLevel.GameModeScriptOverride != null)
            {
                gameModeScript = CurrentLevel.GameModeScriptOverride;
            }
            else
            {
                string gameModeScriptPath = ProjectSettings.GetSetting("application/game_framework/default_game_mode").AsString();
                gameModeScript = GD.Load<Script>(gameModeScriptPath);
            }

            if (gameModeScript == null)
            {
                GD.PrintErr($"Load script from {gameModeScript} path failed. Please update \"application/game_framework/default_game_mode\" option in project settings.");
                return;
            }

            Node gameModeNode = new Node();
            gameModeNode.Name = "GameMode";
            ulong gameModeNodeId = gameModeNode.GetInstanceId();
            gameModeNode.SetScript(gameModeScript);

            if (GameMode != null)
            {
                GameMode.Free();
            }

            GameMode = InstanceFromId(gameModeNodeId) as GameMode;
            Root.AddChild(GameMode);
            GameMode.InitGame(this);
        }
    }

}