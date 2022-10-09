using Godot;

namespace GB.Core
{ 
    public partial class GameInstance : Godot.Object
	{
        private SceneTree SceneTreeObject;

        public GameInstance() { }

        public virtual void Init() { }

        public void SetSceneTree(SceneTree _SceneTreeObject)
        {
            if (_SceneTreeObject != null && SceneTreeObject != _SceneTreeObject)
            {
                SceneTreeObject = _SceneTreeObject;
            }
        }

        public static GameInstance GetGameInstance(SceneTree _SceneTreeObject) => _SceneTreeObject.GetScript().AsGodotObject() as GameInstance;
        public static T GetGameInstance<T>(SceneTree _SceneTreeObject) where T : GameInstance => GetGameInstance(_SceneTreeObject) as T;
    }
}