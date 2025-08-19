using Godot;

public partial class GA_Test1 : GameFramework.AbilitySystem.GameplayAbility
{
    [Export]
    int a = 100;

    private Timer timer;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        GD.Print(a);

        timer = new Timer();
        AbilitySystemComponent.AddChild(timer);
        timer.WaitTime = 2f;
        timer.OneShot = true;
        timer.Connect("timeout", Callable.From(EndTimer));
        timer.Start();
    }

    private void EndTimer()
    {
        EndAbility();
    }

    public override void InputPressed()
    {
        GD.Print("Input pressed");
        EndAbility(true);
    }

    public override void InputReleased()
    {
        GD.Print("Input released");
    }

    public override void OnGiveAbility()
    {
        base.OnGiveAbility();
        GD.Print("GA_Test1.OnGiveAbility");
    }

    public override void EndAbility(bool wasCanceled = false)
    {
        base.EndAbility(wasCanceled);

        timer.QueueFree();
        GD.Print("End ability: " + GetType() + " was canceled: " +  wasCanceled);
    }
}
