using Godot;

using GameFramework.System;

namespace Example
{
    public partial class MyGameInstance : GameInstance
    {
        public override void Init(GFSceneTree sceneTree)
        {
            base.Init(sceneTree);

            GD.Print("MyGameInstance.Init");
        }
    }
}