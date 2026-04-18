using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class DedicatedMonoBehaviour : MonoBehaviour
    {
        protected abstract EPlayMode DedicatedType { get; }
        protected bool IsValid => GameInstance.PlayMode == DedicatedType;

        protected virtual void OnAwake() { }
        private void Awake()
        {
            if(IsValid == false)
            {
                Destroy(this);
                return;
            }

            OnAwake();
        }

        protected virtual void OnDisabled() { }
        private void OnDisable()
        {
            if(IsValid == false)
                return;
            
            OnDisabled();
        }

        protected virtual void OnDestroyed() { }
        private void OnDestroy()
        {
            if(IsValid == false)
                return;
            
            OnDestroyed();
        }
    }
}