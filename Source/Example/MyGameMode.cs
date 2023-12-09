using Godot;

using GameFramework.System;

namespace Example
{
    public partial class MyGameMode : GameMode
    {
        public override void InitGame()
        {
            base.InitGame();

            GD.Print("MyGameMode.Init");
        }
    }
}