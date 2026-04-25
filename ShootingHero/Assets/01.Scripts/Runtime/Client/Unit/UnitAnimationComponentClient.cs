using System;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class UnitAnimationComponentClient : DedicatedMonoBehaviour
    {
        private static readonly int IDLE_ANIMATION_HASH = Animator.StringToHash("Idle");
        private static readonly int WALK_ANIMATION_HASH = Animator.StringToHash("Walk");

        [SerializeField]
        private UnitMovementComponent unitMovementComponent = null;

        [SerializeField]
        private Animator animatorComponent = null;
        
        protected override EPlayMode DedicatedType => EPlayMode.Client;

        private int currentAnimationHash = 0;
        private int currentDirection = 0;

        protected override void OnAwake()
        {
            base.OnAwake();
            PlayAnimation(IDLE_ANIMATION_HASH);
        }

        private void LateUpdate()
        {
            Vector2 movementInput = unitMovementComponent.MovementInput;
            if(movementInput.sqrMagnitude > 0.1f)
                PlayAnimation(WALK_ANIMATION_HASH);
            else
                PlayAnimation(IDLE_ANIMATION_HASH);
        }

        private void PlayAnimation(int hash)
        {
            if(currentAnimationHash == hash)
                return;

            currentAnimationHash = hash;
            animatorComponent.Play(hash, 0, 0f);
            animatorComponent.Update(0f);
        }
    }
}