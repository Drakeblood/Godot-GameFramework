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
        switch(hintString)
        {
            case "GameplayTagContainer":
            {
                AddPropertyEditor(name, new GameplayTagContainerEditorProperty());
                return true;    
            }
            case "GameplayTag":
            {
                AddPropertyEditor(name, new GameplayTagEditorProperty());
                return true;
            }
        }

        return false;
    }

}
#endif