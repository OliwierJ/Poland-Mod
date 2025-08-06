using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{
    public class PolishFlag : ModItem
    {
        public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PolishFlagTile>());
			Item.value = Item.buyPrice(copper: 10);
		}

        // create recipe for the Polish flag
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.RedDye, 1);
            recipe.AddIngredient(ItemID.SilverDye, 1);
            recipe.AddTile(TileID.Loom);
            recipe.Register();

        }
    }

}