#if TOOLS
using Godot;

[Tool]
public partial class GameFrameworkEditorPlugin : EditorPlugin
{
	private GameFrameworkEditorInspector plugin;

    public override void _EnterTree()
    {
        plugin = new GameFrameworkEditorInspector();
        AddInspectorPlugin(plugin);
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(plugin);
    }
}
#endif
