using Godot;

using GB.GameFramework;

namespace Example
{
    public partial class MainMenuLevel : GBLevel
    {
        public override void InitLevel()
        {
            base.InitLevel();

            GD.Print("MainMenuLevel.InitLevel");
        }
    }
}