using Godot;

using GameFramework.System;

namespace Example
{
    public partial class MainMenuLevel : Level
    {
        public override void InitLevel()
        {
            base.InitLevel();

            GD.Print("MainMenuLevel.InitLevel");
        }
    }
}