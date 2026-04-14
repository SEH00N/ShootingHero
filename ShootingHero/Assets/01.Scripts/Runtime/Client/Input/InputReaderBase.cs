using UnityEngine.InputSystem;

namespace ShootingHero.Clients
{
    public abstract class InputReaderBase
    {
        public virtual void Initialize(InputActions inputActions) { }
        public virtual void Update() { }
        public virtual void Release() { }

        public abstract InputActionMap GetInputActionMap();
    }
}