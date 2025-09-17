using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Bosses
{
	// Basic code for a boss treasure bag
	public class EagleBossBag : ModItem
	{
		public override void SetStaticDefaults() {
			// It will create a glowing effect around the item when dropped in the world.
			// It will also let our boss bag drop dev armor..
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; 

			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true; // This makes sure that "Expert" displays in the tooltip and the item name color changes
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			// We have to replicate the expert drops from MinionBossBody here

			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<EagleBossMask>(), 7));
			// itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExampleItem>(), 1, 12, 16));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<EagleBoss>()));
		}
	}
}
