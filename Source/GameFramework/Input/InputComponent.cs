using System;
using System.Collections.Generic;

using Godot;

public partial class InputComponent : Node
{
    public bool Enabled { get; set; } = true;
    public List<ActionInputBinding> Bindings {  get; set; } = new List<ActionInputBinding>();

    public virtual void BindAction(StringName actionName, TriggerEvent triggerEvent, Action action)
    {
        bool found = false;
        for (int i = 0; i < Bindings.Count; i++)
        {
            if (Bindings[i].ActionName == actionName)
            {
                Bindings[i].AddBinding(triggerEvent, action);
                found = true;
                break;
            }
        }

        if (!found)
        {
            var newBinding = new ActionInputBinding(actionName);
            newBinding.AddBinding(triggerEvent, action);
            Bindings.Add(newBinding);
        }
    }

    public virtual void RemoveBinding(StringName actionName, TriggerEvent triggerEvent, Action action)
    {
        for (int i = 0; i < Bindings.Count; i++)
        {
            if (Bindings[i].ActionName == actionName)
            {
                Bindings[i].RemoveBinding(triggerEvent, action);
                break;
            }
        }
    }

    public virtual void RemoveBinding(StringName actionName)
    {
        for (int i = 0; i < Bindings.Count; i++)
        {
            if (Bindings[i].ActionName == actionName)
            {
                Bindings.RemoveAt(i);
                break;
            }
        }
    }

    public void RemoveAllBindings()
    {
        for (int i = Bindings.Count - 1; i >= 0; i--)
        {
            Bindings[i].RemoveAllBindings();
        }
        Bindings.Clear();
    }

    public bool IsActionPressed(StringName actionName)
    {
        if (!Enabled) return false;

        for (int i = 0; i < Bindings.Count; i++)
        {
            if (Bindings[i].Pressed && Bindings[i].ActionName == actionName) { return true; }
        }
        return false;
    }
}
