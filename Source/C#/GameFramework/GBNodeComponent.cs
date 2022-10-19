using Godot;

namespace GB.GameFramework
{
    public partial class GBNodeComponent : Godot.Object
    {
        private Node ComponentOwner;

        public GBNodeComponent(Node ComponentOwnerReference)
        {
            ComponentOwner = ComponentOwnerReference;
        }

        public T GetOwner<T>() where T : Node => ComponentOwner as T;
    }
}