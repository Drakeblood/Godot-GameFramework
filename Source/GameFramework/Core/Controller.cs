using Godot;

namespace GameFramework.Core
{
    public partial class Controller : Node
    {
        public PawnHandler PawnHandler { get; private set; }

        public virtual void SetPawnHandler(PawnHandler inPawnHandler)
        {
            PawnHandler = inPawnHandler;
        }

        public void Possess(PawnHandler inPawnHandler)
        {
            PawnHandler CurrentPawn = PawnHandler;

            OnPossess(inPawnHandler);
        }

        public void UnPossess()
        {
            PawnHandler CurrentPawn = PawnHandler;
            if (CurrentPawn == null) { return; }

            OnUnPossess();
        }

        public virtual void OnPossess(PawnHandler inPawnHandler)
        {
            bool bNewPawnHandler = PawnHandler != inPawnHandler;

            if (bNewPawnHandler && PawnHandler != null) { UnPossess(); }
            if (inPawnHandler == null) { return; }

            if (inPawnHandler.Controller != null)
            {
                inPawnHandler.Controller.UnPossess();
            }

            inPawnHandler.PossessedBy(this);
            SetPawnHandler(inPawnHandler);
        }

        public virtual void OnUnPossess()
        {
            if (PawnHandler != null)
            {
                PawnHandler.UnPossessed();
            }

            SetPawnHandler(null);
        }
    }
}