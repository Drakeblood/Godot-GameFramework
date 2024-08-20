using Godot;

using GameFramework.Core;

namespace Example
{
    public partial class MyGameInstance : GameInstance
    {
        public override void Init(GFSceneTree sceneTree)
        {
            base.Init(sceneTree);

            GD.Print("MyGameInstance.Init");
        }

        protected override void InitTreeAvailable()
        {
            base.InitTreeAvailable();

            GD.Print("MyGameInstance.InitTreeAvailable");
            //SceneTree.Root.Size = DisplayServer.ScreenGetSize();
        }
    }
}