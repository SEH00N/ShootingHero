using UnityEngine;

namespace ShootingHero.Shared
{
    public class StairCollider : MonoBehaviour
    {
        [SerializeField]
        private int height = 0;

        public void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.TryGetComponent<Unit>(out Unit unit) == false)
                return;
            
            unit.SetHeight(height);
        }
    }
}