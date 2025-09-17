using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Bosses
{
    // This is a homing dagger projectile used by the Eagle Boss
    // It homes in on the player after a short delay and has a trailing effect
	public class BossHomingDagger : ModProjectile
	{
        
		public override void SetDefaults()
        {
            Projectile.width = 20; // The width of projectile hitbox
            Projectile.height = 40; // The height of projectile hitbox

            Projectile.friendly = false; 
            Projectile.hostile = true;  // It can damage the player
            Projectile.timeLeft = 200;
            Projectile.penetrate = 2;   // It can hit enemies twice before being destroyed
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // Number of trail positions to keep
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // 0 = normal trail, 2 = afterimage
        }

        // AI method is called every tick to update the projectile's behavior
        public override void AI()
        {
            // The target player is determined by the first AI parameter
            int targetPlayer = (int)Projectile.ai[0];
            if (targetPlayer < 0 || targetPlayer >= Main.maxPlayers || !Main.player[targetPlayer].active)
                return; // Invalid target, do nothing

            // Get the target player
            Player player = Main.player[targetPlayer];

            // Apply homing after 1.5 seconds
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 40f && Projectile.ai[1] <= 145)
            {
                // Default movement parameters (here for attacking)
                float speed = 50;
                float inertia = 80f;
                // Calculate the direction to the player
                Vector2 toPlayer = player.Center - Projectile.Center;
                toPlayer.Normalize();
                toPlayer *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + toPlayer) / inertia;

            }

            // The projectile is rotated to face the direction of travel
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            // Update oldRot for the trail
            for (int i = Projectile.oldRot.Length - 1; i > 0; i--)
            {
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
            }
            Projectile.oldRot[0] = Projectile.rotation;

            // Some visuals here
            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.78f);

        }

        // PreDraw is called before the projectile is drawn
        // We use it to draw the trail with rotation
        public override bool PreDraw(ref Color lightColor)
        {
            // Get the texture
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            // Draw each trail segment with its stored rotation
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + origin - Main.screenPosition;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                float rotation = Projectile.oldRot[i]; // Use the stored rotation for each segment
                // Draw the trail segment
                Main.spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            // Draw the projectile itself
            return true;
        }
        // OnKill is called when the projectile is destroyed
		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
            for (int i = 0; i < 5; i++) // Creates a splash of dust around the position the projectile dies.
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 0.9f;
            }
        }
	

        

        
    }
}

