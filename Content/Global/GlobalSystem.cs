using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PolandMod.Content.Global
{
    class GlobalSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            //group for evil boss drop i.e. tissue sample/shadow scale
            RecipeGroup evilBossdropGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowScale)}", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowScale), evilBossdropGroup);

            // group for crowns i.e. gold/platinum crown
            RecipeGroup crownGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldCrown)}", ItemID.GoldCrown, ItemID.PlatinumCrown);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldCrown), crownGroup);
        }
    }
}
