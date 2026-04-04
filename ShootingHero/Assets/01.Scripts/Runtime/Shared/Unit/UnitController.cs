using UnityEngine;

namespace ShootingHero.Shared
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D unitRigidbody = null;

        [SerializeField]
        private float maxSpeed = 10f;

        [SerializeField]
        private float acceleration = 10f;

        private Vector2 input = Vector2.zero;
        private Vector2 moveDirection = Vector2.zero;
        private float speed = 0f;

        // debug
        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            SetInput(new Vector2(horizontal, vertical));
        }

        private void FixedUpdate()
        {
            float acceleration = this.acceleration * (input == Vector2.zero ? -1 : 1);
            speed = Mathf.Clamp(speed + Time.fixedDeltaTime * acceleration, 0, maxSpeed);

            unitRigidbody.linearVelocity = moveDirection * speed;
        }

        public void SetInput(Vector2 input)
        {
            this.input = input.normalized;
            if(this.input != Vector2.zero)
                moveDirection = this.input;
        }
    }
}
