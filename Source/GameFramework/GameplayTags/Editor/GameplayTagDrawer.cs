#if TOOLS
using Godot;

public partial class GameplayTagDrawer : EditorInspectorPlugin
{
	private OptionButton dropdown;

    public override bool _CanHandle(GodotObject @object)
    {
        return true;
    }

	public override bool _ParseProperty(GodotObject @object, Variant.Type type,
        string name, PropertyHint hintType, string hintString,
        PropertyUsageFlags usageFlags, bool wide)
    {
        if (type == Variant.Type.Object)
        {
			if (name != "x") { return false; }
            AddPropertyEditor(name, new GameplayTagDrawerProperty());

			return true;
        }

        return false;
    }

	private void OnGameplayTagOptionSelected(int index)
	{
		dropdown.GetItemText(index);
	}
}
#endif