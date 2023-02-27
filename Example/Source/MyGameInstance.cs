using Godot;

using GB.GameFramework;

namespace Example
{
    public partial class MyGameInstance : GBGameInstance
    {
        public override void Init()
        {
            base.Init();

            GD.Print("MyGameInstance.Init");
        }
    }
}