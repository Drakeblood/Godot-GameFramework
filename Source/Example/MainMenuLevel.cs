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

        public override void _EnterTree()
        {
            GD.Print("MainMenuLevel._EnterTree");

            GetNode<Button>("Button").ButtonDown += OnButtonDown;
        }

        private void OnButtonDown()
        {
            GFSceneTree gfSceneTree = GetTree() as GFSceneTree;

            if(gfSceneTree != null )
            {
                gfSceneTree.OpenLevel("res://Content/Example/Scenes/Level0.tscn");
            }
        }
    }
}