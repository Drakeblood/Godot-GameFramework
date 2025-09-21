using GameFramework.Core;

public interface IPawn
{
    public void PossessedBy(Controller NewController) { }
    public void UnPossessed() { }
    public void SetupInputComponent(InputComponent inputComponent) { }
}