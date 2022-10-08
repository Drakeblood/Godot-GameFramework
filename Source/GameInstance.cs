using Godot;
using Godot.Collections;

using GodotFramework.Statics;

namespace GodotFramework.Core
{
	public partial class GameInstance : Node
	{
        public GameInstance()
        {

        }

        public override void _Ready()
        {
            base._Ready();
            
            OpenLevel(ProjectStatics.StartupLevelResourcePath);
        }

        public static GameInstance GetGameInstance(SceneTree SceneTreeObject)
        {
            if (SceneTreeObject != null)
            {
                return SceneTreeObject.Root.GetChildOrNull<GameInstance>(0);
            }
            return null;
        }

        public static T GetGameInstance<T>(SceneTree SceneTreeObject) where T : GameInstance => GetGameInstance(SceneTreeObject) as T;

        public void OpenLevel(string ResourcePath)
        {
            PackedScene LevelPackedScene = ResourceLoader.Load<PackedScene>(ResourcePath);
            if(LevelPackedScene != null)
            {
                AddChild(LevelPackedScene.Instantiate());
            }
        }

        public Level GetCurrentLevel()
        {
            Level CurrentLevel = null;
            Array<Node> GameInstanceChildren = GetChildren();

            foreach(Node NodeObject in GameInstanceChildren)
            {
                CurrentLevel = NodeObject as Level;

                if (CurrentLevel != null)
                {
                    return CurrentLevel;
                }
            }
            return null;
        }

        public T GetCurrentLevel<T>() where T : Level => GetCurrentLevel() as T;
    }
}