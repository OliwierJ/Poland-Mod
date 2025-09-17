using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Bosses
{
	// This tells tModLoader to look for a texture called EagleBossMask_Head, which is the texture on the player
	// and then registers this item to be accepted in head equip slots
	[AutoloadEquip(EquipType.Head)]
	public class EagleBossMask : ModItem
	{
		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 28;

			// Common values for every boss mask
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
			Item.maxStack = 1;
		}
	}
}
