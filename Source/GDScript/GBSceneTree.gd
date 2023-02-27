extends SceneTree
class_name GBSceneTree

func _initialize():
	var script = load("res://Example/Source/MyGameInstance.cs").new()
	script.call("SetSceneTree", self)
	set_script(script)
	script.call("Init")
	get_root().get_child(0).call("InitLevel")
	pass
