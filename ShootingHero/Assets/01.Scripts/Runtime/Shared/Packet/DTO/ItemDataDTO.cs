using MemoryPack;
using UnityEngine;

namespace ShootingHero.Shared
{
    [MemoryPackable]
    public partial class ItemDataDTO
    {
        public int ItemID { get; set; }
        public Vector2 Position { get; set; }
    }
}