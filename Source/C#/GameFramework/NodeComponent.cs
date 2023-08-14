using Godot;

namespace GameFramework.System
{
    public partial class NodeComponent : GodotObject
    {
        private Node Owner;

        public NodeComponent(Node InOwner)
        {
            Owner = InOwner;
        }

        public T GetOwner<T>() where T : Node => Owner as T;
    }
}