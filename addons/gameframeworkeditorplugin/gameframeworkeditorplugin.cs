#if TOOLS
using Godot;
using System;

[Tool]
public partial class gameframeworkeditorplugin : EditorPlugin
{
	private GameplayTagDrawer _plugin;

    public override void _EnterTree()
    {
        _plugin = new GameplayTagDrawer();
        AddInspectorPlugin(_plugin);
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(_plugin);
    }
}
#endif
