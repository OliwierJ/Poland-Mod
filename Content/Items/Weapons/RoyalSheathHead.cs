using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
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
            // We don't want the default drawing to occur, so return false.
            return false;
        }
    }
}

