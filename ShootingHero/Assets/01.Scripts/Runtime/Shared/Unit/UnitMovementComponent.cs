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

        private void FixedUpdate()
        {
            float acceleration = this.acceleration * (movementInput == Vector2.zero ? -1 : 1);
            moveSpeed = Mathf.Clamp(moveSpeed + Time.fixedDeltaTime * acceleration, 0, maxSpeed);

            unitRigidbody.linearVelocity = moveDirection * moveSpeed;
        }

        public void SetMovementInput(Vector2 input)
        {
            movementInput = input.normalized;
            if(movementInput != Vector2.zero)
                moveDirection = movementInput;
        }
    }
}
