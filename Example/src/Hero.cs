using Example;
using GameFramework.AbilitySystem;
using GameFramework.Core;
using GameFramework.GameplayTags;
using Godot;
using Godot.Collections;

public partial class Hero : Node2D, IPawn
{
    [Export] private GameplayTag x;
    [Export] private GameplayTagContainer tagContainer = new GameplayTagContainer();
    [Export] private Array<string> huC = new Array<string>();

    private long frameCounter = 0;

    public override void _Ready()
    {
        base._Ready();
        GD.Print("Hero._Ready");
        GD.Print(x);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        frameCounter++;
    }

    public void SetupInputComponent(InputComponent inputComponent) 
    {
        inputComponent.BindAction("fire", TriggerEvent.Started, () => { GD.Print(string.Format("Fire started {0}: {1}", frameCounter, Name)); });
        inputComponent.BindAction("fire", TriggerEvent.Triggered, () => { GD.Print(string.Format("Fire triggered {0}: {1}", frameCounter, Name)); });
        inputComponent.BindAction("fire", TriggerEvent.Completed, () => { GD.Print(string.Format("Fire completed {0}: {1}", frameCounter, Name)); });

        inputComponent.BindAction("switchPawn", TriggerEvent.Started, () =>
        {
            Hero Hero2 = ProjectStatics.GetLevel<Level>(GetTree()).GetNodeOrNull<Hero>("Hero2");
            if (Hero2 != null && Hero2 != this)
            {
                ProjectStatics.GetGameInstance<GameInstance>(GetTree()).GetPlayerController(0).Possess(Hero2.GetNode<PawnHandler>("PawnHandler"));
            }
            else
            {
                ProjectStatics.GetGameInstance<GameInstance>(GetTree()).GetPlayerController(0).Possess(ProjectStatics.GetGameMode<MyGameMode>(GetTree()).Hero.GetNode<PawnHandler>("PawnHandler"));
            }
        });
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
