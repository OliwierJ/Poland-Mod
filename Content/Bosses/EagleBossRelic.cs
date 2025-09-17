using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Bosses
{
	public class EagleBossRelic : ModItem
	{
		public override void SetDefaults() {
			// This method sets the item to be placeable as a tile, and specifies which tile it should place
			Item.DefaultToPlaceableTile(ModContent.TileType<EagleBossRelicTile>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}
}
