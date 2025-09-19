using Godot;

namespace GameFramework.Core
{
    public partial class Controller : Node
    {
        public Pawn Pawn { get; private set; }

        public virtual void SetPawn(Pawn inPawn)
        {
            Pawn = inPawn;
        }

        public void Possess(Pawn inPawn)
        {
            Pawn CurrentPawn = Pawn;

            OnPossess(inPawn);
        }

        public void UnPossess()
        {
            Pawn CurrentPawn = Pawn;
            if (CurrentPawn == null) { return; }

            OnUnPossess();
        }

        public virtual void OnPossess(Pawn inPawn)
        {
            bool bNewPawn = Pawn != inPawn;

            if (bNewPawn && Pawn != null) { UnPossess(); }
            if (inPawn == null) { return; }

            if (inPawn.Controller != null)
            {
                inPawn.Controller.UnPossess();
            }

            inPawn.PossessedBy(this);
            SetPawn(inPawn);
        }

        public virtual void OnUnPossess()
        {
            if (Pawn != null)
            {
                Pawn.UnPossessed();
                SetPawn(null);
            }
        }
    }
}