using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Tiles
{
	public class MossBlock : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;

			DustType = DustID.Grass; // Snap Thorn
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		// public override void NearbyEffects(int i, int j, bool closer) {
		// 	Player player = Main.LocalPlayer;
			
		// 	// Check if the player is standing on the block
		// 	if (closer && player.velocity.Y == 0f && player.oldVelocity.Y >= 0f) {
		// 		// Get the position of the player's feet
		// 		Vector2 dustPosition = player.Bottom;
				
		// 		// Create dust at the player's feet
		// 		if (Main.rand.NextBool(3)) { // Adjust the frequency of dust spawning
		// 			Dust.NewDust(dustPosition, 8, 8, DustType, player.velocity.X * 0.2f, 0f);
		// 		}
		// 	}
		// }

		// public override void ChangeWaterfallStyle(ref int style) {
		// 	style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
		// }
	}
}