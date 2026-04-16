using System;
using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class ItemBase : MonoBehaviour
    {
        private int itemID = 0;
        private string uuid = string.Empty;

        public int ItemID => itemID;
        public string UUID => uuid;

        private Action destroyCallback = null;

        public void Initialize(int itemID, string uuid, Action destroyCallback)
        {
            this.itemID = itemID;
            this.uuid = uuid;
            this.destroyCallback = destroyCallback;
        }

        protected abstract void OnInteract(Unit unit);
        public void Interact(Unit unit)
        {
            Vector2 distance = transform.position - unit.transform.position;
            if(distance.sqrMagnitude >= GameDefine.UNIT_INTERACT_DISTANCE * GameDefine.UNIT_INTERACT_DISTANCE)
                return;

            OnInteract(unit);
        }

        protected void DestroyItem()
        {
            destroyCallback?.Invoke();
        }
    }
}
