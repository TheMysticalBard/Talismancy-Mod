using Microsoft.Xna.Framework;
using Talismancy.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace Talismancy.Items
{
    class BasicTalisman : TalismanBaseItem
    {
        public override void SafeSetDefaults()
        {
            item.damage = 999;
            item.crit = 15;
            item.useTime = 20;
            item.useAnimation = 20;
            item.shootSpeed = 12f;
        }

        public override bool CanUseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                item.shoot = ModContent.ProjectileType<BasicTalismanAltProj>();
            }
            else
            {
                item.shoot = ModContent.ProjectileType<BasicTalismanProj>();
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //This next bit is to prevent the player from spawning too many traps and to make sure they can activate traps, so we only want to active it if the projectile being shot is the trap.
            if(type == ModContent.ProjectileType<BasicTalismanAltProj>())
            {
                //It's bad practice to use foreach since Main.projectile contains a dummy 1001st slot that should never be iterated over, according to tModLoader developers!
                /*foreach (Projectile proj in Main.projectile)
                {
                    if(proj.type == type)
                    {

                    }
                }*/

                //Search all projectiles for the alternate, if it exists and is NOT a trap, cancel new projectile and turn old one into trap.
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].type == type)
                    {
                        BasicTalismanAltProj customProj = Main.projectile[i].modProjectile as BasicTalismanAltProj;

                        if (customProj != null)
                        {
                            if (!customProj.activeTrap && Main.projectile[i].active)
                            {
                                customProj.activeTrap = true;
                                return false;
                            }
                        }
                    }
                }
                //This is an easy way to handle the not being able to shoot more than the maximum number of traps.
                if (player.ownedProjectileCounts[ModContent.ProjectileType<BasicTalismanAltProj>()] >= player.GetModPlayer<TalismanPlayer>().maxTraps)
                    return false;
            }
            return true;
        }
    }
}
