using GameFramework.AbilitySystem;
using GameFramework.GameplayTags;
using Godot;

public partial class Hero : Node2D
{
    [Export]
    private GameplayTag x = GameplayTagsManager.Instance.GetTag("Character");

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
