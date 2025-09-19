using GameFramework.Core;
using Godot;
using System;

public partial class Pawn : Node
{
    public Controller Controller { get; private set; }

    public Action<Controller> PossessedBy;
    public Action UnPossessed;

    public Pawn()
    {
        PossessedBy += PossessedByAction;
        UnPossessed += UnPossessedAction;
    }

    private void PossessedByAction(Controller NewController)
    {
        Controller = NewController;
    }

    public virtual void UnPossessedAction()
    {
        Controller = null;
    }
}