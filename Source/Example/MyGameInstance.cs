using Godot;

using GameFramework.System;

namespace Example
{
    public partial class MyGameInstance : GameInstance
    {
        public override void Init()
        {
            base.Init();

            GD.Print("MyGameInstance.Init");
        }
    }
}