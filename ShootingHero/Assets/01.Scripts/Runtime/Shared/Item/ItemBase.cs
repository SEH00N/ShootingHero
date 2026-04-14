using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class ItemBase : MonoBehaviour
    {
        protected abstract void OnInteract(Unit unit);
        public void Interact(Unit unit)
        {
            Vector2 distance = transform.position - unit.transform.position;
            if(distance.sqrMagnitude >= GameDefine.UNIT_INTERACT_DISTANCE * GameDefine.UNIT_INTERACT_DISTANCE)
                return;

            OnInteract(unit);
        }
    }
}
