#if TOOLS
using Godot;

public partial class GameFrameworkEditorInspector : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        return true;
    }

	public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if (type == Variant.Type.Object)
        {
            if (@object.Get(name).AsGodotObject() is Resource resource)
            {
                if (resource.GetScript().AsGodotObject() is Script script)
                {
                    if (script.GetGlobalName() == "GameplayTag")
                    {
                        AddPropertyEditor(name, new GameplayTagEditorProperty());
                        return true;
                    }
                    else if (script.GetGlobalName() == "GameplayTagContainer")
                    {
                        AddPropertyEditor(name, new GameplayTagContainerEditorProperty());
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
#endif