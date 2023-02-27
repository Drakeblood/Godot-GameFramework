using Godot;

namespace GB.GameFramework
{
    public partial class GBNodeComponent : GodotObject
    {
        private Node ComponentOwner;

        public GBNodeComponent(Node Owner)
        {
            ComponentOwner = Owner;
        }

        public T GetOwner<T>() where T : Node => ComponentOwner as T;
    }
}