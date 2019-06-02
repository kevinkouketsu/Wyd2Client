using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.Utility
{
    public static class W2Objects
    {
        public static Dictionary<int, MItemList> ItemList { get; set; }

        public static int GetMaxAbility(MMobCore mob, int eff)
        {
            int maxAbility = 0;
            for (int i = 0; i < GameBasics.MAXL_EQUIP; i++)
            {
                if (mob.Equip.Items[i].Index == 0)
                    continue;

                short itemAbility = GetItemAbility(mob.Equip.Items[i], eff);
                if (maxAbility < itemAbility)
                    maxAbility = itemAbility;
            }

            return maxAbility;
        }

        public static short GetMobAbility(MMobCore mob, int eff)
        {
            short total = 0;

            if (eff == MItemDefinition.RANGE)
            {
                total = (short)GetMaxAbility(mob, eff);

                int face = mob.Equip.Items[0].Index / 10;
                if (total < 2 && face == 3)
                    if ((mob.LearnedSkill & 0x040) != 0)
                        total = 2;

                return total;
            }

            int[] array = new int[GameBasics.MAXL_EQUIP];
            for (int i = 0; i < GameBasics.MAXL_EQUIP; i++)
            {
                array[i] = 0;

                int itemId = mob.Equip.Items[i].Index;
                if (itemId == 0 && i != 7)
                    continue;

                if (i >= 1 && i <= 5)
                    array[i] = ItemList[itemId].Unique;

                if (eff == MItemDefinition.DAMAGE && i == 6)
                    continue;

                if (eff == MItemDefinition.MAGIC && i == 7)
                    continue;

                if (i == 7 && eff == MItemDefinition.DAMAGE)
                {
                    int dam1 = GetItemAbility(mob.Equip.Items[6], MItemDefinition.DAMAGE) +
                        GetItemAbility(mob.Equip.Items[6], MItemDefinition.DAMAGE2);

                    int dam2 = GetItemAbility(mob.Equip.Items[7], MItemDefinition.DAMAGE) +
                        GetItemAbility(mob.Equip.Items[7], MItemDefinition.DAMAGE2);

                    int arm1 = mob.Equip.Items[6].Index;
                    int arm2 = mob.Equip.Items[7].Index;

                    int unique1 = 0;
                    if (arm1 > 0 && arm1 < GameBasics.MAX_ITEMLIST)
                        unique1 = ItemList[itemId].Unique;

                    int unique2 = 0;
                    if (arm2 > 0 && arm2 < GameBasics.MAX_ITEMLIST)
                        unique2 = ItemList[itemId].Unique;

                    if (unique1 != 0 && unique2 != 0)
                    {
                        int porc = 50;
                        if (unique1 == unique2)
                            porc = 70;

                        if (dam1 > dam2)
                            total = (short)((total + dam1) + (dam2 * porc / 100));
                        else
                            total = (short)((total + dam2) + (dam1 * porc / 100));

                        continue;
                    }

                    if (dam1 > dam2)
                        total += (short)dam1;
                    else
                        total += (short)dam2;

                    continue;
                }

                short value = GetItemAbility(mob.Equip.Items[i], eff);
                if (eff == MItemDefinition.ATTSPEED && value == 1)
                    value = 10;

                total += value;
            }

            return total;
        }

        public static short GetItemAbility(MItem item, int eff)
        {
            int result = 0;
            int itemId = item.Index;
            int unique = ItemList[itemId].Unique;
            int pos = ItemList[itemId].Pos;

            if (eff == MItemDefinition.DAMAGEADD || eff == MItemDefinition.MAGICADD)
                if (unique < 41 || unique > 50)
                    return 0;

            if (eff == MItemDefinition.CRITICAL)
                if (item.Effects[1].Index == MItemDefinition.CRITICAL2 || item.Effects[2].Index == MItemDefinition.CRITICAL2)
                    eff = MItemDefinition.CRITICAL2;

            if (eff == MItemDefinition.DAMAGE && pos == 32)
                if (item.Effects[1].Index == MItemDefinition.DAMAGE2 || item.Effects[2].Index == MItemDefinition.DAMAGE2)
                    eff = MItemDefinition.DAMAGE2;

            if (eff == MItemDefinition.MPADD2)
                if (item.Effects[1].Index == MItemDefinition.MPADD2 || item.Effects[2].Index == MItemDefinition.MPADD2)
                    eff = MItemDefinition.MPADD2;

            if (eff == MItemDefinition.ACADD)
                if (item.Effects[1].Index == MItemDefinition.ACADD2 || item.Effects[2].Index == MItemDefinition.ACADD2)
                    eff = MItemDefinition.ACADD2;

            if (eff == MItemDefinition.LEVEL && itemId >= 2330 && itemId < 2360)
                result = item.Effects[1].Index - 1;
            else if (eff == MItemDefinition.LEVEL)
                result += ItemList[itemId].Level;

            if (eff == MItemDefinition.REQ_STR)
                result += ItemList[itemId].Str;
            if (eff == MItemDefinition.REQ_INT)
                result += ItemList[itemId].Int;
            if (eff == MItemDefinition.REQ_CON)
                result += ItemList[itemId].Con;
            if (eff == MItemDefinition.REQ_DEX)
                result += ItemList[itemId].Dex;

            if (eff == MItemDefinition.POS)
                result += ItemList[itemId].Pos;

            if (eff != MItemDefinition.INCUBATE)
            {
                foreach (var i in ItemList[itemId].Effect)
                {
                    if (i.Index != eff)
                        continue;

                    int val = i.Value;
                    if (eff == MItemDefinition.ATTSPEED && val == 1)
                        val = 10;

                    result += val;
                    break;
                }
            }

            // parte de montarias não feito
            for (int i = 0; i < 3; i++)
            {
                if (item.Effects[i].Index != eff)
                    continue;

                int val = item.Effects[i].Value;
                if (eff == MItemDefinition.ATTSPEED && val == 1)
                    val = 10;

                result += val;
            }

            if (eff >= MItemDefinition.RESIST1 && eff <= MItemDefinition.RESIST4)
            {
                foreach (var i in ItemList[itemId].Effect)
                {
                    if (i.Index != MItemDefinition.RESISTALL)
                        continue;

                    result += i.Value;
                }

                for (int i = 0; i < 3; i++)
                {
                    if (item.Effects[i].Index != MItemDefinition.RESISTALL)
                        continue;

                    result += item.Effects[i].Value;
                    break;
                }
            }

            return (short)result;
        }
    }
}
