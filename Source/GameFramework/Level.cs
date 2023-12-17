using Godot;
using Godot.Collections;

namespace GameFramework.System
{
    public partial class Level : Node
    {
        [Export]
        public Script GameModeScriptOverride;

        public virtual void InitLevel(GFSceneTree sceneTree)
        {
            playerStartsLocations.Resize(playerStartsNodePathes.Count);

            for (int i = 0; i < playerStartsNodePathes.Count; i++)
            {
                Node node = GetNodeOrNull(playerStartsNodePathes[i]);

                if (node is Node2D playerStart2D)
                {
                    playerStartsLocations[i] = new Vector3(playerStart2D.Position.X, playerStart2D.Position.Y, 0f);
                }
                else if (node is Node3D playerStart3D)
                {
                    playerStartsLocations[i] = playerStart3D.Position;
                }
            }
        }

        private Array<Vector3> playerStartsLocations = new Array<Vector3>();

        [Export]
        private Array<NodePath> playerStartsNodePathes = new Array<NodePath>();
    }
}
