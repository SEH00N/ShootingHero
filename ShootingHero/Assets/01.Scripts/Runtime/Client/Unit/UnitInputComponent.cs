using System;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class UnitInputComponent : MonoBehaviour
    {
        private Unit unit = null;
        private PlayerInputReader playerInputReader = null;

        private Vector2 lastMoveInput = Vector2.zero;

        private void Awake()
        {
            unit = GetComponent<Unit>();

            playerInputReader = InputManager.GetInput<PlayerInputReader>();
            playerInputReader.OnInteractEvent += HandleInteract;
            playerInputReader.OnFireEvent += HandleFire;
            playerInputReader.OnReloadEvent += HandleReload;
        }

        private void Update()
        {
            if(lastMoveInput != playerInputReader.MovementInput)
            {
                lastMoveInput = playerInputReader.MovementInput;
                GameManager.Instance.Session.SendAsync(new C2S_MoveInputPacket() { MoveInput = lastMoveInput });
            }
        }

        private void HandleFire()
        {
            WeaponBase weapon = unit.UnitWeaponComponent.Weapon;
            if(weapon == null)
                return;
            
            Vector2 aim = playerInputReader.AimPosition;
            Vector3 aimWorldPosition = Camera.main.ScreenToWorldPoint(aim);
            Vector2 direction = (Vector2)(aimWorldPosition - weapon.transform.position);
            GameManager.Instance.Session.SendAsync(new C2S_FireWeaponPacket() { Direction = direction.normalized });
        }

        private void HandleInteract()
        {
            Collider2D[] detectedItems = Physics2D.OverlapCircleAll(transform.position, GameDefine.UNIT_INTERACT_DISTANCE, (int)GameDefine.ELayerMask.ItemLayer);
            if(detectedItems == null || detectedItems.Length <= 0)
                return;
            
            Array.Sort(detectedItems, (a, b) => {
                float sqrDistanceA = ((Vector2)(a.transform.position - transform.position)).sqrMagnitude;
                float sqrDistanceB = ((Vector2)(b.transform.position - transform.position)).sqrMagnitude;
                return sqrDistanceA.CompareTo(sqrDistanceB);
            });

            foreach(Collider2D detectedItem in detectedItems)
            {
                if(detectedItem.TryGetComponent<ItemBase>(out ItemBase item) == false)
                    continue;
                
                GameManager.Instance.Session.SendAsync(new C2S_InteractItemPacket() { ItemUUID = item.UUID });
                return;
            }
        }

        private void HandleReload()
        {
            WeaponBase weapon = unit.UnitWeaponComponent.Weapon;
            if(weapon == null)
                return;
            
            if(weapon.IsReloading == true)
                return;
            
            GameManager.Instance.Session.SendAsync(new C2S_ReloadWeaponPacket() { });
        }
    }
}