using Godot;

namespace GameFramework.System
{ 
    public partial class GameInstance : GodotObject
	{
        public GFSceneTree OwningSceneTree { get; set; }

        public GameInstance() { }

        public virtual void Init() { }
    }
}