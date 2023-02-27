using Godot;

namespace GB.GameFramework
{ 
    public partial class GBGameInstance : GodotObject
	{
        private SceneTree SceneTreeObject;

        public GBGameInstance() { }

        public virtual void Init() { }

        public SceneTree GetSceneTree() => SceneTreeObject;

        public void SetSceneTree(SceneTree NewSceneTree)
        {
            if (NewSceneTree != null && NewSceneTree != SceneTreeObject)
            {
                SceneTreeObject = NewSceneTree;
            }
        }
    }
}