using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PolandMod.Content.Items
{
    // Acts as a container for "downed boss" flags.
    // Set a flag like this in your bosses OnKill hook:
    //    NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);
    public class DownedBossSystem : ModSystem
    {
        public static bool downedEagleBoss = false;

        public override void ClearWorld()
        {
            downedEagleBoss = false;
        }

        // We save our data sets using TagCompounds.
        // NOTE: The tag instance provided here is always empty by default.
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedEagleBoss)
            {
                tag["downedEagleBoss"] = true;
            }

        }
        public override void LoadWorldData(TagCompound tag)
        {
            downedEagleBoss = tag.ContainsKey("downedEagleBoss");

        }
        
        public override void NetSend(BinaryWriter writer) {
			// Order of parameters is important and has to match that of NetReceive
			writer.WriteFlags(downedEagleBoss);
			// WriteFlags supports up to 8 entries, if you have more than 8 flags to sync, call WriteFlags again.

			// If you need to send a large number of flags, such as a flag per item type or something similar, BitArray can be used to efficiently send them. See Utils.SendBitArray documentation.
		}

		public override void NetReceive(BinaryReader reader) {
			// Order of parameters is important and has to match that of NetSend
			reader.ReadFlags(out downedEagleBoss);
			// ReadFlags supports up to 8 entries, if you have more than 8 flags to sync, call ReadFlags again.
		}
    }
}