using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{

    public class Potato : ModItem
    {

        public override void SetStaticDefaults() {
			ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true; // This prevents this item from being automatically dropped from potato tile. 
			Item.ResearchUnlockCount = 25;
		}

        public override void SetDefaults()
        {


            Item.DefaultToPlaceableTile(ModContent.TileType<PotatoPlant>());
            Item.width = 12;
            Item.height = 14;
            Item.value = Item.buyPrice(silver: 1);
            Item.shoot = ModContent.ProjectileType<Weapons.PotatoRocket>(); // The projectile that weapons fire when using this item as ammunition.
            Item.shootSpeed = 3f; // The speed of the projectile.
            Item.ammo = Item.type; // Important. The first item in an ammo class sets the AmmoID to its type
            Item.damage = 12; 
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = Item.CommonMaxStack;
			Item.knockBack = 1.5f;

        }

        

        
    }
}

