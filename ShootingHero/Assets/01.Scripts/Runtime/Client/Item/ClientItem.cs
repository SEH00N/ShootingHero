using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class ClientItem : ItemBase
    {
        protected override void OnInteract(Unit unit)
        {
            Debug.Log($"Interact with {unit.gameObject.name}!!");
        }
    }
}