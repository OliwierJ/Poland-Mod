using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Capture;

namespace PolandMod.Content.Biomes
{
    public class PolishBiome : ModBiome
    {
        // Override if you want custom water style
        // public override ModWaterStyle WaterStyle => ModContent.GetInstance<PolishWaterStyle>();

        // Music - you'll need to add music file to your mod
        // public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/PolishBiomeMusic");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => new Color(50, 250, 30); // green

        // Calculate when the biome is active
        public override bool IsBiomeActive(Player player)
        {
            // First, check for the custom tile count condition
			bool b1 = ModContent.GetInstance<MossBlockCount>().mossBlockCount >= 200;
            
            // Finally, we will limit the height at which this biome can be active to above ground (ie sky and surface).
            bool b2 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            
            return b1 && b2;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
    }
}