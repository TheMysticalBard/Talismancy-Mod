using IL.Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

namespace Talismancy.Items
{
    //This was all done following exampleMod's exampleDamage class for the most part.
	public class TalismanBaseItem : ModItem
	{
        public override bool CloneNewInstances => true;
        public int curseSlots = 1;

        public virtual void SafeSetDefaults()
        {

        }
		public sealed override void SetDefaults() 
		{
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
            //This is because we don't want weird talisman things in hands, just thrown paper.
            item.noUseGraphic = true;
            //We also don't want any damage hitboxes on the animation, just the projectile.
            item.noMelee = true;
            //All talismans are thrown so we want the hand throwing them.
            item.useStyle = ItemUseStyleID.SwingThrow;
		}

        public override void ModifyWeaponDamage(Terraria.Player player, ref float add, ref float mult, ref float flat)
        {
			add += player.GetModPlayer<TalismanPlayer>().curseDamageAdd;
			mult *= player.GetModPlayer<TalismanPlayer>().curseDamageMult;
        }

        public override void GetWeaponKnockback(Terraria.Player player, ref float knockback)
        {
			knockback += player.GetModPlayer<TalismanPlayer>().lesserCurseKB;
        }

        public override void GetWeaponCrit(Terraria.Player player, ref int crit)
        {
            crit += player.GetModPlayer<TalismanPlayer>().lesserCurseCrit;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if(line != null)
            {
                String[] text = line.text.Split(' ');
                String damage = text.First();
                String localText = text.Last();
                line.text = damage + " curse " + localText;
            }

            //Add a tooltip letting the player know how many slots each curse uses.
            if(curseSlots > 1)
            {
                tooltips.Add(new TooltipLine(mod, "Curse Slot Useage", $"Uses {curseSlots} slots per curse"));
            }
        }

        //Every talisman should have an alt fire for bosses.
        public override bool AltFunctionUse(Terraria.Player player)
        {
            return true;
        }
    }
}