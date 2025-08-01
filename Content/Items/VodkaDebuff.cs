using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PolandMod.Content.Items
{
    // This file contains all the code necessary for the icon you can click on to despawn the minion
    // https://github.com/tModLoader/tModLoader/wiki/Basic-Minion-Guide


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
            player.statDefense -= defenseDecrease;
            player.GetDamage(DamageClass.Melee) += attackIncrease;
            player.GetDamage(DamageClass.Magic) += attackIncrease;
            player.GetDamage(DamageClass.Ranged) += attackIncrease;
            player.GetDamage(DamageClass.Summon) += attackIncrease;
            player.GetDamage(DamageClass.Throwing) += attackIncrease;

        }
    }
}