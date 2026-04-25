using System;
using UnityEngine;

namespace ShootingHero.Shared
{
    public abstract class ItemBase : MonoBehaviour
    {
        private int itemID = 0;
        private string uuid = string.Empty;
        private int height = 0;

        public int ItemID => itemID;
        public string UUID => uuid;
        public int Height => height;

        private Action destroyCallback = null;

        public void Initialize(int itemID, string uuid, int height, Action destroyCallback = null)
        {
            this.itemID = itemID;
            this.uuid = uuid;
            this.height = height;
            this.destroyCallback = destroyCallback;
        }

        protected abstract void OnInteract(Unit unit);
        public void Interact(Unit unit)
        {
            OnInteract(unit);
        }

        protected void DestroyItem()
        {
            destroyCallback?.Invoke();

            Destroy(gameObject);
            GameManager.Instance.RemoveItem(uuid);
        }
    }
}
