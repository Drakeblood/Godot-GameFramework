using System.Collections.Generic;

using Godot;

namespace GameFramework.Core
{ 
    public partial class GameInstance : GodotObject
	{
        private GFSceneTree sceneTree;
        public GFSceneTree SceneTree
        {
            get => sceneTree;
            private set { sceneTree = value; }
        }

        private List<LocalPlayer> localPlayers = new List<LocalPlayer>();

        private List<object> heroes = new List<object>();
        public List<object> Heroes
        {
            get => heroes;
            private set { heroes = value; }
        }

        public GameInstance() { }

        public virtual void Init(GFSceneTree sceneTree)
        {
            SceneTree = sceneTree;
            SceneTree.TreeChanged += InitTreeAvailable;
        }

        ///<summary> Called during game initialization when SceneTree is set. </summary>
        protected virtual void InitTreeAvailable()
        {
            SceneTree.TreeChanged -= InitTreeAvailable;
        }

        public LocalPlayer CreateLocalPlayer()
        {
            LocalPlayer newLocalPlayer = new LocalPlayer();
            localPlayers.Add(newLocalPlayer);
            return newLocalPlayer;
        }

        public void LoginLocalPlayers()
        {
            for (int i = 0; i < localPlayers.Count; i++)
            {
                PlayerController playerController = sceneTree.GameMode.Login(localPlayers[i]);
                playerController.SetPlayer(localPlayers[i]);
                sceneTree.GameMode.PostLogin(playerController);
            }
        }
    }
}