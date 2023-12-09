using Godot;

namespace GameFramework.System
{
    public partial class NodeComponent : GodotObject
    {
        private Node owner;

        public NodeComponent(Node inOwner)
        {
            owner = inOwner;
        }

        public T GetOwner<T>() where T : Node => owner as T;
    }
}