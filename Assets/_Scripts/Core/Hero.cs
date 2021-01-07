using UnityEngine;

public static class Hero
{
    // Параметры Героя
    public static float[] PARAMS = new float[81];
    public static string nameHero;                                                      //Имя героя
    public static int[] expForLvlUp;                                                    //Сколько нужно опыта, для левел апа. Хз. Если можно это сделать масивом, то массив.
    public static int[] vipExpForLvlUp;                                                 //Сколько нужно вип опыта, для левел апа. Хз. Если можно это сделать масивом, то массив.
    public static Sprite img;                                                                  //Картинка
    public static Sprite img2;                                                                 //КартинкаЮнита


    //Слоты героя для ТоверДефенса
    public static Item head;                                                            //Слот для шлемов
    public static Item tors;                                                            //Слот для нагрудника
    public static Item pants;                                                           //Слот для штаники
    public static Item bots;                                                            //Слот для ботинок
    public static Item weapon0;                                                         //Слот для оружия 1
    public static Item weapon1;                                                         //Слот для оружия 2
    public static Item amulet;                                                          //Слот для амулета
    public static Item ring0;                                                           //Слот для кольца 1
    public static Item ring1;                                                           //Слот для кольца 2
    public static Item art0;                                                            //Слот для артефакта 1
    public static Item art1;                                                            //Слот для артефакта 2
    public static Item art2;                                                            //Слот для артефакта 3


    //Слоты шмота героя для Арены
    public static Item arenaHead;                                                       //Слот для шлемов
    public static Item arenaTors;                                                       //Слот для нагрудника
    public static Item arenaPants;                                                      //Слот для штаники
    public static Item arenaBots;                                                       //Слот для ботинок
    public static Item arenaWeapon0;                                                    //Слот для оружия
    public static Item arenaWeapon1;                                                    //Слот для оружия
    public static Item arenaAmulet;                                                     //Слот для амулета
    public static Item arenaRing0;                                                      //Слот для кольца 1
    public static Item arenaRing1;                                                      //Слот для кольца 2
    public static Item arenaArt;                                                        //Слот для артефакта

}
