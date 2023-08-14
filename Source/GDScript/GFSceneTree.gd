extends SceneTree
class_name GFSceneTree

func _initialize():
	var script = load("res://Example/Source/MyGameInstance.cs").new()
	script.SceneTree = self
	set_script(script)
	script.call("Init")
	get_root().get_child(0).call("InitLevel")
	pass
