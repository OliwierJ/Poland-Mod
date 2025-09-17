using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items.Weapons

{

    public class PotatoCannon : ModItem
    {

        // Set defaults for weapon
        public override void SetDefaults()
        {
            // Common Properties
            Item.width = 50;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 6);

            // Weapon Properties
            Item.damage = 36;
            Item.knockBack = 6;

            // Make it a ranged weapon
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PotatoRocket>();
            Item.shootSpeed = 14;
            Item.useAmmo = ModContent.ItemType<Potato>();   // use potatoes as ammo
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item11;  // snowcannon sound

        }

        
        // Create Recipe 
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SnowballCannon)
                .AddIngredient<Potato>(15)
                .AddRecipeGroup(nameof(ItemID.ShadowScale), 10) // either evil boss drop
                .AddTile(TileID.Anvils)                         // craft at anvil
                .Register();
        }

        // change where the gun is held
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, -3f);
        }

        // Randomise the arc of the projectile
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
        }
	}
}
