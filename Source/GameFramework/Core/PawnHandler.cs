using Godot;

using GameFramework.Core;

public partial class PawnHandler : Node
{
    [Export] private NodePath PawnNodePath;

    public Controller Controller { get; private set; }
    public IPawn Pawn { get; private set; }
    protected InputComponent InputComponent;

    public virtual void PossessedBy(Controller NewController)
    {
        Controller = NewController;

        Pawn = GetNodeOrNull<IPawn>(PawnNodePath);
        if (Pawn != null)
        {
            Pawn.PossessedBy(Controller);
        }

        if (NewController is PlayerController playerController)
        {
            if (InputComponent == null)
            {
                InputComponent = new InputComponent();
                GetNode<Node>(PawnNodePath).AddChild(InputComponent);
            }

            playerController.RegisterInputComponent(InputComponent);
            if (Pawn != null)
            {
                Pawn.SetupInputComponent(InputComponent);
            }
        }
        
    }

    public virtual void UnPossessed()
    {
        if (Controller is PlayerController playerController)
        {
            playerController.UnregisterInputComponent(InputComponent);
        }

        Controller = null;

        if (Pawn != null)
        {
            Pawn.UnPossessed();
        }

        if (InputComponent != null)
        {
            CallDeferred("ClearInputComponent");
        }
    }

    private void ClearInputComponent()
    {
        InputComponent.Enabled = false;
        InputComponent.RemoveAllBindings();
        InputComponent.QueueFree();
        InputComponent = null;
    }
}
