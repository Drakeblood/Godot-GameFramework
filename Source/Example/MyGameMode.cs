using Godot;

using GameFramework.System;

namespace Example
{
    public partial class MyGameMode : GameMode
    {
        public override void InitGame(GFSceneTree sceneTree)
        {
            base.InitGame(sceneTree);

            GD.Print("MyGameMode.Init");
        }
    }
}