using UnityEngine;
using static Defines;

public static class CreateGeneral
{
    public static General GenarateGeneral()
    {
        int i = 0;
        //i = Random.Range(0, 9);
        General general = new General();
        general.nameGeneral = GeneralText.GetName(i);
        general.descGeneral = GeneralText.GetDescription(i);
        general.prefab = ResManager.instance.gens[i];
        general.img = ResManager.instance.genImg[i];
        general.img2 = ResManager.instance.genImg2[i];
        general.PARAMS[GENERAL_LEVEL] = Random.Range(7, 15);
        general.PARAMS[GENERAL_STRENGTH] = Random.Range(3, 8) + (Random.Range(1, 3) * general.PARAMS[GENERAL_LEVEL]) - 2;                                                                //Сила
        general.PARAMS[GENERAL_AGILITY] = Random.Range(3, 8) + (Random.Range(1, 3) * general.PARAMS[GENERAL_LEVEL]) - 2;       //Ловкость
        general.PARAMS[GENERAL_INTELLECT] = Random.Range(3, 8) + (Random.Range(1, 3) * general.PARAMS[GENERAL_LEVEL]) - 2;       //Интеллект
        general.PARAMS[GENERAL_EXP] = 0;        
        general.PARAMS[GENERAL_HP] = 500 + 5 * general.PARAMS[GENERAL_STRENGTH];                                                                      //Жизни
        general.PARAMS[GENERAL_MANA] = 10 + 5 * general.PARAMS[GENERAL_INTELLECT];                                                                    //Мана
        general.PARAMS[GENERAL_DAMAGE] = 10 + (3 * general.PARAMS[GENERAL_STRENGTH] + general.PARAMS[GENERAL_INTELLECT]) / 2;                                                                  //Урон физический
        general.PARAMS[GENERAL_DAMAGE_ICE] = 0;                                                               //Урон ледяной
        general.PARAMS[GENERAL_DAMAGE_FIRE] = 0;                                                              //Урон огненый
        general.PARAMS[GENERAL_DAMAGE_ELECTRIC] = 0;                                                          //Урон электрический
        general.PARAMS[GENERAL_DEFENSE] = 0 + (2 * general.PARAMS[GENERAL_STRENGTH] + 3 * general.PARAMS[GENERAL_AGILITY]) / 10;                                                                 //Защита/Броня
        general.PARAMS[GENERAL_DEFENSE_ICE] = 0 + general.PARAMS[GENERAL_INTELLECT] / 2;                                                              //Защита от льда
        general.PARAMS[GENERAL_DEFENSE_FIRE] = 0 + general.PARAMS[GENERAL_INTELLECT] / 2;                                                             //Защита от огня
        general.PARAMS[GENERAL_DEFENSE_ELECTRIC] = 0 + general.PARAMS[GENERAL_INTELLECT] / 2;                                                         //Защита от электричества
        general.PARAMS[GENERAL_EVASION] = 0 + general.PARAMS[GENERAL_AGILITY] / 5;                                                                //Уклонение
        general.PARAMS[GENERAL_BLOCK] = 0 + general.PARAMS[GENERAL_AGILITY] / 5;                                                                   //Блок
        general.PARAMS[GENERAL_ACCURACY] = 40 + (3 * general.PARAMS[GENERAL_AGILITY] + 2 * general.PARAMS[GENERAL_INTELLECT]) / 10;                                                                //Точность
        general.PARAMS[GENERAL_CRIT_DAMAGE] = 30;                                                              //Критический урон
        general.PARAMS[GENERAL_CRIT_CHANGE] = 5;                                                              //Крит шанс
        general.PARAMS[GENERAL_INITIATIVE] = 0 + general.PARAMS[GENERAL_INTELLECT] * 5;                                                              //Инициатива
        general.PARAMS[GENERAL_PARRY] = 0 + general.PARAMS[GENERAL_AGILITY] / 5;                                                                   //Парирование удара/отражение

        general.skill0 = SkillName.skill0;
        general.skill_lvl_0 = 1;
        general.skill0Delay = SkillInfo.GetSkill(general.skill0, general.skill_lvl_0).delay;
        general.skill0Delay_Curent = general.skill0Delay;
        general.skill1 = SkillName.skill1;
        general.skill_lvl_1 = 1;
        general.skill2 = SkillName.skill2;
        general.skill_lvl_2 = 1;

        general.skillB = SkillNameB.skillB0;
        general.skill_lvl_B = 1;

        /*switch (general.nameGeneral)    Скилы выдаваемые при создании генералу, в зависимости от его имени либо типа.
        {
            case "Алёша":
                general.skill0 = SkillName.skill0;
                general.skill_lvl_0 = 1;
                general.skill1 = SkillName.skill1;
                general.skill_lvl_1 = 0;
                general.skill2 = SkillName.skill2;
                general.skill_lvl_2 = 0;
                general.skillB = SkillNameB.skillB0;
                general.skill_lvl_B = 0;
                break;
            case "Клоун":
                general.skill0 = SkillName.skill0;
                general.skill_lvl_0 = 0;
                general.skill1 = SkillName.skill1;
                general.skill_lvl_1 = 0;
                general.skill2 = SkillName.skill2;
                general.skill_lvl_2 = 0;
                general.skillB = SkillNameB.skillB0;
                general.skill_lvl_B = 0;
                break;

        }*/


        //Боевые параметры(Текущий)
        general.PARAMS[GENERAL_HP_CURRENT] = general.PARAMS[GENERAL_HP];
        general.PARAMS[GENERAL_MANA_CURRENT] = general.PARAMS[GENERAL_MANA];
        general.PARAMS[GENERAL_DAMAGE_CURRENT] = general.PARAMS[GENERAL_DAMAGE];
        general.PARAMS[GENERAL_DAMAGE_ICE_CURRENT] = general.PARAMS[GENERAL_DAMAGE_ICE];
        general.PARAMS[GENERAL_DAMAGE_FIRE_CURRENT] = general.PARAMS[GENERAL_DAMAGE_FIRE];
        general.PARAMS[GENERAL_DAMAGE_ELECTRIC_CURRENT] = general.PARAMS[GENERAL_DAMAGE_ELECTRIC];
        general.PARAMS[GENERAL_STRENGTH_CURRENT] = general.PARAMS[GENERAL_STRENGTH];
        general.PARAMS[GENERAL_AGILITY_CURRENT] = general.PARAMS[GENERAL_AGILITY];
        general.PARAMS[GENERAL_INTELLECT_CURRENT] = general.PARAMS[GENERAL_INTELLECT];
        general.PARAMS[GENERAL_DEFENSE_CURRENT] = general.PARAMS[GENERAL_DEFENSE];
        general.PARAMS[GENERAL_DEFENSE_ICE_CURRENT] = general.PARAMS[GENERAL_DEFENSE_ICE];
        general.PARAMS[GENERAL_DEFENSE_FIRE_CURRENT] = general.PARAMS[GENERAL_DEFENSE_FIRE];
        general.PARAMS[GENERAL_DEFENSE_ELECTRIC_CURRENT] = general.PARAMS[GENERAL_DEFENSE_ELECTRIC];
        general.PARAMS[GENERAL_EVASION_CURRENT] = general.PARAMS[GENERAL_EVASION];
        general.PARAMS[GENERAL_BLOCK_CURRENT] = general.PARAMS[GENERAL_BLOCK];
        general.PARAMS[GENERAL_ACCURACY_CURRENT] = general.PARAMS[GENERAL_ACCURACY];
        general.PARAMS[GENERAL_CRIT_DAMAGE_CURRENT] = general.PARAMS[GENERAL_CRIT_DAMAGE];
        general.PARAMS[GENERAL_CRIT_CHANGE_CURRENT] = general.PARAMS[GENERAL_CRIT_CHANGE];
        general.PARAMS[GENERAL_INITIATIVE_CURRENT] = general.PARAMS[GENERAL_INITIATIVE];
        general.PARAMS[GENERAL_PARRY_CURRENT] = general.PARAMS[GENERAL_PARRY];

        return general;
    }

    public static General GeneralEasy(int level)
    {
        General general = new General();
        general.nameGeneral = "Генерал";
        general.PARAMS[GENERAL_LEVEL] = level;
        general.PARAMS[GENERAL_STRENGTH] = Random.Range(3, 8) + (Random.Range(1, 3) * general.PARAMS[GENERAL_LEVEL]) - 2;                                                                //Сила
        general.PARAMS[GENERAL_AGILITY] = Random.Range(3, 8) + (Random.Range(1, 3) * general.PARAMS[GENERAL_LEVEL]) - 2;                                                                 //Ловкость
        general.PARAMS[GENERAL_INTELLECT] = Random.Range(3, 8) + (Random.Range(1, 3) * general.PARAMS[GENERAL_LEVEL]) - 2;                                                               //Интеллект
        general.PARAMS[GENERAL_HP] = 500 + 4 * general.PARAMS[GENERAL_STRENGTH];                                                                      //Жизни
        general.PARAMS[GENERAL_MANA] = 10 + 5 * general.PARAMS[GENERAL_INTELLECT];                                                                    //Мана
        general.PARAMS[GENERAL_DAMAGE] = 10 + (3 * general.PARAMS[GENERAL_STRENGTH] + general.PARAMS[GENERAL_INTELLECT]) / 2;                                                                  //Урон физический
        general.PARAMS[GENERAL_DAMAGE_ICE] = 0;                                                               //Урон ледяной
        general.PARAMS[GENERAL_DAMAGE_FIRE] = 0;                                                              //Урон огненый
        general.PARAMS[GENERAL_DAMAGE_ELECTRIC] = 0;                                                          //Урон электрический
        general.PARAMS[GENERAL_DEFENSE] = 0 + (2 * general.PARAMS[GENERAL_STRENGTH] + 3 * general.PARAMS[GENERAL_AGILITY]) / 10;                                                                 //Защита/Броня
        general.PARAMS[GENERAL_DEFENSE_ICE] = 0 + general.PARAMS[GENERAL_INTELLECT] / 2;                                                              //Защита от льда
        general.PARAMS[GENERAL_DEFENSE_FIRE] = 0 + general.PARAMS[GENERAL_INTELLECT] / 2;                                                             //Защита от огня
        general.PARAMS[GENERAL_DEFENSE_ELECTRIC] = 0 + general.PARAMS[GENERAL_INTELLECT] / 2;                                                         //Защита от электричества
        general.PARAMS[GENERAL_EVASION] = 0 + general.PARAMS[GENERAL_AGILITY] / 5;                                                                //Уклонение
        general.PARAMS[GENERAL_BLOCK] = 0 + general.PARAMS[GENERAL_AGILITY] / 5;                                                                   //Блок
        general.PARAMS[GENERAL_ACCURACY] = 40 + (3 * general.PARAMS[GENERAL_AGILITY] + 2 * general.PARAMS[GENERAL_INTELLECT]) / 10;                                                                //Точность
        general.PARAMS[GENERAL_CRIT_DAMAGE] = 30;                                                              //Критический урон
        general.PARAMS[GENERAL_CRIT_CHANGE] = 5;                                                              //Крит шанс
        general.PARAMS[GENERAL_INITIATIVE] = 5;//0 + general.PARAMS[GENERAL_INTELLECT] * 5;                                                              //Инициатива
        general.PARAMS[GENERAL_PARRY] = 0 + general.PARAMS[GENERAL_AGILITY] / 5;

        //Боевые параметры(Текущий)
        general.PARAMS[GENERAL_HP_CURRENT] = general.PARAMS[GENERAL_HP];                                                               //
        general.PARAMS[GENERAL_MANA_CURRENT] = general.PARAMS[GENERAL_MANA];                                                             //
        general.PARAMS[GENERAL_DAMAGE_CURRENT] = general.PARAMS[GENERAL_DAMAGE];                                                           //
        general.PARAMS[GENERAL_DAMAGE_ICE_CURRENT] = general.PARAMS[GENERAL_DAMAGE_ICE];                                                        //
        general.PARAMS[GENERAL_DAMAGE_FIRE_CURRENT] = general.PARAMS[GENERAL_DAMAGE_FIRE];                                                       //
        general.PARAMS[GENERAL_DAMAGE_ELECTRIC_CURRENT] = general.PARAMS[GENERAL_DAMAGE_ELECTRIC];                                                   //
        general.PARAMS[GENERAL_STRENGTH_CURRENT] = general.PARAMS[GENERAL_STRENGTH];                                                         //
        general.PARAMS[GENERAL_AGILITY_CURRENT] = general.PARAMS[GENERAL_AGILITY];                                                          //
        general.PARAMS[GENERAL_INTELLECT_CURRENT] = general.PARAMS[GENERAL_INTELLECT];                                                        //
        general.PARAMS[GENERAL_DEFENSE_CURRENT] = general.PARAMS[GENERAL_DEFENSE];                                                      //
        general.PARAMS[GENERAL_DEFENSE_ICE_CURRENT] = general.PARAMS[GENERAL_DEFENSE_ICE];                                                       //применения модификаций
        general.PARAMS[GENERAL_DEFENSE_FIRE_CURRENT] = general.PARAMS[GENERAL_DEFENSE_FIRE];                                                      //
        general.PARAMS[GENERAL_DEFENSE_ELECTRIC_CURRENT] = general.PARAMS[GENERAL_DEFENSE_ELECTRIC];                                                  //
        general.PARAMS[GENERAL_EVASION_CURRENT] = general.PARAMS[GENERAL_EVASION];                                                          //
        general.PARAMS[GENERAL_BLOCK_CURRENT] = general.PARAMS[GENERAL_BLOCK];                                                            //
        general.PARAMS[GENERAL_ACCURACY_CURRENT] = general.PARAMS[GENERAL_ACCURACY];                                                         //
        general.PARAMS[GENERAL_CRIT_DAMAGE_CURRENT] = general.PARAMS[GENERAL_CRIT_DAMAGE];                                                       //
        general.PARAMS[GENERAL_CRIT_CHANGE_CURRENT] = general.PARAMS[GENERAL_CRIT_CHANGE];                                                       //
        general.PARAMS[GENERAL_INITIATIVE_CURRENT] = general.PARAMS[GENERAL_INITIATIVE];                                                       //
        general.PARAMS[GENERAL_PARRY_CURRENT] = general.PARAMS[GENERAL_PARRY];                                                            //

        return general;
    }
}
