using UnityEngine;
using UnityEngine.UI;

public class PointAttack : MonoBehaviour
{
    public AttackPoints ap;
    public bool isSelected = false;
    public bool isEnemy = false;
    Image image;
    ArenaTown arenaTown;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        /*
        if(Hero.arenaHead != null)
            image.sprite = Hero.arenaHead.img; */
        arenaTown = Camera.main.transform.GetComponent<ArenaTown>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectPoint()
    {
        if (!isSelected && !arenaTown.isEndurn)
        {
            if (isEnemy)
            {
                image.color = Colors.RedColor;
                isSelected = true;
                arenaTown.SelectApEnemy(ap);
            }
            else
            {
                image.color = Colors.GreenColor;
                isSelected = true;
                arenaTown.SelectApHero(ap);
            }
        }
    }

    public void UnSelected()
    {
        image.color = Colors.WhiteColor;
        isSelected = false;
    }
}
