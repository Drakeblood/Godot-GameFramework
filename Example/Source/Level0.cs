using Godot;

using GameFramework.System;

namespace Example
{
    public partial class Level0 : Level
    {
        public override void InitLevel()
        {
            base.InitLevel();

            GD.Print("Level0.InitLevel");
        }

        public override void _EnterTree()
        {
            GD.Print("Level0._EnterTree");

            GetNode<Button>("Button").ButtonDown += OnButtonDown;
        }

        private void OnButtonDown()
        {
            GFSceneTree gfSceneTree = GetTree() as GFSceneTree;

            if(gfSceneTree != null )
            {
                gfSceneTree.OpenLevel("res://Example/Content/Scenes/MainMenuLevel.tscn");
            }
        }
    }
}