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

        public virtual void InitGame(GFSceneTree sceneTree)
        {
            gfSceneTree = sceneTree;

            CreatePlayer();
        }

        public virtual void CreatePlayer()
        {
            if (GameModeSettings.PlayerScene == null) return;//TO DO add log here

            if (gfSceneTree.CurrentLevel.PlayerStartsLocations.Count > 0)
            {
                Node player = GameModeSettings.PlayerScene.Instantiate();

                if (player is Node2D player2D)
                {
                    Vector3 playerPosition = gfSceneTree.CurrentLevel.PlayerStartsLocations[0];
                    player2D.Position = new Vector2(playerPosition.X, playerPosition.Y);
                    player2D.Rotation = gfSceneTree.CurrentLevel.PlayerStartsRotations[0].X;
                }
                else if (player is Node3D player3D)
                {
                    player3D.Position = gfSceneTree.CurrentLevel.PlayerStartsLocations[0];
                    player3D.Rotation = gfSceneTree.CurrentLevel.PlayerStartsRotations[0];
                }
            }
        }
    }
}