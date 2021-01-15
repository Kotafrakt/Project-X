
public static class Defines
{
    ///////////////////////////////////////
    /// Описание параметров класcа Hero ///
    ///////////////////////////////////////

    //Параметры Героя
    public const int HERO_EXP = 0;                                                          //Опыт
    public const int HERO_LEVEL = 1;                                                        //Уровень
    public const int HERO_VIP_EXP = 2;                                                      //ВипОпыт
    public const int HERO_VIP_LEVEL = 3;                                                    //ВипУровень
    public const int HERO_MANA_MAX = 4;                                                     //Мана героя для призыва
    public const int HERO_MANA_CURRENT = 5;                                                 //Текущая мана
    public const int HERO_MANA_REGEN = 6;                                                   //Востановление маны

    //Бонусы от Героя в числах для компании
    public const int HERO_HP_BONUS = 7;                                                     //Дополнительное здоровье для монстра
    public const int HERO_HP_REGEN_BONUS = 8;                                               //Востановление здоровья монстров
    public const int HERO_DAMAGE_BONUS = 9;                                                 //Бонус урона
    public const int HERO_CRITS_DAMAGE_BONUS = 10;                                          //Бонус критического урона
    public const int HERO_CRITS_CHANCE_BONUS = 11;                                          //Бонус шанса критической атаки
    public const int HERO_ATTACK_DELAY_BONUS = 12;                                          //Задержка между атаками
    public const int HERO_DISTANCE_BONUS = 13;                                              //Бонус дистанции для атаки
    public const int HERO_ARMOR_BONUS = 14;                                                 //Бонус брони
    public const int HERO_FIRE_RESIST_BONUS = 15;                                           //Бонус резиста от огня
    public const int HERO_ICE_RESIST_BONUS = 16;                                            //Бонус резиста от холода
    public const int HERO_ELECTRIC_RESIST_BONUS = 17;                                       //Бонус резиста от молнии
    public const int HERO_SPEED_BONUS = 18;                                                 //Бонус скорости передвижения (Должен делиться на 100!)
    public const int HERO_UNIT_BONUS = 19;                                                  //Бонусный юнит бесплатно
    public const int HERO_UNIT_UPGRADE_BONUS = 20;                                          //Халявное грейд юнита во время призыва

    //Бонусы от Героя в % для компании
    public const int HERO_HP_BONUS_P = 21;                                                  //Дополнительное здоровье для монстра
    public const int HERO_HP_REGEN_BONUS_P = 22;                                            //Востановление здоровья монстров
    public const int HERO_DAMAGE_BONUS_P = 23;                                              //Бонус урона
    public const int HERO_CRITS_DAMAGE_BONUS_P = 24;                                        //Бонус критического урона
    public const int HERO_CRITS_CHANCE_BONUS_P = 25;                                        //Бонус шанса критической атаки
    public const int HERO_ATTACK_DELAY_BONUS_P = 26;                                        //Задержка между атаками
    public const int HERO_DISTANCE_BONUS_P = 27;                                            //Бонус дистанции для атаки
    public const int HERO_ARMOR_BONUS_P = 28;                                               //Бонус брони
    public const int HERO_FIRE_RESIST_BONUS_P = 29;                                         //Бонус резиста от огня
    public const int HERO_ICE_RESIST_BONUS_P = 30;                                          //Бонус резиста от холода
    public const int HERO_ELECTRIC_RESIST_BONUS_P = 31;                                     //Бонус резиста от молнии
    public const int HERO_SPEED_BONUS_P = 32;                                               //Бонус скорости передвижения (Должен делиться на 100!)



    ///////////////////////////////////////
    //////////Параметры для Арены /////////
    ///////////////////////////////////////


    //Параметры Героя
    //Боевые параметры(Основные)
    public const int ARENA_HERO_HP = 33;                                               //Жизни
    public const int ARENA_HERO_MANA = 34;                                             //Мана
    public const int ARENA_HERO_DAMAGE = 35;                                           //Урон физический
    public const int ARENA_HERO_DAMAGE_ICE = 36;                                       //Урон ледяной
    public const int ARENA_HERO_DAMAGE_FIRE = 37;                                      //Урон огненый
    public const int ARENA_HERO_DAMAGE_ELECTRIC = 38;                                  //Урон электрический
    public const int ARENA_HERO_STRENGTH = 39;                                         //Сила
    public const int ARENA_HERO_AGILITY = 40;                                          //Ловкость
    public const int ARENA_HERO_INTELLECT = 41;                                        //Интеллект
    public const int ARENA_HERO_DEFENSE_HEAD = 42;                                     //Защита головы
    public const int ARENA_HERO_DEFENSE_TORS = 43;                                     //Защита торса
    public const int ARENA_HERO_DEFENSE_PANTS = 44;                                    //Защита области пояса(паха)
    public const int ARENA_HERO_DEFENSE_BOTS = 45;                                     //Защита Ног
    public const int ARENA_HERO_DEFENSE_ICE = 46;                                      //Защита от льда
    public const int ARENA_HERO_DEFENSE_FIRE = 47;                                     //Защита от огня
    public const int ARENA_HERO_DEFENSE_ELECTRIC = 48;                                 //Защита от электричества
    public const int ARENA_HERO_AP_ATTACK = 49;                                        //Очки действия для атаки
    public const int ARENA_HERO_AP_DEFENSE = 50;                                       //Очки действия для защиты
    public const int ARENA_HERO_EVASION = 51;                                          //Уклонение
    public const int ARENA_HERO_BLOCK = 52;                                            //Блок
    public const int ARENA_HERO_ACCURACY = 53;                                         //Точность
    public const int ARENA_HERO_CRIT_DAMAGE = 54;                                      //Критический урон
    public const int ARENA_HERO_CRIT_CHANGE = 55;                                      //Крит шанс
    public const int ARENA_HERO_PARRY = 56;                                            //Парирование удара/отражение

    //Боевые параметры(Текущий)
    public const int ARENA_HERO_HP_CURRENT = 57;                                       //Жизни
    public const int ARENA_HERO_MANA_CURRENT = 58;                                     //Мана
    public const int ARENA_HERO_DAMAGE_CURRENT = 59;                                   //Урон физический
    public const int ARENA_HERO_DAMAGE_ICE_CURRENT = 60;                               //Урон ледяной
    public const int ARENA_HERO_DAMAGE_FIRE_CURRENT = 61;                              //Урон огненый
    public const int ARENA_HERO_DAMAGE_ELECTRIC_CURRENT = 62;                          //Урон электрический
    public const int ARENA_HERO_STRENGTH_CURRENT = 63;                                 //Сила
    public const int ARENA_HERO_AGILITY_CURRENT = 64;                                  //Ловкость
    public const int ARENA_HERO_INTELLECT_CURRENT = 65;                                //Интеллект
    public const int ARENA_HERO_DEFENSE_HEAD_CURRENT = 66;                             //Защита головы
    public const int ARENA_HERO_DEFENSE_TORS_CURRENT = 67;                             //Защита торса
    public const int ARENA_HERO_DEFENSE_PANTS_CURRENT = 68;                            //Защита области пояса(паха)
    public const int ARENA_HERO_DEFENSE_BOTS_CURRENT = 69;                             //Защита Ног
    public const int ARENA_HERO_DEFENSE_ICE_CURRENT = 70;                              //Защита от льда
    public const int ARENA_HERO_DEFENSE_FIRE_CURRENT = 71;                             //Защита от огня
    public const int ARENA_HERO_DEFENSE_ELECTRIC_CURRENT = 72;                         //Защита от электричества
    public const int ARENA_HERO_AP_ATTACK_CURRENT = 73;                                //Очки действия для атаки
    public const int ARENA_HERO_AP_DEFENSE_CURRENT = 74;                               //Очки действия для защиты
    public const int ARENA_HERO_EVASION_CURRENT = 75;                                  //Уклонение
    public const int ARENA_HERO_BLOCK_CURRENT = 76;                                    //Блок
    public const int ARENA_HERO_ACCURACY_CURRENT = 77;                                 //Точность
    public const int ARENA_HERO_CRIT_DAMAGE_CURRENT = 78;                              //Критический урон
    public const int ARENA_HERO_CRIT_CHANGE_CURRENT = 79;                              //Крит шанс
    public const int ARENA_HERO_PARRY_CURRENT = 80;                                    //Парирование удара/отражение



    //Параметры врага
    public const int ARENA_ENEMY_EXP = 0;                                                   //Опыт
    public const int ARENA_ENEMY_LEVEL = 1;                                                 //Уровень
    public const int ARENA_ENEMY_VIP_EXP = 2;                                               //ВипОпыт
    public const int ARENA_ENEMY_VIP_LEVEL = 3;                                             //ВипУровень

    //Боевые параметры(Основные)                                                                                      
    public const int ARENA_ENEMY_HP = 4;                                              //Жизни
    public const int ARENA_ENEMY_MANA = 5;                                            //Мана
    public const int ARENA_ENEMY_DAMAGE = 6;                                          //Урон физический
    public const int ARENA_ENEMY_DAMAGE_ICE = 7;                                      //Урон ледяной
    public const int ARENA_ENEMY_DAMAGE_FIRE = 8;                                     //Урон огненый
    public const int ARENA_ENEMY_DAMAGE_ELECTRIC = 9;                                 //Урон электрический
    public const int ARENA_ENEMY_STRENGTH = 10;                                        //Сила
    public const int ARENA_ENEMY_AGILITY = 11;                                         //Ловкость
    public const int ARENA_ENEMY_INTELLECT = 12;                                       //Интеллект
    public const int ARENA_ENEMY_DEFENSE_HEAD = 13;                                    //Защита головы
    public const int ARENA_ENEMY_DEFENSE_TORS = 14;                                    //Защита торса
    public const int ARENA_ENEMY_DEFENSE_PANTS = 15;                                   //Защита области пояса(паха)
    public const int ARENA_ENEMY_DEFENSE_BOTS = 16;                                    //Защита Ног
    public const int ARENA_ENEMY_DEFENSE_ICE = 17;                                     //Защита от льда
    public const int ARENA_ENEMY_DEFENSE_FIRE = 18;                                    //Защита от огня
    public const int ARENA_ENEMY_DEFENSE_ELECTRIC = 19;                               //Защита от электричества
    public const int ARENA_ENEMY_AP_ATTACK = 20;                                      //Очки действия для атаки
    public const int ARENA_ENEMY_AP_DEFENSE = 21;                                     //Очки действия для защиты
    public const int ARENA_ENEMY_EVASION = 22;                                        //Уклонение
    public const int ARENA_ENEMY_BLOCK = 23;                                          //Блок
    public const int ARENA_ENEMY_ACCURACY = 24;                                       //Точность
    public const int ARENA_ENEMY_CRIT_DAMAGE = 25;                                    //Критический урон
    public const int ARENA_ENEMY_CRIT_CHANGE = 26;                                    //Крит шанс
    public const int ARENA_ENEMY_PARRY = 27;                                          //Парирование удара/отражение

    //Боевые параметры(Текущий)
    public const int ARENA_ENEMY_HP_CURRENT = 28;                                     //Жизни
    public const int ARENA_ENEMY_MANA_CURRENT = 29;                                   //Мана
    public const int ARENA_ENEMY_DAMAGE_CURRENT = 30;                                 //Урон физический
    public const int ARENA_ENEMY_DAMAGE_ICE_CURRENT = 31;                             //Урон ледяной
    public const int ARENA_ENEMY_DAMAGE_FIRE_CURRENT = 32;                            //Урон огненый
    public const int ARENA_ENEMY_DAMAGE_ELECTRIC_CURRENT = 33;                        //Урон электрический
    public const int ARENA_ENEMY_STRENGTH_CURRENT = 34;                               //Сила
    public const int ARENA_ENEMY_AGILITY_CURRENT = 35;                                //Ловкость
    public const int ARENA_ENEMY_INTELLECT_CURRENT = 36;                              //Интеллект
    public const int ARENA_ENEMY_DEFENSE_HEAD_CURRENT = 37;                           //Защита головы
    public const int ARENA_ENEMY_DEFENSE_TORS_CURRENT = 38;                           //Защита торса
    public const int ARENA_ENEMY_DEFENSE_PANTS_CURRENT = 39;                          //Защита области пояса(паха)
    public const int ARENA_ENEMY_DEFENSE_BOTS_CURRENT = 40;                           //Защита Ног
    public const int ARENA_ENEMY_DEFENSE_ICE_CURRENT = 41;                            //Защита от льда
    public const int ARENA_ENEMY_DEFENSE_FIRE_CURRENT = 42;                           //Защита от огня
    public const int ARENA_ENEMY_DEFENSE_ELECTRIC_CURRENT = 43;                       //Защита от электричества
    public const int ARENA_ENEMY_AP_ATTACK_CURRENT = 44;                              //Очки действия для атаки
    public const int ARENA_ENEMY_AP_DEFENSE_CURRENT = 45;                             //Очки действия для защиты
    public const int ARENA_ENEMY_EVASION_CURRENT = 46;                                //Уклонение
    public const int ARENA_ENEMY_BLOCK_CURRENT = 47;                                  //Блок
    public const int ARENA_ENEMY_ACCURACY_CURRENT = 48;                               //Точность
    public const int ARENA_ENEMY_CRIT_DAMAGE_CURRENT = 49;                            //Критический урон
    public const int ARENA_ENEMY_CRIT_CHANGE_CURRENT = 50;                            //Крит шанс
    public const int ARENA_ENEMY_PARRY_CURRENT = 51;                                  //Парирование удара/отражение



    ///////////////////////////////////////
    ////////Параметры     Генералов////////
    ///////////////////////////////////////  



    //Боевые параметры(Основной)
    public const int GENERAL_EXP = 0;
    public const int GENERAL_LEVEL = 1;
    public const int GENERAL_HP = 2;                                                                      //Жизни
    public const int GENERAL_MANA = 3;                                                                    //Мана
    public const int GENERAL_DAMAGE = 4;                                                                  //Урон физический
    public const int GENERAL_DAMAGE_ICE = 5;                                                               //Урон ледяной
    public const int GENERAL_DAMAGE_FIRE = 6;                                                              //Урон огненый
    public const int GENERAL_DAMAGE_ELECTRIC = 7;                                                          //Урон электрический
    public const int GENERAL_STRENGTH = 8;                                                                //Сила
    public const int GENERAL_AGILITY = 9;                                                                 //Ловкость
    public const int GENERAL_INTELLECT = 10;                                                               //Интеллект
    public const int GENERAL_DEFENSE = 11;                                                                 //Защита/Броня
    public const int GENERAL_DEFENSE_ICE = 12;                                                              //Защита от льда
    public const int GENERAL_DEFENSE_FIRE = 13;                                                             //Защита от огня
    public const int GENERAL_DEFENSE_ELECTRIC = 14;                                                         //Защита от электричества
    public const int GENERAL_EVASION = 15;                                                                 //Уклонение
    public const int GENERAL_BLOCK = 16;                                                                   //Блок
    public const int GENERAL_ACCURACY = 17;                                                                //Точность
    public const int GENERAL_CRIT_DAMAGE = 18;                                                              //Критический урон
    public const int GENERAL_CRIT_CHANGE = 19;                                                              //Крит шанс
    public const int GENERAL_INITIATIVE = 20;                                                              //Инициатива
    public const int GENERAL_PARRY = 21;                                                                   //Парирование удара/отражение

    //Боевые параметры(Текущий)
    public const int GENERAL_HP_CURRENT = 22;                                                               //
    public const int GENERAL_MANA_CURRENT = 23;                                                             //
    public const int GENERAL_DAMAGE_CURRENT = 24;                                                           //
    public const int GENERAL_DAMAGE_ICE_CURRENT = 25;                                                        //
    public const int GENERAL_DAMAGE_FIRE_CURRENT = 26;                                                       //
    public const int GENERAL_DAMAGE_ELECTRIC_CURRENT = 27;                                                   //
    public const int GENERAL_STRENGTH_CURRENT = 28;                                                         //
    public const int GENERAL_AGILITY_CURRENT = 29;                                                          //
    public const int GENERAL_INTELLECT_CURRENT = 30;                                                        //
    public const int GENERAL_DEFENSE_CURRENT = 31;
    public const int GENERAL_DEFENSE_ICE_CURRENT = 32;                                                       //применения модификаций
    public const int GENERAL_DEFENSE_FIRE_CURRENT = 33;                                                      //
    public const int GENERAL_DEFENSE_ELECTRIC_CURRENT = 34;                                                  //
    public const int GENERAL_EVASION_CURRENT = 35;                                                          //
    public const int GENERAL_BLOCK_CURRENT = 36;                                                            //
    public const int GENERAL_ACCURACY_CURRENT = 37;                                                         //
    public const int GENERAL_CRIT_DAMAGE_CURRENT = 38;                                                       //
    public const int GENERAL_CRIT_CHANGE_CURRENT = 39;                                                       //
    public const int GENERAL_INITIATIVE_CURRENT = 40;                                                       //
    public const int GENERAL_PARRY_CURRENT = 41;                                                            //





    ///////////////////////////////////////
    ///////Описание параметров Мобов///////
    ///////////////////////////////////////




    public const int UNIT_COST = 0;                                         //Цена за еденицу
    public const int UNIT_COUNT = 1;                                        //Колличество
    public const int UNIT_HP_MAX = 2;                                        //Максимальное хп
    public const int UNIT_HP_REGEN = 3;                                      //Востановление здоровья монстров
    public const int UNIT_ATTACK_DELAY = 4;                                  //Задежка атаки
    public const int UNIT_DISTANCE = 5;                                     //Дистанция атаки
    public const int UNIT_SPEED = 6;                                        //Скорость передвижения
    public const int UNIT_DAMAGE = 7;                                       //Урон
    public const int UNIT_CRIT_DAMAGE = 8;                                  //Бонус критического урона
    public const int UNIT_CRIT_CHANCE = 9;                                  //Бонус шанса критической атаки
    public const int UNIT_ARMOR = 10;                                        //Защита
    public const int UNIT_FIRE_RESIST = 11;                                   //Защита от атак огня
    public const int UNIT_ICE_RESIST = 12;                                    //Защита от атак холода
    public const int UNIT_ELECTRIC_RESIST = 13;                               //Защита от атак молнии




    public const int ENEMY_COUNT = 0;                                        //Колличество
    public const int ENEMY_HP_MAX = 1;                                        //Максимальное хп
    public const int ENEMY_ATTACK_DELAY = 2;                                  //Задежка атаки
    public const int ENEMY_DISTANCE = 3;                                     //Дистанция атаки
    public const int ENEMY_SPEED = 4;                                        //Скорость передвижения
    public const int ENEMY_DAMAGE = 5;                                       //Урон
    public const int ENEMY_ARMOR = 6;                                        //Защита
    public const int ENEMY_FIRE_RESIST = 7;                                   //Защита от атак огня
    public const int ENEMY_ICE_RESIST = 8;                                    //Защита от атак холода
    public const int ENEMY_ELECTRIC_RESIST = 9;                               //Защита от атак молнии


    ///////////////////////////////////////
    /// Описание параметров класcа Item ///
    ///////////////////////////////////////

    //Бонусы от Героя в числах
    public const int ITEM_HP = 0;                                                           //Дополнительное здоровье для монстра
    public const int ITEM_HP_REGEN = 1;                                                     //Востановление здоровья монстров
    public const int ITEM_DAMAGE = 2;                                                       //Бонус урона
    public const int ITEM_CRITS_DAMAGE = 3;                                                 //Бонус критического урона
    public const int ITEM_CRITS_CHANCE = 4;                                                 //Бонус шанса критической атаки
    public const int ITEM_ATTACK_DELAY = 5;                                                 //Задержка между атаками
    public const int ITEM_DISTANCE = 6;                                                     //Бонус дистанции для атаки
    public const int ITEM_ARMOR = 7;                                                        //Бонус брони
    public const int ITEM_FIRE_RESIST = 8;                                                  //Бонус резиста от огня
    public const int ITEM_ICE_RESIST = 9;                                                   //Бонус резиста от холода
    public const int ITEM_ELECTRIC_RESIST = 10;                                             //Бонус резиста от молнии
    public const int ITEM_SPEED = 11;                                                       //Бонус скорости передвижения (Должен делиться на 100!)
    public const int ITEM_UNIT = 12;                                                        //Бонусный юнит бесплатно
    public const int ITEM_UNIT_UPGRADE = 13;                                                //Халявное грейд юнита во время призыва

    //Бонусы от Героя в %
    public const int ITEM_HP_P = 14;                                                        //Дополнительное здоровье для монстра
    public const int ITEM_HP_REGEN_P = 15;                                                  //Востановление здоровья монстров
    public const int ITEM_DAMAGE_P = 16;                                                    //Бонус урона
    public const int ITEM_CRITS_DAMAGE_P = 17;                                              //Бонус критического урона
    public const int ITEM_CRITS_CHANCE_P = 18;                                              //Бонус шанса критической атаки
    public const int ITEM_ATTACK_DELAY_P = 19;                                              //Задержка между атаками
    public const int ITEM_DISTANCE_P = 20;                                                  //Бонус дистанции для атаки
    public const int ITEM_ARMOR_P = 21;                                                     //Бонус брони
    public const int ITEM_FIRE_RESIST_P = 22;                                               //Бонус резиста от огня
    public const int ITEM_ICE_RESIST_P = 23;                                                //Бонус резиста от холода
    public const int ITEM_ELECTRIC_RESIST_P = 24;                                           //Бонус резиста от молнии
    public const int ITEM_SPEED_P = 25;                                                     //Бонус скорости передвижения

    ///////////////////////////////////////
    /// Описание параметров кла Ресурсы ///
    ///////////////////////////////////////

    //Ресурсы
    public const int GOLD = 0;               //Роскошь
    public const int WOOD = 1;               //Дерево
    public const int ROCK = 2;               //Камень
    public const int IRON = 3;               //Железо
    public const int BONES = 4;              //Кости
    public const int BODY = 5;               //Плоть
    public const int SOULS = 6;              //Души
    public const int REAL = 7;               //Премиум валюта

    //Целые предметы
    //Артефакты
    public const int ARTIFACT_V0 = 8;           //Неизвестный Артефакт
    public const int ARTIFACT_V1 = 9;           //Неизвестный Артефакт 1 грейда
    public const int ARTIFACT_V2 = 10;          //Неизвестный Артефакт 2 грейда

    //Эквип 
    public const int EQUIPMENT_V0 = 11;         //Неизвестная Экипировка
    public const int EQUIPMENT_V1 = 12;         //Неизвестная Экипировка 1 грейда
    public const int EQUIPMENT_V2 = 13;         //Неизвестная Экипировка 2 грейда

    //Юниты
    public const int UNIT_V0 = 14;              //Неизвестный Юнит
    public const int UNIT_V1 = 15;              //Неизвестный Юнит 1 грейда
    public const int UNIT_V2 = 16;              //Неизвестный Юнит 2 грейда

    //Сборные предметы
    //Артефакты
    public const int ARTIFACT_V0_PARTS = 17;           //Неизвестный Артефакт состоящий из N частей
    public const int ARTIFACT_V1_PARTS = 18;           //Неизвестный Артефакт 1 грейда состоящий из N частей
    public const int ARTIFACT_V2_PARTS = 19;          //Неизвестный Артефакт 2 грейда состоящий из N частей

    //Эквип 
    public const int EQUIPMENT_V0_PARTS = 20;         //Неизвестная Экипировка состоящий из N частей
    public const int EQUIPMENT_V1_PARTS = 21;         //Неизвестная Экипировка 1 грейда состоящий из N частей
    public const int EQUIPMENT_V2_PARTS = 22;         //Неизвестная Экипировка 2 грейда состоящий из N частей

    //Юниты
    public const int UNIT_V0_PARTS = 23;              //Неизвестный Юнит состоящий из N частей
    public const int UNIT_V1_PARTS = 24;              //Неизвестный Юнит 1 грейда состоящий из N частей
    public const int UNIT_V2_PARTS = 25;              //Неизвестный Юнит 2 грейда состоящий из N частей
}
