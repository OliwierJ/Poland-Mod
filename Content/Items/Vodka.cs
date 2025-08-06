using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{

    public class Vodka : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(240, 240, 240),
                new Color(200, 200, 200),
                new Color(140, 140, 140)
            ];
        }

        public override void SetDefaults()
        {
            Item.useTime = 24;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.width = 24;
            Item.height = 24;
            Item.UseSound = SoundID.Item3;
            Item.useAnimation = 40;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<Buffs.VodkaDebuff>();
            Item.value = Item.buyPrice(gold: 1);
            Item.buffTime = 60 * 60 * 10;
        }

        // create recipe for the vodka 
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Potato>(10)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddTile(TileID.Kegs)
                .Register();

        }
        
    }
}