
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items.Weapons

{

	public class PotatoRocket : ModProjectile
	{

		public override string Texture => "PolandMod/Content/Items/Potato";
		public override void SetDefaults()
		{
			Projectile.width = 20; // The width of projectile hitbox
			Projectile.height = 20; // The height of projectile hitbox

			Projectile.arrow = true;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.coldDamage = true;
			Projectile.penetrate = 2;


		}

		public override void AI()
		{
			// Apply gravity after a quarter of a second
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 15f)
			{
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.1f;
			}

			// The projectile is rotated to face the direction of travel
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Cap downward velocity
			if (Projectile.velocity.Y > 18f)
			{
				Projectile.velocity.Y = 18f;
			}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.78f);
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
			dust.noGravity = true;
			dust.scale *= 1.5f;
			dust.velocity *= 1.5f;
			dust.scale *= 0.9f;
		}

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

