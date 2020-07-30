using Talismancy.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace Talismancy.Items
{
    class BasicTalisman : TalismanBaseItem
    {
        public override void SafeSetDefaults()
        {

        }

        public override bool CanUseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                item.shoot = mod.ProjectileType<>();
            }
            else
            {
                item.shoot = mod.ProjectileType<>();
            }
            return base.CanUseItem(player);
        }
    }
}
