using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace PolandMod.Content.Global
{
    public class ModIntergrationSystem : ModSystem
    {
        public override void PostSetupContent() {

			// Boss Checklist shows comprehensive information about bosses in its own UI
			DoBossChecklistIntegration();

		}

        private void DoBossChecklistIntegration()
        {
            // If we navigate the wiki, we can find the "LogBoss" method, which we want in this case
            // A feature of the call is that it will create an entry in the localization file of the specified NPC type for its spawn info, so make sure to visit the localization file after your mod runs once to edit it
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }

            // For some messages, mods might not have them at release, so we need to verify when the last iteration of the method variation was first added to the mod, in this case 1.6
            // Usually mods either provide that information themselves in some way, or it's found on the GitHub through commit history/blame
            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            // The "LogBoss" method requires many parameters, defined separately below:

            // Your entry key can be used by other developers to submit mod-collaborative data to your entry. It should not be changed once defined
            string internalName = "PolandEagleBoss";

            // Value inferred from boss progression, see the wiki for details
            float weight = 6f;

            // Used for tracking checklist progress
            Func<bool> downed = () => DownedBossSystem.downedEagleBoss;

            // The NPC type of the boss
            int bossType = ModContent.NPCType<Bosses.EagleBoss>();

            // The item used to summon the boss with (if available)
            int spawnItem = ModContent.ItemType<Bosses.FeatheryCrown>();

            // "collectibles" like relic, trophy, mask, pet
            List<int> collectibles = new List<int>()
            {
                // TODO ---------------------------------------------------------------

				// ModContent.ItemType<Items.Potato>(),
                // ModContent.ItemType<Content.Pets.MinionBossPet.MinionBossPetItem>(),
                ModContent.ItemType<Bosses.EagleBossTrophy>(),
                ModContent.ItemType<Bosses.EagleBossMask>()
            };

            // By default, it draws the first frame of the boss
            // If you want a custom portrait, you can provide a texture and a source rectangle for it
            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = spawnItem,
                    ["collectibles"] = collectibles
                }
            );


        }
    }
}