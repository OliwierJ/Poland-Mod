using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Bosses
{
    // This is the javelin projectile used by the Eagle Boss
    // It has custom gravity and dust effects
    public class BossJavelin : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 62;
            Projectile.hostile = true;      // Makes it damage players
            Projectile.friendly = false;    // Doesn't damage NPCs
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            Projectile.aiStyle = 507;       // Using the aiStyle of the vanilla javelin for basic behavior
        }

        public override void AI()
        {
            // Apply gravity after 2 seconds
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 60f)
            {
                Projectile.ai[0] = 60f;
                Projectile.velocity.Y += 0.3f;
            }

            // The projectile is rotated to face the direction of travel
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // // Cap downward velocity
            if (Projectile.velocity.Y > 18f)
            {
                Projectile.velocity.Y = 18f;
            }

            // Some visuals here
            Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * 0.5f);
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Gold);
            dust.noGravity = true;
            dust.velocity *= 1.1f;
            dust.scale *= 1f;
        }

        // When the projectile hits something, it creates a sound and dust effects
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
            for (int i = 0; i < 6; i++) // Creates a splash of dust around the position the projectile dies.
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Gold);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 1.4f;
            }
        }
    }
}