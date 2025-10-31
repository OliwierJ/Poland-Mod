using Terraria.ModLoader;
using System;

namespace PolandMod.Content.Biomes
{
    public class MossBlockCount : ModSystem
    {
        public int mossBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            // This will hold the count of your custom tiles
            mossBlockCount = tileCounts[ModContent.TileType<Tiles.MossBlock>()];
        }
    }
}