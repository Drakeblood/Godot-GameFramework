using GameFramework.AbilitySystem;
using GameFramework.GameplayTags;
using Godot;
using Godot.Collections;

public partial class Hero : Node2D
{
    [Export] private GameplayTag x;

    [Export] private GameplayTagContainer tagContainer = new GameplayTagContainer();

    [Export] private Array<string> huC = new Array<string>();

    public override void _Ready()
    {
        base._Ready();
        GD.Print("Hero._Ready");
        GD.Print(x);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Q)
            {
                var ASC = GetNode<AbilitySystemComponent>("ASC");
                ASC.TryActivateAbility(typeof(GA_Test1));
            }
        }
    }
}
