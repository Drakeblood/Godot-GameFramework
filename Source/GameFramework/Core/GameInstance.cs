using System.Collections.Generic;

using Godot;

namespace GameFramework.Core
{ 
    public partial class GameInstance : GodotObject
	{
        private GFSceneTree sceneTree;
        public GFSceneTree SceneTree
        {
            get => sceneTree;
            private set { sceneTree = value; }
        }

        private List<object> localPlayers = new List<object>();
        public List<object> LocalPlayers
        {
            get => localPlayers;
            private set { localPlayers = value; }
        }

        public GameInstance() { }

        public virtual void Init(GFSceneTree sceneTree)
        {
            SceneTree = sceneTree;
        }
    }
}