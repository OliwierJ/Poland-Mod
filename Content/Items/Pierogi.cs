using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{

    public class Pierogi : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsFood[Type] = true; // This allows it to be placed on a plate and held correctly
            // These colors will appear when the food is eaten            
            ItemID.Sets.FoodParticleColors[Item.type] = [
                new Color(175, 114, 0),
                new Color(237, 211, 94),
                new Color(219, 192, 74)
            ];
        }
        public override void SetDefaults()
        {
            // DefaultToFood sets all of the food related item defaults such as the buff type, buff duration, use sound, and animation time.
			Item.DefaultToFood(22, 22, BuffID.WellFed3, 30 * 1800); // 57600 is 16 minutes: 16 * 60 * 60
			Item.value = Item.buyPrice(0, 3);
			Item.rare = ItemRarityID.Red;

        }

        public override void AddRecipes()
        {
            // temp
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Potato>(), 4);
            recipe.AddTile(TileID.Campfire);
            recipe.Register();
        }

        
    }
}

