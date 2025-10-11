using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace PolandMod.Content.Items.Weapons

{

    public class RoyalSheathHead : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 8; // The width of projectile hitbox
            Projectile.height = 8; // The height of projectile hitbox

            Projectile.timeLeft = 30;
            Projectile.damage = 0;

        }

        // Custom AI
        public override void AI()
        {
            // Keep the projectile fixed relative to the player
            Player owner = Main.player[Projectile.owner];
            Projectile.Center = new Vector2(owner.Center.X + (23 * owner.direction), owner.Center.Y - 29);
            Projectile.spriteDirection = owner.direction;

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];

            // Only draw the held item when the player is actually holding the RoyalSheath
            if (owner.HeldItem == null || owner.HeldItem.type != ModContent.ItemType<RoyalSheath>())
                return false;

            // Request the item's display texture (tweak path if your mod uses a different one)
            Texture2D tex = ModContent.Request<Texture2D>("PolandMod/Content/Items/Weapons/RoyalSheath_InUse").Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(-5 * owner.direction, 25); // Slightly adjust vertical position if needed

            // Origin: center of the texture (adjust if your sprite's origin differs)
            Vector2 origin = tex.Size() * 0.5f;

            // Rotation: use the player's item rotation so it behaves like a normal held sprite
            float rotation = owner.itemRotation;
            SpriteEffects effects = owner.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            // If player's spriteDirection flips the sprite horizontally for left, compensate by flipping vertically here
            // (tweak if your art needs horizontal flip instead)
            if (owner.direction == -1)
                rotation += MathHelper.Pi;

            // Draw with lighting color so it matches world lighting
            Main.EntitySpriteDraw(tex, drawPos, null, lightColor, rotation, origin, 1f, effects, 0);

            // Skip default projectile drawing
            return false;
        }
    }
}

