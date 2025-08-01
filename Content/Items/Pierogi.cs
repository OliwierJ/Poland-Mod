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


            // This allows you to change the color of the crumbs that are created when you eat.
            // The numbers are RGB (Red, Green, and Blue) values which range from 0 to 255.
            // Most foods have 3 crumb colors, but you can use more or less if you desire.
            // Depending on if you are making solid or liquid food switch out FoodParticleColors
            // with DrinkParticleColors. The difference is that food particles fly outwards
            // whereas drink particles fall straight down and are slightly transparent
            
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        
    }
}

