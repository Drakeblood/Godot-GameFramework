using GameFramework.Statics;
using Godot;

namespace GameFramework.System
{
    public partial class GameMode : Node
    {
        private GFSceneTree gfSceneTree;

        public virtual void InitGame(GFSceneTree sceneTree)
        {
            gfSceneTree = sceneTree;
        }

        //public virtual void CreatePlayer()
        //{
            //if(gfSceneTree.CurrentLevel.PlayerStartsLocations.Count > 0)
            //{

            //}
        //}
    }
}