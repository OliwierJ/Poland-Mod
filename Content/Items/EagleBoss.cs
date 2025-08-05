using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{
	[AutoloadBossHead]

	public class EagleBoss : ModNPC
	{

		// public override string Texture => "PolandMod/Content/Items/UnfinishedTexture";
		private int attackTimer;
		private int bigAttackTimer = 0;
		private int bigAttackCooldown = 800;
		public ref float AI_State => ref NPC.ai[0];
		public ref float TimerToSwap => ref NPC.localAI[1];
		public int idlePositionSwap = 500;
		public bool isLeft = true;
		private int shootCooldown = 100;
		private int roarCooldown;
		private bool spawnedHoming = false;
		private bool reachedIdle;
		private bool doneRoar;

		private enum ActionState
		{
			Idle,
			MoveToCharge,
			PerformCharge,
			Grabbing,
			Roar
		}
		private enum Frame
		{
			Idle1,
			Idle2,
			Idle3,
			Idle4,
			Charge,
			Roar,
			Grab
			
		}
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 7; // make sure to set this for your modnpcs.

			// Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 150;
			NPC.height = 150;
			NPC.damage = 50;
			NPC.defense = 10;
			NPC.lifeMax = 6000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = Item.buyPrice(gold: 5);
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.npcSlots = 15f; // Take up open spawn slots, preventing random NPCs from spawning during the fight

			// Custom AI, 0 is "bound town NPC" AI which slows the NPC down and changes sprite orientation towards the target
			NPC.aiStyle = -1;

			// Custom boss bar
			// NPC.BossBar = ModContent.GetInstance<MinionBossBossBar>();

			// The following code assigns a music track to the boss in a simple way.
			// if (!Main.dedServ) {
			// 	Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ropocalypse2");
			// }
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{

		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{

		}

		public override void OnKill()
		{
			// This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
			NPC.SetEventFlagCleared(ref DownedBossSystem.downedEagleBoss, -1);
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server) {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0) {
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				// int backGoreType = Mod.Find<ModGore>("MinionBossBody_Back").Type;
				// int frontGoreType = Mod.Find<ModGore>("MinionBossBody_Front").Type;

				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 2; i++) {
					// Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
					// Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
				}

				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				// This adds a screen shake (screenshake) similar to Deerclops
				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}

		public override void FindFrame(int frameHeight)
		{

			Player player = Main.player[NPC.target];
			NPC.spriteDirection = (player.Center.X > NPC.Center.X) ? -1 : 1;

			// For the most part, our animation matches up with our states.
			switch (AI_State)
			{
				case (float)ActionState.Idle:
					// npc.frame.Y is the goto way of changing animation frames. npc.frame starts from the top left corner in pixel coordinates, so keep that in mind.
					// Here we have 4 frames that we want to cycle through.
					NPC.frameCounter++;

					if (NPC.frameCounter < 10) {
						NPC.frame.Y = (int)Frame.Idle1 * frameHeight;
					} else if (NPC.frameCounter < 20)
					{
						NPC.frame.Y = (int)Frame.Idle2 * frameHeight;
					} else if (NPC.frameCounter < 30)
					{
						NPC.frame.Y = (int)Frame.Idle3 * frameHeight;
					} else if (NPC.frameCounter < 40)
					{
						NPC.frame.Y = (int)Frame.Idle4 * frameHeight;
					} else {
						NPC.frameCounter = 0;
					}

					break;
				case (float)ActionState.PerformCharge:
					NPC.frame.Y = (int)Frame.Charge * frameHeight;
					break;
				case (float)ActionState.Roar:
					// Going from Notice to Asleep makes our npc look like it's crouching to jump.
					NPC.frame.Y = (int)Frame.Roar * frameHeight;
					break;
				case (float)ActionState.Grabbing:
					NPC.frame.Y = (int)Frame.Grab * frameHeight;
					break;

			}

		}
		public override void AI()
		{
			// This should almost always be the first code in AI() as it is responsible for finding the proper player target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}

			Player player = Main.player[NPC.target];

			if (player.dead)
			{
				// If the targeted player is dead, flee
				NPC.velocity.Y -= 0.04f;
				// This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
				NPC.EncourageDespawn(10);
				return;
			}

			ScaleWithHP();
			// reset the boss damage
			NPC.damage = 50;
			switch (AI_State)
			{
				case (float)ActionState.Idle:
					FindPosition(player);
					ShootProjectile(player);
					break;
				case (float)ActionState.MoveToCharge:
					MoveToChargeAttack(player);
					break;
				case (float)ActionState.PerformCharge:
					ChargeAttack(player);
					break;
				case (float)ActionState.Grabbing:
					GrabAttack(player);
					break;
				case (float)ActionState.Roar:
					RoarAttack(player);
					break;
			}

		}

        private void ScaleWithHP()
        {
			if (NPC.life < NPC.lifeMax / 2)
			{
				shootCooldown = 70;
				idlePositionSwap = 450;
				bigAttackCooldown = 700;
				
			}
			if (NPC.life < NPC.lifeMax / 3)
			{
				shootCooldown = 48;
				idlePositionSwap = 350;
				bigAttackCooldown = 600;
			}
        }

        // TODO spritework
        // // TODO boss loot
        // TODO beastiary
        // TODO icons

        // perform roar attack that spawns homing projectiles
        private void RoarAttack(Player player)
		{
			NPC.velocity = Vector2.Zero;
			roarCooldown++;
			if (!doneRoar)
			{
				SoundEngine.PlaySound(SoundID.Roar, NPC.position);
				doneRoar = true;
			}
			NPC.spriteDirection = -1;
			SpawnHoming(player);

			if (roarCooldown > 200)
			{
				roarCooldown = 0;
				spawnedHoming = false;
				doneRoar = false;
				AI_State = (float)ActionState.Idle;
			}
		}

		private void SpawnHoming(Player player)
		{
			if (spawnedHoming) { return; }

			spawnedHoming = true;
			// spawn extra daggers in expert
			int maxProjectilesSpawned = Main.expertMode ? 8 : 6;
			float speed = 5f;

			for (int i = 0; i < maxProjectilesSpawned; i++)
			{

				// calculate the angle for each projectile
				float angle = MathHelper.TwoPi / maxProjectilesSpawned * i;
				Vector2 direction = new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);

				// Spawn the projectile
				if (Main.netMode != NetmodeID.MultiplayerClient) // Only spawn on server or singleplayer
				{
					Projectile.NewProjectile(
						NPC.GetSource_FromAI(),
						NPC.Center,
						direction,
						ModContent.ProjectileType<BossHomingDagger>(), // Replace with your projectile type
						40, // damage
						2f, // knockback
						Main.myPlayer,
						ai0: player.whoAmI // Pass the player's ID in ai[0]
					);

				}
			}
		}

		// perform a grab attack in an arc
		private void GrabAttack(Player player)
		{
			NPC.damage *= 2;
			int maxChargeDistance = 50;


			// Default movement parameters (here for attacking)
			float speed = 45f;
			float inertia = 90f;
			Vector2 toPlayer = player.Center - NPC.Center;
			toPlayer.Normalize();
			toPlayer *= speed;
			NPC.velocity = (NPC.velocity * (inertia - 1) + toPlayer) / inertia;

			// Use direction multiplier: 1 for right, -1 for left
			int direction = isLeft ? 1 : -1;

			// Check if boss has passed the target distance
			float targetX = player.Center.X + maxChargeDistance * direction;
			bool passedTarget = isLeft ? NPC.Center.X > targetX : NPC.Center.X < targetX;

			if (passedTarget)
			{
				AI_State = (float)ActionState.Idle;
			}
		}

		// perform the charge attack
		private void ChargeAttack(Player player)
		{
			NPC.damage *= 2;
			int maxChargeDistance = 600;
			NPC.velocity.Y = 0f;

			// Use direction multiplier: 1 for right, -1 for left
			int direction = isLeft ? 1 : -1;
			NPC.velocity.X = 20f * direction;

			// Check if boss has passed the target distance
			float targetX = player.Center.X + maxChargeDistance * direction;
			bool passedTarget = isLeft ? NPC.Center.X > targetX : NPC.Center.X < targetX;

			if (passedTarget)
			{
				AI_State = (float)ActionState.Idle;
			}
		}

		// move into position for a grab attack
		private void MoveToChargeAttack(Player player)
		{
			// the direction to the where the boss should go
			Vector2 vectorToIdlePosition;
			float distanceToIdlePosition;
			float prefferedDistance = 600;

			// where the boss should go 
			Vector2 idlePosition;
			idlePosition.Y = player.Center.Y;
			idlePosition.X = 0;

			// check the direction
			if (isLeft)
			{
				idlePosition.X = player.Center.X - prefferedDistance;
			}
			else
			{
				idlePosition.X = player.Center.X + prefferedDistance;
			}

			vectorToIdlePosition = idlePosition - NPC.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();


			// Default movement parameters (here for attacking)
			float speed = 20f;
			float inertia = 10f;

			if (distanceToIdlePosition > 35f)
			{
				// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
				vectorToIdlePosition.Normalize();
				vectorToIdlePosition *= speed;
				NPC.velocity = (NPC.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
			}
			else
			{
				AI_State = (float)ActionState.PerformCharge;
				NPC.velocity = Vector2.Zero;
			}
		}

		private void ShootProjectile(Player player)
		{
			// In your AI() method, after FindPosition(player);
			attackTimer++;
			
			if (attackTimer >= (shootCooldown)) // Every 2 seconds (60 ticks = 1 second)
			{
				attackTimer = 0;

				float projectileSpeed = 12f;

				// Calculate direction to player
				Vector2 toPlayer = player.Center - NPC.Center;
				float distance = toPlayer.Length();
				float timeToReach = distance / projectileSpeed;

				Vector2 predictiedPosition = player.Center + player.velocity * timeToReach;
				Vector2 direction = predictiedPosition - NPC.Center;
				direction.Normalize();
				direction *= projectileSpeed;
				// Spawn the projectile
				if (Main.netMode != NetmodeID.MultiplayerClient) // Only spawn on server or singleplayer
				{
					Projectile.NewProjectile(
						NPC.GetSource_FromAI(),
						NPC.Center,
						direction,
						ProjectileID.JavelinHostile, // Replace with your projectile type
						20, // damage
						2f, // knockback
						Main.myPlayer
					);
				}
			}
		}

		private void FindPosition(Player player)
		{

			TimerToSwap++;
			if (TimerToSwap > idlePositionSwap)
			{
				TimerToSwap = 0;
				isLeft = !isLeft;
				reachedIdle = false;
			}

			// the direction to the where the boss should go
			Vector2 vectorToIdlePosition;
			float distanceToIdlePosition;
			float prefferedDistanceY = 300;
			float prefferedDistanceX = 400;

			// where the boss should go 
			Vector2 idlePosition;
			idlePosition.Y = player.Center.Y - prefferedDistanceY;
			idlePosition.X = 0;

			if (isLeft)
			{
				// NPC.alpha = 0;
				idlePosition.X = player.Center.X - prefferedDistanceX;
			}
			else
			{

				idlePosition.X = player.Center.X + prefferedDistanceX;

			}

			vectorToIdlePosition = idlePosition - NPC.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();


			// Default movement parameters (here for attacking)
			float speed;
			float inertia;

			if (distanceToIdlePosition > 400f)
			{
				// Speed up the minion if it's away from the player
				speed = 20f;
				inertia = 60f;
			}
			else
			{
				// Slow down the minion if closer to the player
				speed = 6f;
				inertia = 40f;
			}

			if (distanceToIdlePosition > 20f)
			{
				// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
				vectorToIdlePosition.Normalize();
				vectorToIdlePosition *= speed;
				NPC.velocity = (NPC.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				reachedIdle = true;
			}


			// cause boss to perform a big attack
			bigAttackTimer++;
			if (bigAttackTimer > bigAttackCooldown && reachedIdle)
			{
				reachedIdle = false;
				bigAttackTimer = 0; // reset timer
									// generate random number for attack
				int randomValue = Main.rand.Next(1, 4);
				if (randomValue == 1) { AI_State = (float)ActionState.MoveToCharge; }
				if (randomValue == 2) { AI_State = (float)ActionState.Grabbing; }
				if (randomValue == 3) { AI_State = (float)ActionState.Roar; }
			}

		}

	
	}
}