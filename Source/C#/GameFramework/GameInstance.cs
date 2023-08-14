using Godot;

namespace GameFramework.System
{ 
    public partial class GameInstance : GodotObject
	{
        public SceneTree SceneTree { get; set; }

        public GameInstance() { }

        public virtual void Init() { }
    }
}