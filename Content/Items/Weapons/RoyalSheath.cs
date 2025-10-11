using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items.Weapons
{

    public class RoyalSheath : ModItem
    {


        public override void SetDefaults()
        {

            // Common Properties
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 4); // The value of the item in copper coins.

            // Use Properties
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            // Weapon Properties
            Item.DamageType = DamageClass.Magic;
            Item.damage = 50;
            Item.knockBack = 6;
            Item.noMelee = true;

            // Wand Properties
            Item.noUseGraphic = true; // <--- added: stop default held graphic so we can draw a different one while shooting
            Item.shoot = ModContent.ProjectileType<RoyalSheathProjectile>();
            Item.shootSpeed = 15;
            Item.mana = 8;

        }
        // Prevent item rotation when held out
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            // Keep the item straight out from the player's hand
            player.itemRotation = 0f;
        }

        public override void UseItemFrame(Player player)
        {
            // Prevent the item from rotating with the cursor
            player.itemRotation = 0f;
        }

        public override Vector2? HoldoutOffset()
        {
            // Move the item 3 pixels forward and 0 pixels upward
            return new Vector2(3, -5);
        }
        // This method allows you to determine where the projectile is spawned
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // position = player.Center + new Vector2(40 * player.direction, -40);
            
            // Spawn the Head of the eagle at the correct position
            var head = Projectile.NewProjectileDirect(
                source,
                position,
                Vector2.Zero,
                ModContent.ProjectileType<RoyalSheathHead>(),
                0,
                0,
                Main.myPlayer
            );
            // shoot towards the cursor from the heads offset position
            Vector2 cursor = Main.MouseWorld;
            head.rotation = (float)Math.Atan2(cursor.Y - head.Center.Y, cursor.X - head.Center.X);
            if (player.direction == -1)
            {
                head.rotation += (float)Math.PI;
            }
            Vector2 offset = new Vector2(head.position.X + (23 * player.direction), head.position.Y - 26);
            var projectile = Projectile.NewProjectileDirect(source, offset, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
            return false;
        }
        
    }
}