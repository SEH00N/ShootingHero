using MemoryPack;
using UnityEngine;

namespace ShootingHero.Shared
{
    [MemoryPackable]
    public partial class UnitDataDTO
    {
        public Vector2 Position { get; set; }
        public int Height { get; set; }
        public int CurrentHP { get; set; }
        public int CurrentWeaponID { get; set; }
        public string CurrentWeaponStatus { get; set; }
    }
}