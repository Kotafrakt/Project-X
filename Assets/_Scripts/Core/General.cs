using UnityEngine;

public class General
{
    public float[] PARAMS = new float[42];
    public UnitType type;
    public string nameGeneral;                                                          //Имя Генерала
    public string descGeneral;
    public int[] expForLvlUp;                                                           //Сколько нужно опыта, для левел апа. Хз. Если можно это сделать масивом, то массив. 
    public Sprite img;                                                                  //Картинка
    public Sprite img2;                                                                 //КартинкаЮнита
    public GameObject prefab;
    public bool isDead = false; // трупик


    //Слоты шмота вражеского героя
    public Item head;                                                                   //Слот для шлемов
    public Item tors;                                                                   //Слот для нагрудника
    public Item bots;                                                                   //Слот для ботинок
    public Item weapon;                                                                 //Слот для оружия
    public Item art;                                                                    //Слот для артефакта
}
