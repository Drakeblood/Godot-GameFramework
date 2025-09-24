using GameFramework.Assertion;
using Godot;
using System.Collections.Generic;

namespace GameFramework.Core
{
    public partial class PlayerController : Controller
    {
        [Export] public bool AutoManageActiveCameraTarget = true;
        protected Player Player;
        protected InputComponent InputComponent;
        private List<InputComponent> currentInputStack = new List<InputComponent>();

        public override void _EnterTree()
        {
            //InputComponent = new InputComponent();
            //AddChild(InputComponent);
            //RegisterInputComponent(InputComponent);
        }

        public override void _Process(double delta)
        {
            for (int i = currentInputStack.Count - 1; i >= 0; i--)
            {
                if (!currentInputStack[i].Enabled) { continue; }

                for (int j = 0; j < currentInputStack[i].Bindings.Count; j++)
                {
                    if (Input.IsActionPressed(currentInputStack[i].Bindings[j].ActionName))
                    {
                        currentInputStack[i].Bindings[j].Execute(TriggerEvent.Triggered);
                    }
                }
            }
        }

        public override void _Input(InputEvent @event)
        {
            for (int i = currentInputStack.Count - 1; i >= 0; i--)
            {
                if (!currentInputStack[i].Enabled) { continue; }

                for (int j = 0; j < currentInputStack[i].Bindings.Count; j++)
                {
                    if (Input.IsActionJustPressed(currentInputStack[i].Bindings[j].ActionName))
                    {
                        currentInputStack[i].Bindings[j].Pressed = true;
                        currentInputStack[i].Bindings[j].Execute(TriggerEvent.Started);
                        currentInputStack[i].Bindings[j].Execute(TriggerEvent.Triggered);
                    }
                    else if (Input.IsActionJustReleased(currentInputStack[i].Bindings[j].ActionName))
                    {
                        currentInputStack[i].Bindings[j].Pressed = false;
                        currentInputStack[i].Bindings[j].Execute(TriggerEvent.Completed);
                    }
                }
            }
        }

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
                    Node CameraNode = GetParent().FindChild("Camera");
                    if (CameraNode != null)
                    {
                        if (CameraNode.IsInsideTree()) { SetPawnCameraNodeAsCurrent(); }
                        else { CallDeferred("SetPawnCameraNodeAsCurrent"); }
                    }
                }
            }
        }

        public void SetPawnCameraNodeAsCurrent()
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

        public void RegisterInputComponent(InputComponent input)
        {
            if (!currentInputStack.Contains(input))
            {
                currentInputStack.Add(input);
            }
        }

        public void UnregisterInputComponent(InputComponent input)
        {
            currentInputStack.Remove(input);
        }
    }
}