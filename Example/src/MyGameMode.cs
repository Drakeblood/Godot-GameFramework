using Godot;

using GameFramework.Core;

namespace Example
{
    public partial class MyGameMode : GameMode
    {
        public Hero Hero { get; set; }

        public override void InitGame(GFSceneTree sceneTree)
        {
            base.InitGame(sceneTree);

            GD.Print("MyGameMode.Init");
        }

        public override void _EnterTree()
        {
            GD.Print("MyGameMode._EnterTree");
        }

        public override void PostLogin(PlayerController playerController)
        {
            base.PostLogin(playerController);
            Hero = playerController.PawnHandler.GetParent<Hero>();
        }
    }
}