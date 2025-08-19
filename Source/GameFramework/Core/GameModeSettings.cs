using Godot;

namespace GameFramework.Core
{
    [GlobalClass]
    public partial class GameModeSettings : Resource
    {
        [Export]
        public Script GameModeScript;

        [Export]
        public PackedScene PawnScene;

        [Export]
        public PackedScene PlayerControllerScene;
    }
}