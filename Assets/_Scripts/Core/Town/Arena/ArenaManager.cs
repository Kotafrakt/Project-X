using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;

public enum AttackPoints
{
    head, tors, pants, bots
};

public class ArenaManager : MonoBehaviour
{
    [HideInInspector]
    public bool isEndurn = false;                  //true если завершен ход
    [SerializeField]
    GameObject game;
    [SerializeField]
    GameObject find;
    [SerializeField]
    Text infoBattle;
    [SerializeField]
    Text hpHero;
    [SerializeField]
    Text hpEnemy;
    [SerializeField]
    Button turne;
    [SerializeField]
    Image heroImg;
    [SerializeField]
    Image enemyImg;
    

    HeroEnemy enemy;                        //Вражесий герой
    [SerializeField]
    PointAttack[] hiroPoints;
    [SerializeField]
    PointAttack[] enemyPoints;
    AttackPoints apHero;
    AttackPoints apEnemy;

    AttackPoints защитаВрага; // переменная для определения что защищал противник
    float защитаЧасти; //Голова, пах и т.д.
    int rChance0;
    int rChance1;
    int rChance2;
    int rChance3;

    public void ExitMenu()
    {
        SceneManager.LoadScene("Main");
    }
    public void ВГород()
    {
        SceneManager.LoadScene("Town");
    }

    public void SelectApHero(AttackPoints ap)
    {
        switch (ap)
        {
            case AttackPoints.head:
                for (int i = 0; i < hiroPoints.Length; i++)
                {
                    if (i != 0)
                        hiroPoints[i].UnSelected();
                }
                apHero = ap;
                break;
            case AttackPoints.tors:
                for (int i = 0; i < hiroPoints.Length; i++)
                {
                    if (i != 1)
                        hiroPoints[i].UnSelected();
                }
                apHero = ap;
                break;
            case AttackPoints.pants:
                for (int i = 0; i < hiroPoints.Length; i++)
                {
                    if (i != 2)
                        hiroPoints[i].UnSelected();
                }
                apHero = ap;
                break;
            case AttackPoints.bots:
                for (int i = 0; i < hiroPoints.Length; i++)
                {
                    if (i != 3)
                        hiroPoints[i].UnSelected();
                }
                apHero = ap;
                break;
        }
    }

    public void SelectApEnemy(AttackPoints ap)
    {
        switch (ap)
        {
            case AttackPoints.head:
                for (int i = 0; i < enemyPoints.Length; i++)
                {
                    if (i != 0)
                        enemyPoints[i].UnSelected();
                }
                apEnemy = ap;
                break;
            case AttackPoints.tors:
                for (int i = 0; i < enemyPoints.Length; i++)
                {
                    if (i != 1)
                        enemyPoints[i].UnSelected();
                }
                apEnemy = ap;
                break;
            case AttackPoints.pants:
                for (int i = 0; i < enemyPoints.Length; i++)
                {
                    if (i != 2)
                        enemyPoints[i].UnSelected();
                }
                apEnemy = ap;
                break;
            case AttackPoints.bots:
                for (int i = 0; i < enemyPoints.Length; i++)
                {
                    if (i != 3)
                        enemyPoints[i].UnSelected();
                }
                apEnemy = ap;
                break;
        }
    }



    void CreateEnemy()
    {
        enemy = CreateHeroEnemy.GenarateHero();
    }

    public void ToGame()
    {
        CreateEnemy();
        hpHero.text = Hero.PARAMS[ARENA_HERO_HP_CURRENT].ToString();
        hpEnemy.text = enemy.PARAMS[ARENA_ENEMY_HP_CURRENT].ToString();
        find.SetActive(false);
        apHero = AttackPoints.head;
        apEnemy = AttackPoints.head;
        game.SetActive(true);
    }

    public void EndTurne()
    {
        isEndurn = true;
        turne.interactable = false;
        if (Random.Range(0, 10) > 4)
        {
            HeroAttrack();
            Invoke("EnemyAttrack", 0.5f);
        }
        else
        {
            EnemyAttrack();
            Invoke("HeroAttrack", 0.5f);
        }
        Invoke("NextTurne", 1.5f);
    }

    void NextTurne()
    {
        heroImg.color = Colors.WhiteColor;
        enemyImg.color = Colors.WhiteColor;
        isEndurn = false;
        turne.interactable = true;
    }

    void МеханикаАтакиИгрока()
    {
        rChance0 = Random.Range(0, 3); //Определяем какую часть защищает компьютер
        switch (rChance0)
        {
            case 0:
                защитаВрага = AttackPoints.head; //защита головы
                break;
            case 1:
                защитаВрага = AttackPoints.tors; //защита торса
                break;
            case 2:
                защитаВрага = AttackPoints.pants; //защита паха
                break;
            case 3:
                защитаВрага = AttackPoints.bots; //защита ног
                break;
        }

        if (apEnemy != защитаВрага) //Условие при варианте, что игрок ударил в не защищаемое место
        {
            rChance1 = Random.Range(0, 124);
            if (rChance1 <= 24) rChance1 = 0;
            else if (rChance1 > 24 && rChance1 <= 49) rChance1 = 1;
            else if (rChance1 > 49 && rChance1 <= 74) rChance1 = 2;
            else if (rChance1 > 74 && rChance1 <= 99) rChance1 = 3;
            else rChance1 = 4;

            switch (rChance1)
            {
                case 0://Кейс Попадание
                    rChance2 = Random.Range(0, 99);
                    if (rChance2 <= 24) rChance2 = 0;
                    else if (rChance2 > 24 && rChance2 <= 49) rChance2 = 1;
                    else if (rChance2 > 49 && rChance2 <= 74) rChance2 = 2;
                    else rChance2 = 3;

                    switch (rChance2)
                    {
                        case 0://удар состоялся
                            infoBattle.text = "удар состоялся\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 1://удар крит
                            infoBattle.text = "удар крит\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 2://удар усиленный крит
                            infoBattle.text = "удар усиленный крит\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 3://удар с дебафами, оглушениями и подобным
                            infoBattle.text = "удар с дебафами, оглушениями и подобным\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 4://удар навредил герою
                            infoBattle.text = "Себя бить то зачем???\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                    }
                    break;
                case 1://Кейс Промах
                    rChance2 = Random.Range(0, 99);
                    if (rChance2 <= 24) rChance2 = 0;
                    else if (rChance2 > 24 && rChance2 <= 49) rChance2 = 1;
                    else if (rChance2 > 49 && rChance2 <= 74) rChance2 = 2;
                    else rChance2 = 3;

                    switch (rChance2)
                    {
                        case 0://Промах
                            infoBattle.text = "По ходу вы промазали\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 1://Нанес уров врагу, хоть и промазал.
                            infoBattle.text = "Удача ваше второе я, при таком промахе, вы умудрились поцарапать врага.\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 2://Нанес уров себе, хоть и промазал.
                            infoBattle.text = "Это же на сколько нужно быть неудачником, чтобы ухерачить в себя, атакуя вражину\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 3://Критический промах, штрафы и т.д.
                            infoBattle.text = "Промахнувшить ты шлепнулся как мешок навоза, потеряв следующий ход\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                    }
                    break;
                case 2://Кейс Уклонения
                    rChance2 = Random.Range(0, 124);
                    if (rChance2 <= 24) rChance2 = 0;
                    else if (rChance2 > 24 && rChance2 <= 49) rChance2 = 1;
                    else if (rChance2 > 49 && rChance2 <= 74) rChance2 = 2;
                    else if (rChance2 > 74 && rChance2 <= 99) rChance2 = 3;
                    else rChance2 = 4;

                    switch (rChance2)
                    {
                        case 0://Сработал уклон
                            infoBattle.text = "Враг мастерски укланился\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 1://Сработал уклон но, урон все равно прошел, либо часть урона
                            infoBattle.text = "Хоть он  и укланялся как мортышка, а поцарапать вам его удалось\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 2://Не сработал уклон
                            infoBattle.text = "Как бы эта обезьяна не уворачивалась, ваш кулак достиг цели\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 3://Не сработал уклон и ещё навредил ему
                            infoBattle.text = "По ходу этот идиот, повредил себе, что-то пытаясь укланиться от вашей атаки\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 4://Не сработал уклон и ещё навредил ему, + вы ему вдарили.
                            infoBattle.text = "Мало того, что вы ему врезали, так он ещё и что-то сломал пока бегал от вас\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                    }
                    break;
                case 3://Кейс Парирования
                    rChance2 = Random.Range(0, 99);
                    if (rChance2 <= 24) rChance2 = 0;
                    else if (rChance2 > 24 && rChance2 <= 49) rChance2 = 1;
                    else if (rChance2 > 49 && rChance2 <= 74) rChance2 = 2;
                    else rChance2 = 3;

                    switch (rChance2)
                    {
                        case 0://Сработало парирование
                            infoBattle.text = "Сработало парирование\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 1://Сработало парирование но, урон все равно прошел, либо часть урона
                            infoBattle.text = "Сработало парирование но, урон все равно прошел, либо часть урона\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 2://Не сработало парирование
                            infoBattle.text = "Не сработало парирование\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 3://Не сработало парирование и ещё навредило тебе
                            infoBattle.text = "Не сработало парирование и ещё навредило тебе\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                    }
                    break;
                case 4://Кейс Блок
                    rChance2 = Random.Range(0, 99);
                    if (rChance2 <= 24) rChance2 = 0;
                    else if (rChance2 > 24 && rChance2 <= 49) rChance2 = 1;
                    else if (rChance2 > 49 && rChance2 <= 74) rChance2 = 2;
                    else rChance2 = 3;

                    switch (rChance2)
                    {
                        case 0://Сработал блок
                            infoBattle.text = "Сработал блок\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 1://Сработал блок но, урон все равно прошел, либо часть урона
                            infoBattle.text = "Сработал блок но, урон все равно прошел, либо часть урона\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 2://Не сработал блок
                            infoBattle.text = "Не сработал блок\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                        case 3://Не сработал блок и ещё навредил тебе
                            infoBattle.text = "Не сработал блок и ещё навредил тебе\n";
                            enemyImg.color = Colors.RedColor;
                            break;
                    }
                    break;
            }
            //enemy.arenaHpCurrent -= Hero.arenaDamageCurrent;
        }
        else     // В элсе вариант атаки при условии, что враг защищает атакуемое место (30%) урон
        {
            infoBattle.text = "Вражина защитил то, куда ты метил\n";
        }
    }
    void HeroAttrack()
    {
        heroImg.color = Colors.WhiteColor;
        switch (apEnemy)
        {
            case AttackPoints.head:
                защитаЧасти = enemy.PARAMS[ARENA_ENEMY_DEFENSE_HEAD_CURRENT];
                break;
            case AttackPoints.tors:
                защитаЧасти = enemy.PARAMS[ARENA_ENEMY_DEFENSE_TORS_CURRENT];
                break;
            case AttackPoints.pants:
                защитаЧасти = enemy.PARAMS[ARENA_ENEMY_DEFENSE_PANTS_CURRENT];
                break;
            case AttackPoints.bots:
                защитаЧасти = enemy.PARAMS[ARENA_ENEMY_DEFENSE_BOTS_CURRENT];
                break;
        }
        МеханикаАтакиИгрока();

        //hpEnemy.text = enemy.arenaHpCurrent.ToString();
    }
    void EnemyAttrack()
    {
        enemyImg.color = Colors.WhiteColor;
        switch (Random.Range(0, 3))
        {
            case 0:
                if (apHero != AttackPoints.head)
                {
                    //Hero.arenaHpCurrent -= enemy.arenaDamage;
                    //infoBattle.text += "пиздюлина получена на " + enemy.arenaDamage + " хп\n";
                    //heroImg.color = Colors.RedColor;
                }
                else
                {
                    //infoBattle.text += "Ха-ха лох\n";
                }
                break;
            case 1:
                if (apHero != AttackPoints.tors)
                {
                    //Hero.arenaHpCurrent -= enemy.arenaDamage;
                    //infoBattle.text += "пиздюлина получена на " + enemy.arenaDamage + " хп\n";
                    //heroImg.color = Colors.RedColor;
                }
                else
                {
                    //infoBattle.text += "Ха-ха лох\n";
                }
                break;
            case 2:
                if (apHero != AttackPoints.pants)
                {
                    //Hero.arenaHpCurrent -= enemy.arenaDamage;
                    //infoBattle.text += "пиздюлина получена на " + enemy.arenaDamage + " хп\n";
                    //heroImg.color = Colors.RedColor;
                }
                else
                {
                    //infoBattle.text += "Ха-ха лох\n";
                }
                break;
            case 3:
                if (apHero != AttackPoints.bots)
                {
                    //Hero.arenaHpCurrent -= enemy.arenaDamage;
                    //infoBattle.text += "пиздюлина получена на " + enemy.arenaDamage + " хп\n";
                    //heroImg.color = Colors.RedColor;
                }
                else
                {
                    //infoBattle.text += "Ха-ха лох\n";
                }
                break;
        }
        //hpHero.text = Hero.arenaHpCurrent.ToString();
    }
}
