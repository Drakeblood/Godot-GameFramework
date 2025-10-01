# Godot-GameFramework
  GameFramework for Godot inspired by Unreal Engine

Features
  - ...

# How to install
  Copy addons, Config, Content, Source folders to your project.
  Add project settings to .godot file
  
  ```ini
  [application]
  run/main_loop_type="GFSceneTree"
  game_framework/game_instance_script="res://Source/GameFramework/Core/GameInstance.cs"
  game_framework/default_game_mode_settings="res://Content/GameFramework/DefaultGameModeSettings.tres"
  game_framework/gameplay_tags_files=PackedStringArray("res://Config/Tags/DefaultGameplayTags.ini")
  
  [editor_plugins]
  enabled=PackedStringArray("res://addons/GameFramework/plugin.cfg")
  ```
  
Main hero scene should contains Node child with PawnHandler script

# Export
Add "Config/*" to "Filters to export non-resources files/folders" field. (Export->Resources)