using GameFramework.Assertion;
using Godot;

namespace GameFramework.Core
{
    public partial class GameMode : Node
    {
        private GFSceneTree gfSceneTree;

        private GameModeSettings gameModeSettings;
        public GameModeSettings GameModeSettings
        { 
            get => gameModeSettings; 
            set { gameModeSettings = value; } 
        }

        /// <summary>
        /// Called before being added to tree.
        /// </summary>
        public virtual void InitGame(GFSceneTree sceneTree)
        {
            gfSceneTree = sceneTree;
            gfSceneTree.GameInstance.Heroes.Clear(); 
        }

        public virtual PlayerController Login(Player player)
        {
            return SpawnPlayerController();
        }

        public virtual void PostLogin(PlayerController playerController)
        {
            playerController.Pawn = SpawnPawnAtPlayerStart();
        }

        public virtual PlayerController SpawnPlayerController()
        {
            return GameModeSettings.PlayerControllerScene.Instantiate<PlayerController>();
        }

        public virtual Pawn SpawnPawnAtPlayerStart()
        {
            Assert.IsNotNull(GameModeSettings.PawnScene, "PawnScene is not valid");
            
            Node newNode = GameModeSettings.PawnScene.Instantiate();
            Pawn pawn = newNode as Pawn;

            if (pawn == null)
            {
                var children = newNode.GetChildren();

                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] is Pawn)
                    {
                        pawn = (Pawn)children[i];
                        break;
                    }
                }
            }

            Assert.IsNotNull(pawn, "Pawn was not found.");
            gfSceneTree.CurrentLevel.AddChild(newNode);

            gfSceneTree.CurrentLevel.PreparePlayerStartsLocation();
            if (gfSceneTree.CurrentLevel.PlayerStartsLocations.Count > 0)
            {
                if (newNode is Node2D player2D)
                {
                    Vector3 playerPosition = gfSceneTree.CurrentLevel.PlayerStartsLocations[0];
                    player2D.Position = new Vector2(playerPosition.X, playerPosition.Y);
                    player2D.Rotation = gfSceneTree.CurrentLevel.PlayerStartsRotations[0].X;
                }
                else if (newNode is Node3D player3D)
                {
                    player3D.Position = gfSceneTree.CurrentLevel.PlayerStartsLocations[0];
                    player3D.Rotation = gfSceneTree.CurrentLevel.PlayerStartsRotations[0];
                }
            }
            
            return pawn;
        }
    }
}