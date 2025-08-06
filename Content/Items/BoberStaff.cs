using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{
    // This file contains all the code necessary for the icon you can click on to despawn the minion
    // https://github.com/tModLoader/tModLoader/wiki/Basic-Minion-Guide


    public class BoberStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }

        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.knockBack = 4f;
            Item.mana = 12; // mana cost
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 24;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;       // swing style when in use
            Item.value = Item.sellPrice(gold: 30);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item44; // basic minion summon sound ID

            // Below needed for minion weapon
            Item.noMelee = true; // no melee damage
            Item.DamageType = DamageClass.Summon; // register as summon damage
            Item.buffType = ModContent.BuffType<Buffs.BoberBuff>();
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.shoot = ModContent.ProjectileType<BoberMinion>();

        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Spawn the minion at the cursor
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BeetleHusk, 15);
            recipe.AddIngredient(ItemID.Acorn, 5);
            recipe.AddIngredient(ItemID.Wood, 50);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }


    }
}