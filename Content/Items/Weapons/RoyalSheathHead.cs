using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

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
            Player player = Main.player[Projectile.owner];
            Texture2D heldTex = ModContent.Request<Texture2D>("PolandMod/Content/Items/Weapons/RoyalSheath_InUse").Value;

            // compute hand position; tweak offsets to fit your sprite
            Vector2 hand = player.RotatedRelativePoint(player.MountedCenter) + new Vector2(12 * player.direction, -8);
            Vector2 origin = heldTex.Size() * 0.5f;
            SpriteEffects effects = player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            float rotation = Projectile.rotation;
            if (player.direction == -1) rotation += MathHelper.Pi;

            Main.EntitySpriteDraw(heldTex, hand - Main.screenPosition, null, lightColor, rotation, origin, 1f, effects, 0);
            return false; // still draw projectile sprite if needed; return false to skip default projectile draw
        }
    }
}

