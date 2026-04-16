using UnityEngine;

namespace ShootingHero.Shared
{
    public class HealPackItem : ItemBase
    {
        protected override void OnInteract(Unit unit)
        {
            Debug.LogError("HealPack!!");
            DestroyItem();
        }
    }
}