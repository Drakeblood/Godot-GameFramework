using Godot;
using Godot.Collections;

namespace GameFramework.Core
{
    public partial class Level : Node
    {
        [Export]
        public GameModeSettings GameModeSettingsOverride;

        public virtual void InitLevel(GFSceneTree sceneTree)
        {
            PlayerStartsLocations.Resize(playerStartsNodePathes.Count);
            PlayerStartsRotations.Resize(playerStartsNodePathes.Count);

            for (int i = 0; i < playerStartsNodePathes.Count; i++)
            {
                Node node = GetNodeOrNull(playerStartsNodePathes[i]);

                if (node is Node2D playerStart2D)
                {
                    PlayerStartsLocations[i] = new Vector3(playerStart2D.Position.X, playerStart2D.Position.Y, 0f);
                    PlayerStartsRotations[i] = new Vector3(playerStart2D.Rotation, 0f, 0f);
                }
                else if (node is Node3D playerStart3D)
                {
                    PlayerStartsLocations[i] = playerStart3D.Position;
                    PlayerStartsRotations[i] = playerStart3D.Rotation;
                }
            }
        }

        public Array<Vector3> PlayerStartsLocations = new Array<Vector3>();
        public Array<Vector3> PlayerStartsRotations = new Array<Vector3>();

        [Export]
        private Array<NodePath> playerStartsNodePathes = new Array<NodePath>();
    }
}
