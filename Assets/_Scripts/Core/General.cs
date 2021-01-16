using UnityEngine;

public enum SkillName
{
    none, skill0, skill1, skill2, aoeLink, aoeBoom, aoeRow, rebound//aoe - все скилы с уроном по области, Линк цепной урон типа молнии, от одного к другому
}                                                   //Бум это масс урон типа гранаты. Row это урон по линиям, Передняя либо задняя.
public enum SkillNameB
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
    public SkillName skill0;
    public SkillName skill1;
    public SkillName skill2;
    public int availableSkill;

    public int skill0Delay;
    public int skill1Delay;
    public int skill2Delay;


    public int skill_lvl_0 = 0;
    public int skill_lvl_1 = 0;
    public int skill_lvl_2 = 0;
    public int skill_lvl_B = 0;


    public SkillNameB skillB;
    public int skillBDelay;
    public bool availableSkillB = false;



    //Слоты шмота вражеского героя
    public Item head;                                                                   //Слот для шлемов
    public Item tors;                                                                   //Слот для нагрудника
    public Item bots;                                                                   //Слот для ботинок
    public Item weapon;                                                                 //Слот для оружия
    public Item art;                                                                    //Слот для артефакта
}
