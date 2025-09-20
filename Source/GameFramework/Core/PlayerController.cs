using Godot;

using GameFramework.Assertion;

namespace GameFramework.Core
{
    public partial class PlayerController : Controller
    {
        [Export] public bool AutoManageActiveCameraTarget = true;
        private Player player;

        public void SetPlayer(Player player)
        {
            this.player = player;
            player.PlayerController = this;
        }

        public override void OnPossess(Pawn pawnToPossess)
        {
            if (pawnToPossess != null)
            {
                bool bNewPawn = (Pawn != pawnToPossess);

                if (Pawn != null && bNewPawn)
                {
                    UnPossess();
                }

                if (pawnToPossess.Controller != null)
                {
                    pawnToPossess.Controller.UnPossess();
                }

                pawnToPossess.PossessedBy(this);

                SetPawn(pawnToPossess);
                Assert.IsNotNull(Pawn);

                if (AutoManageActiveCameraTarget)
                {
                    Node CameraNode = Pawn.GetParent().FindChild("Camera");
                    if (CameraNode != null)
                    {
                        if (CameraNode is Camera3D camera3D) { camera3D.MakeCurrent(); }
                        else if (CameraNode is Camera2D camera2D) { camera2D.MakeCurrent(); }
                    }
                }
            }
        }

        public override void OnUnPossess()
        {
            if (Pawn != null)
            {
                Pawn.UnPossessed();
            }

            SetPawn(null);
        }
    }
}