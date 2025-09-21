using System;
using System.Collections.Generic;

using Godot;

public partial class InputComponent : Node
{
    public bool Enabled = true;
    private List<ActionInputBinding> bindings = new List<ActionInputBinding>();

    public override void _Ready()
    {
        UpdateProcessStatus();
    }

    public override void _Process(double delta)
    {
        if (!Enabled) { return; }

        for (int i = 0; i < bindings.Count; i++)
        {
            if (Input.IsActionJustPressed(bindings[i].ActionName))
            {
                bindings[i].Pressed = true;
                bindings[i].Execute(TriggerEvent.Started);
                bindings[i].Execute(TriggerEvent.Triggered);
            }
            else if (Input.IsActionPressed(bindings[i].ActionName))
            {
                bindings[i].Execute(TriggerEvent.Triggered);
            }
            else if (Input.IsActionJustReleased(bindings[i].ActionName))
            {
                bindings[i].Pressed = false;
                bindings[i].Execute(TriggerEvent.Completed);
            }
        }
    }

    public virtual void BindAction(StringName actionName, TriggerEvent triggerEvent, Action action)
    {
        bool found = false;
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                bindings[i].AddBinding(triggerEvent, action);
                found = true;
                break;
            }
        }

        if (!found)
        {
            bindings.Add(new ActionInputBinding(actionName));
            bindings[bindings.Count - 1].AddBinding(triggerEvent, action);
        }

        UpdateProcessStatus();
    }

    public virtual void RemoveBinding(StringName actionName, TriggerEvent triggerEvent, Action action)
    {
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                bindings[i].RemoveBinding(triggerEvent, action);
                break;
            }
        }
        UpdateProcessStatus();
    }

    public virtual void RemoveBinding(StringName actionName)
    {
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                bindings.RemoveAt(i);
                break;
            }
        }
        UpdateProcessStatus();
    }

    public bool IsActionPressed(StringName actionName)
    {
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                return bindings[i].Pressed;
            }
        }
        return false;
    }

    public void RemoveAllBindings()
    {
        for (int i = bindings.Count - 1; i >= 0; i--)
        {
            bindings[i].RemoveAllBindings();
        }
        bindings.Clear();
        UpdateProcessStatus();
    }

    private void UpdateProcessStatus()
    {
        bool shouldProcess = false;

        if (bindings.Count > 0)
        {
            shouldProcess = true;
        }

        if (shouldProcess != IsProcessing()) { SetProcess(shouldProcess); }
    }
}
