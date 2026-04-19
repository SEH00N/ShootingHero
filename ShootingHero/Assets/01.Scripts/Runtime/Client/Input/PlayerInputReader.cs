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
        public event Action<int> OnWeaponChangeEvent = null;

        public override void Initialize(InputActions inputActions)
        {
            base.Initialize(inputActions);

            InputActions.PlayerActions playerActions = inputActions.Player;
            playerActions.SetCallbacks(this);
            inputActionMap = playerActions.Get();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                MovementInput = Vector2.zero;
                return;
            }

            MovementInput = context.ReadValue<Vector2>().normalized;
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            AimPosition = context.ReadValue<Vector2>();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if(context.started == true)
                OnFireStartEvent?.Invoke();

            if(context.canceled == true)
                OnFireEndEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnInteractEvent?.Invoke();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnReloadEvent?.Invoke();
        }

        public void OnChangeWeapon1(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnWeaponChangeEvent?.Invoke(1);
        }

        public void OnChangeWeapon2(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnWeaponChangeEvent?.Invoke(2);
        }

        public void OnChangeWeapon3(InputAction.CallbackContext context)
        {
            if(context.performed == false)
                return;

            OnWeaponChangeEvent?.Invoke(3);
        }
    }
}
