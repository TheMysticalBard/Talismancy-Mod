using Terraria.ModLoader;

namespace Talismancy
{
    class TalismanPlayer : ModPlayer
    {
        public float curseDamageAdd;
        public float curseDamageMult = 1.0f;
        //Left click will be used for clearing non-boss enemies and needs knockback and crit. Lesser curse DoT will not have crit.
        public float lesserCurseKB;
        public int lesserCurseCrit;

        //All increases to slots work like minion slots, there are no permanent ones so a single value is fine.
        public int curseSlotsMax = 3;
        //Some armor sets or accessories will multiply slots. This is to encourage using items that require more slots.
        public int curseSlotMult = 1;
    }
}
