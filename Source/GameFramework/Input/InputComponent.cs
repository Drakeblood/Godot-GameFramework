using Godot;
using System;
using System.Collections.Generic;

public class InputComponent
{
    public bool Enabled = true;
    private List<ActionInputBinding> bindings = new List<ActionInputBinding>();

    public virtual void UpdateInput()
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
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                bindings[i].AddBinding(triggerEvent, action);
                return;
            }
        }

        bindings.Add(new ActionInputBinding(actionName));
        bindings[bindings.Count - 1].AddBinding(triggerEvent, action);
    }

    public virtual void RemoveBinding(StringName actionName, TriggerEvent triggerEvent, Action action)
    {
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                bindings[i].RemoveBinding(triggerEvent, action);
                return;
            }
        }
    }

    public virtual void RemoveBinding(StringName actionName)
    {
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].ActionName == actionName)
            {
                bindings.RemoveAt(i);
                return;
            }
        }
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
        for (int i = 0; i < bindings.Count; i++)
        {
            bindings[i].RemoveAllBindings();
        }
    }
}
