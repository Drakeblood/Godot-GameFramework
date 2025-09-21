using Godot;

using GameFramework.Assertion;

namespace GameFramework.Core
{
    public partial class PlayerController : Controller
    {
        [Export] public bool AutoManageActiveCameraTarget = true;
        protected Player Player;
        protected InputComponent InputComponent = new InputComponent();

        public void SetPlayer(Player player)
        {
            this.Player = player;
            player.PlayerController = this;
        }

        public override void OnPossess(PawnHandler pawnToPossess)
        {
            if (pawnToPossess != null)
            {
                bool bNewPawn = (PawnHandler != pawnToPossess);

                if (PawnHandler != null && bNewPawn)
                {
                    UnPossess();
                }

                if (pawnToPossess.Controller != null)
                {
                    pawnToPossess.Controller.UnPossess();
                }

                pawnToPossess.PossessedBy(this);

                SetPawnHandler(pawnToPossess);
                Assert.IsNotNull(PawnHandler);

                if (AutoManageActiveCameraTarget)
                {
                    Node CameraNode = PawnHandler.GetParent().FindChild("Camera");
                    if (CameraNode != null)
                    {
                        if (CameraNode.IsInsideTree()) { SetPawnCameraNodeAsCurrent(); }
                        else { CallDeferred("SetPawnCameraNodeAsCurrent"); }  
                    }
                }
            }
        }

        private void SetPawnCameraNodeAsCurrent()
        {
            Node CameraNode = PawnHandler.GetParent().FindChild("Camera");
            if (CameraNode != null && CameraNode.IsInsideTree())
            {
                if (CameraNode is Camera3D camera3D) { camera3D.MakeCurrent(); }
                else if (CameraNode is Camera2D camera2D) { camera2D.MakeCurrent(); }
            }
        }

        public override void OnUnPossess()
        {
            if (PawnHandler != null)
            {
                PawnHandler.UnPossessed();
            }

            SetPawnHandler(null);
        }
    }
}