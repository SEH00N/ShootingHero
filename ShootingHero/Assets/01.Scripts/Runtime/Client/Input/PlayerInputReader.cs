using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShootingHero.Clients
{
    public class PlayerInputReader : InputReaderBase, InputActions.IPlayerActions
    {
        private InputActionMap inputActionMap = null;
        public override InputActionMap GetInputActionMap() => inputActionMap;

        public Vector2 MovementInput { get; private set; }
        public Vector2 AimPosition { get; private set; }

        public event Action OnFireStartEvent = null;
        public event Action OnFireEndEvent = null;
        public event Action OnInteractEvent = null;
        public event Action OnReloadEvent = null;

        public override void Initialize(InputActions inputActions)
        {
            base.Initialize(inputActions);

            InputActions.PlayerActions playerActions = inputActions.Player;
            playerActions.SetCallbacks(this);
            inputActionMap = playerActions.Get();
        }

        void InputActions.IPlayerActions.OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                MovementInput = Vector2.zero;
                return;
            }

            MovementInput = context.ReadValue<Vector2>().normalized;
        }

        void InputActions.IPlayerActions.OnAim(InputAction.CallbackContext context)
        {
            AimPosition = context.ReadValue<Vector2>();
        }

        void InputActions.IPlayerActions.OnFire(InputAction.CallbackContext context)
        {
            if(context.started == true)
                OnFireStartEvent?.Invoke();

            if(context.canceled == true)
                OnFireEndEvent?.Invoke();
        }

        void InputActions.IPlayerActions.OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnInteractEvent?.Invoke();
        }

        void InputActions.IPlayerActions.OnReload(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnReloadEvent?.Invoke();
        }
    }
}
