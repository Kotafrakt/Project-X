using UnityEngine;
using UnityEngine.UI;

public class PointAttack : MonoBehaviour
{
    public AttackPoints ap;
    public bool isSelected = false;
    public bool isEnemy = false;
    Image image;
    ArenaManager arenaManager;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        /*
        if(Hero.arenaHead != null)
            image.sprite = Hero.arenaHead.img; */
        arenaManager = Camera.main.transform.GetComponent<ArenaManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectPoint()
    {
        if (!isSelected && !arenaManager.isEndurn)
        {
            if (isEnemy)
            {
                image.color = Colors.RedColor;
                isSelected = true;
                arenaManager.SelectApEnemy(ap);
            }
            else
            {
                image.color = Colors.GreenColor;
                isSelected = true;
                arenaManager.SelectApHero(ap);
            }
        }
    }

    public void UnSelected()
    {
        image.color = Colors.WhiteColor;
        isSelected = false;
    }
}
