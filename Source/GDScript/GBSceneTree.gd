extends SceneTree
class_name GBSceneTree

func _initialize():
	var script = load("res://Source/C#/GameFramework/GBGameInstance.cs").new()
	script.call("SetSceneTree", self)
	set_script(script)
	script.call("Init")
	pass