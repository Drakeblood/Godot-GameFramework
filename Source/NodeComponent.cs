using Godot;
using System;

public partial class NodeComponent : Godot.Object
{
    private Node ComponentOwner;

    public NodeComponent(Node _ComponentOwner)
    {
        ComponentOwner = _ComponentOwner;
    }

    public T GetOwner<T>() where T : Node => ComponentOwner as T;
}
