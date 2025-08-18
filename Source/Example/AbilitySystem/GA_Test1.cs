using Godot;

public partial class GA_Test1 : GameFramework.AbilitySystem.GameplayAbility
{
    [Export]
    int a = 100;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        GD.Print(a);
        EndAbility();
    }

    public override void OnGiveAbility()
    {
        base.OnGiveAbility();
        GD.Print("GA_Test1.OnGiveAbility");
    }
}
