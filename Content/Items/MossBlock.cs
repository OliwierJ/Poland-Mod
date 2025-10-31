
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{

    public class MossBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.MossBlock>());
			Item.width = 12;
			Item.height = 12;
		}

    }
}

