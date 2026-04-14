using System;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class UnitInputComponent : MonoBehaviour
    {
        private Unit unit = null;
        private UnitMovementComponent unitMovementComponent = null;

        private PlayerInputReader playerInputReader = null;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            unitMovementComponent = GetComponent<UnitMovementComponent>();

            playerInputReader = InputManager.GetInput<PlayerInputReader>();
            playerInputReader.OnInteractEvent += HandleInteract;
        }

        private void Update()
        {
            unitMovementComponent.SetMovementInput(playerInputReader.MovementInput);
        }

        private void HandleInteract()
        {
            Collider2D[] detectedItems = Physics2D.OverlapCircleAll(transform.position, GameDefine.UNIT_INTERACT_DISTANCE, (int)GameDefine.ELayerMask.ItemLayer);
            if(detectedItems == null || detectedItems.Length <= 0)
                return;
            
            Array.Sort(detectedItems, (a, b) => {
                float sqrDistanceA = (a.transform.position - transform.position).sqrMagnitude;
                float sqrDistanceB = (b.transform.position - transform.position).sqrMagnitude;
                return sqrDistanceA.CompareTo(sqrDistanceB);
            });

            foreach(Collider2D detectedItem in detectedItems)
            {
                if(detectedItem.TryGetComponent<ItemBase>(out ItemBase item) == false)
                    continue;
                
                item.Interact(unit);
                return;
            }
        }
    }
}