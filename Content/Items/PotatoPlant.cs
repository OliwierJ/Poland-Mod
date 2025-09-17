using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PolandMod.Content.Items
{
    // An enum for the 3 stages of plant growth
    public enum GrowthStage : byte
    {
        Planted,
        Growing,
        Grown
    }


    public class PotatoPlant : ModTile
    {

        private const int FrameWidth = 18; // A constant for readability and to kick out those magic numbers

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]); // Make this tile interact with golf balls in the same way other plants do

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(203, 155, 46), name);

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.AnchorValidTiles = [
                TileID.Grass,
                TileID.HallowedGrass,
            ];
            TileObjectData.newTile.AnchorAlternateTiles = [
                TileID.ClayPot,
                TileID.PlanterBox
            ];
            TileObjectData.addTile(Type);

            HitSound = SoundID.Grass;
            DustType = DustID.Ambient_DarkBrown;
        }

        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile.HasTile)
            {
                int tileType = tile.TileType;
                if (tileType == Type)
                {
                    GrowthStage stage = GetStage(i, j);

                    return stage == GrowthStage.Grown;
                }
                else
                {
                    // Support for vanilla herbs/grasses:
                    if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
                    {
                        bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
                        bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);
                        bool harvestableVanillaHerb = Main.tileAlch[tileType] && WorldGen.IsHarvestableHerbWithSeed(tileType, tile.TileFrameX / 18);

                        if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
                        {
                            WorldGen.KillTile(i, j);
                            if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                            }

                            return true;
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -2; // This is -1 for tiles using StyleAlch, but vanilla sets to -2 for herbs, which causes a slight visual offset between the placement preview and the placed tile. 
        }

        public override bool CanDrop(int i, int j)
        {
            GrowthStage stage = GetStage(i, j);

            if (stage == GrowthStage.Planted)
            {
                // Do not drop anything when just planted
                return false;
            }
            return true;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            GrowthStage stage = GetStage(i, j);

            Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();
            Player nearestPlayer = Main.player[Player.FindClosest(worldPosition, 16, 16)];

            int herbItemType = ModContent.ItemType<Potato>();
            int herbItemStack = 1;

            if (nearestPlayer.active && (nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth || nearestPlayer.HeldItem.type == ItemID.AcornAxe))
            {
                // Increased yields with Staff of Regrowth, even when not fully grown
                herbItemStack = Main.rand.Next(4, 7);

            }
            else if (stage == GrowthStage.Grown)
            {
                // Default yields, only when fully grown
                herbItemStack = Main.rand.Next(2, 4);
            }

            if (herbItemType > 0 && herbItemStack > 0)
            {
                yield return new Item(herbItemType, herbItemStack);
            }

        }

        public override bool IsTileSpelunkable(int i, int j)
        {
            GrowthStage stage = GetStage(i, j);
            return stage == GrowthStage.Grown;
        }

       public override void RandomUpdate(int i, int j) {
			Tile tile = Framing.GetTileSafely(i, j);
			GrowthStage stage = GetStage(i, j);

			// Only grow to the next stage if there is a next stage. We don't want our tile turning pink!
			if (stage != GrowthStage.Grown) {
				// Increase the x frame to change the stage
				tile.TileFrameX += FrameWidth;

				// If in multiplayer, sync the frame change
				if (Main.netMode != NetmodeID.SinglePlayer) {
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
		}

		// A helper method to quickly get the current stage of the herb (assuming the tile at the coordinates is our herb)
		private static GrowthStage GetStage(int i, int j) {
			Tile tile = Framing.GetTileSafely(i, j);
			return (GrowthStage)(tile.TileFrameX / FrameWidth);
		}







    }

}
