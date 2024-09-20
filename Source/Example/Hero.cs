using GameFramework.AbilitySystem;
using Godot;
using System;

public partial class Hero : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        GD.Print("Hero._Ready");
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
