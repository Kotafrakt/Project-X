using UnityEngine;

public class HeroEnemy
{

    //Параметры вражеского героя, для офлайн варианта
    public float[] PARAMS = new float[52];
    public string nameHeroEnemy;                                                        //Имя монстра
    public int[] expForLvlUp;                                                           //Сколько нужно опыта, для левел апа. Хз. Если можно это сделать масивом, то массив.
    public int[] vipExpForLvlUp;                                                        //Сколько нужно вип опыта, для левел апа. Хз. Если можно это сделать масивом, то массив.
    public Sprite img;                                                                  //Картинка
    public Sprite img2;                                                                 //КартинкаЮнита

    //Слоты шмота вражеского героя
    public Item head;                                                                   //Слот для шлемов
    public Item tors;                                                                   //Слот для нагрудника
    public Item pants;                                                                  //Слот для штаны
    public Item bots;                                                                   //Слот для ботинок
    public Item weapon0;                                                                //Слот для оружия
    public Item weapon1;                                                                //Слот для оружия
    public Item amulet;                                                                 //Слот для амулета
    public Item ring0;                                                                  //Слот для кольца 1
    public Item ring1;                                                                  //Слот для кольца 2
    public Item art;                                                                    //Слот для артефакта 1
}
