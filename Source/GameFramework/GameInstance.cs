using Godot;

namespace GameFramework.System
{ 
    public partial class GameInstance : GodotObject
	{
        private GFSceneTree sceneTree;
        public GFSceneTree SceneTree
        {
            get => sceneTree;
            private set { sceneTree = value; }
        }

        public GameInstance() { }

        public virtual void Init(GFSceneTree sceneTree)
        {
            SceneTree = sceneTree;
        }
    }
}