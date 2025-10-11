using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PolandMod.Content.Items.Weapons
{
    public class RoyalSheathHeadDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            // Only visible if the player is holding the RoyalSheath and the projectiles exist
            return drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<RoyalSheath>() &&
                   ProjectileExists(drawInfo.drawPlayer);
        }

        private bool ProjectileExists(Player player)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<RoyalSheathHead>())
                    return true;
            }
            return false;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<RoyalSheathHead>())
                {
                    Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[proj.type].Value;
                    Vector2 position = proj.Center - Main.screenPosition;
                    SpriteEffects effects = proj.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    drawInfo.DrawDataCache.Add(new DrawData(
                        texture,
                        position,
                        null,
                        Lighting.GetColor((int)proj.Center.X / 16, (int)proj.Center.Y / 16),
                        proj.rotation,
                        texture.Size() / 2f,
                        proj.scale,
                        effects,
                        0
                    ));
                }
            }
        }
    }
}