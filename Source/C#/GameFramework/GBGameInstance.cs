using Godot;

namespace GB.GameFramework
{ 
    public partial class GBGameInstance : Godot.Object
	{
        private SceneTree SceneTreeObject;

        public GBGameInstance() { }

        public virtual void Init() { }

        public void SetSceneTree(SceneTree SceneTreeReference)
        {
            if (SceneTreeReference != null && SceneTreeObject != SceneTreeReference)
            {
                SceneTreeObject = SceneTreeReference;
            }
        }

        public static GBGameInstance GetGameInstance(SceneTree SceneTreeReference) => SceneTreeReference.GetScript().AsGodotObject() as GBGameInstance;
        public static T GetGameInstance<T>(SceneTree SceneTreeReference) where T : GBGameInstance => GetGameInstance(SceneTreeReference) as T;
    }
}