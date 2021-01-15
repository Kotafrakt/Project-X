using UnityEngine;

public enum SkillType
{
    none, skill0, skill1, skill2, aoeLink, aoeBoom//aoe - все скилы с уроном по области
}
public enum SkillTypeBonus
{
    none, skillB0, skillB1
}

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

    //Скилы
    public SkillType skill0;
    public SkillType skill1;
    public SkillType skill2;
    public int availableSkill;

    public int skillLvl_0 = 0;
    public int skillLvl_1 = 0;
    public int skillLvl_2 = 0;
    public int skillLvlBoss = 0;


    public SkillTypeBonus skillB;
    public bool availableSkillB = false;



    //Слоты шмота вражеского героя
    public Item head;                                                                   //Слот для шлемов
    public Item tors;                                                                   //Слот для нагрудника
    public Item bots;                                                                   //Слот для ботинок
    public Item weapon;                                                                 //Слот для оружия
    public Item art;                                                                    //Слот для артефакта
}
