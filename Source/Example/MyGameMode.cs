using Godot;

using GameFramework.Core;

namespace Example
{
    public partial class MyGameMode : GameMode
    {
        public override void InitGame(GFSceneTree sceneTree)
        {
            base.InitGame(sceneTree);

            GD.Print("MyGameMode.Init");
        }

        public override void _EnterTree()
        {
            GD.Print("MyGameMode._EnterTree");
        }
    }
}