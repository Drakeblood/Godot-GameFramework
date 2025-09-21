using Godot;

using GameFramework.Assertion;

namespace GameFramework.Core
{
    public partial class GameMode : Node
    {
        protected GFSceneTree SceneTree;

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
            SceneTree = sceneTree;
            SceneTree.GameInstance.Heroes.Clear();
        }

        public virtual PlayerController Login(Player player)
        {
            return SpawnPlayerController();
        }

        public virtual void PostLogin(PlayerController playerController)
        {
            playerController.Possess(SpawnPawnAtPlayerStart());
        }

        public virtual PlayerController SpawnPlayerController()
        {
            PlayerController playerController = GameModeSettings.PlayerControllerScene.Instantiate<PlayerController>();
            SceneTree.CurrentLevel.AddChild(playerController);
            return playerController;
        }

        public virtual PawnHandler SpawnPawnAtPlayerStart()
        {
            Assert.IsNotNull(GameModeSettings.PawnScene, "PawnScene is not valid");

            Node newNode = GameModeSettings.PawnScene.Instantiate();
            PawnHandler pawn = null;
            var children = newNode.GetChildren();

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is PawnHandler)
                {
                    pawn = (PawnHandler)children[i];
                    break;
                }
            }

            Assert.IsNotNull(pawn, "Pawn was not found. Please add Node with Pawn script to your hero scene.");
            SceneTree.CurrentLevel.AddChild(newNode);

            SceneTree.CurrentLevel.PreparePlayerStartsLocation();
            if (SceneTree.CurrentLevel.PlayerStartsLocations.Count > 0)
            {
                if (newNode is Node2D player2D)
                {
                    Vector3 playerPosition = SceneTree.CurrentLevel.PlayerStartsLocations[0];
                    player2D.Position = new Vector2(playerPosition.X, playerPosition.Y);
                    player2D.Rotation = SceneTree.CurrentLevel.PlayerStartsRotations[0].X;
                }
                else if (newNode is Node3D player3D)
                {
                    player3D.Position = SceneTree.CurrentLevel.PlayerStartsLocations[0];
                    player3D.Rotation = SceneTree.CurrentLevel.PlayerStartsRotations[0];
                }
            }

            return pawn;
        }
    }
}