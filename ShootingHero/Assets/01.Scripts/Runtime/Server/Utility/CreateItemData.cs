using ShootingHero.Shared;

namespace ShootingHero.Servers
{
    public struct CreateItemData
    {
        public ItemDataDTO itemData;

        public CreateItemData(ItemBase item)
        {
            itemData = new ItemDataDTO() {
                ItemID = item.ItemID, 
                Position = item.transform.position,
                Height = item.Height
            };
        }
    }
}