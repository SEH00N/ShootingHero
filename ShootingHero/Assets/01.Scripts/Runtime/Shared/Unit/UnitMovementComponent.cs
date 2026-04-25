using UnityEngine;

namespace ShootingHero.Shared
{
    public class UnitMovementComponent : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D unitRigidbody = null;

        [SerializeField]
        private float maxSpeed = 10f;

        [SerializeField]
        private float acceleration = 10f;

        private Vector2 movementInput = Vector2.zero;
        private Vector2 moveDirection = Vector2.zero;
        private float moveSpeed = 0f;
        private int currentDirection = 0;

        public Vector2 MovementInput => movementInput;

        private void FixedUpdate()
        {
            float acceleration = this.acceleration * (movementInput == Vector2.zero ? -1 : 1);
            moveSpeed = Mathf.Clamp(moveSpeed + Time.fixedDeltaTime * acceleration, 0, maxSpeed);

            unitRigidbody.linearVelocity = moveDirection * moveSpeed;

            if(movementInput.x != 0)
                SetDirection((int)Mathf.Sign(movementInput.x));
        }

        private void SetDirection(int direction)
        {
            if(currentDirection == direction)
                return;
            
            currentDirection = direction;
            transform.rotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 0);
        }

        public void SetMovementInput(Vector2 input)
        {
            movementInput = input.normalized;
            if(movementInput != Vector2.zero)
                moveDirection = movementInput;
        }
    }
}
