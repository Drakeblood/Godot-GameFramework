using Godot;

using GameFramework.Core;

namespace Example
{
    public partial class MainMenuLevel : Level
    {
        public override void InitLevel(GFSceneTree sceneTree)
        {
            base.InitLevel(sceneTree);

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