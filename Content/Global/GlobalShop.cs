using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Global
{
	class GlobalShop : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{
			// allow the Dryad to sell the Potato item
			if (shop.NpcType == NPCID.Dryad)
			{
				// This item sells for the normal price.
				shop.Add<Items.Potato>();

			}
		}
	}
}
