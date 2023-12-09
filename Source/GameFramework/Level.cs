using Godot;

namespace GameFramework.System
{
    public partial class Level : Node
    {
        [Export]
        public Script GameModeScriptOverride;

        public virtual void InitLevel(GFSceneTree sceneTree)
        {

        }
    }
}
