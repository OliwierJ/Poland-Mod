using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Buffs
{
    // This buff is applied when the player drinks the vodka
    // It increases melee, ranged, magic, summon, and throwing damage by 15% but deceases defense by 7
    public class VodkaDebuff : ModBuff
    {
        private static readonly float attackIncrease = 0.15f;
        private static readonly int defenseDecrease = 7;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false; // This buff will save when you exit the world
            Main.buffNoTimeDisplay[Type] = false; // The time remaining will display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Apply the effects of the buff
            player.statDefense -= defenseDecrease;
            player.GetDamage(DamageClass.Melee) += attackIncrease;
            player.GetDamage(DamageClass.Magic) += attackIncrease;
            player.GetDamage(DamageClass.Ranged) += attackIncrease;
            player.GetDamage(DamageClass.Summon) += attackIncrease;
            player.GetDamage(DamageClass.Throwing) += attackIncrease;

        }
    }
}