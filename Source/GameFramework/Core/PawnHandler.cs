using Godot;

using GameFramework.Assertion;
using GameFramework.Core;

public partial class PawnHandler : Node
{
    [Export] private NodePath PawnNodePath;

    public Controller Controller { get; private set; }
    public IPawn Pawn { get; private set; }
    protected InputComponent InputComponent;

    public override void _Ready()
    {
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        Assert.IsNotNull(InputComponent);
        InputComponent.UpdateInput();
    }

    public virtual void PossessedBy(Controller NewController)
    {
        Controller = NewController;
        Pawn = GetNodeOrNull<IPawn>(PawnNodePath);

        if (Pawn != null)
        {
            Pawn.PossessedBy(Controller);

            if (InputComponent == null) { InputComponent = new InputComponent(); }
            Pawn.SetupInputComponent(InputComponent);

            if (!IsInsideTree()) { CallDeferred(Node.MethodName.SetProcess, true); }
            else { SetProcess(true); }
        }
    }

    public virtual void UnPossessed()
    {
        Controller = null;

        if (InputComponent != null)
        {
            InputComponent.RemoveAllBindings();
            InputComponent = null;
        }

        SetProcess(false);
    }
}
