using System;

using Godot;

public class ActionInputBinding
{
    public StringName ActionName { get; set; }
    public Action[] Actions { get; set; }
    public bool Pressed { get; set; }

    public ActionInputBinding(StringName actionName) 
    {
        ActionName = actionName;
        Actions = new Action[5/*TriggerEventNum*/];
    }

    public void Execute(TriggerEvent triggerEvent)
    {
        Actions[(int)triggerEvent]?.Invoke();
    }

    public void AddBinding(TriggerEvent triggerEvent, Action action)
    {
        Actions[(int)triggerEvent] += action;
    }

    public void RemoveBinding(TriggerEvent triggerEvent, Action action)
    {
        Actions[(int)triggerEvent] -= action;
    }

    public void RemoveAllBindings()
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i] = null;
        }
    }
}