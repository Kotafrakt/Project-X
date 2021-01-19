using UnityEngine;
using UnityEngine.EventSystems;
using static Defines;


public class TurnUnit : TurnBase, IPointerDownHandler, IPointerUpHandler
{
    public Unit unit;
    BossLevel bossLevel;
    GameObject hexPoint;


    float time = 0;
    bool isDown = false;
    bool isShowInfo = false;


    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    void Awake()
    {
        hexPoint = transform.GetChild(1).gameObject;
        text = transform.GetChild(2).transform.GetComponent<TextMesh>();
    }

    void Start()
    {
        bossLevel = Camera.main.transform.GetComponent<BossLevel>();
        animator = GetComponent<Animator>();
        isPlayer = true;
}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDown && Time.timeScale != 0)
            time += 1 * Time.deltaTime / Time.timeScale;
        if (time > 0.5f && !isShowInfo)
        {
            isShowInfo = true;
        }
        if (general.PARAMS[GENERAL_HP_CURRENT] <= 0)
        {
            bossLevel.units.Remove(this);
            bossLevel.unitsCurrent.Remove(this);
            if (bossLevel.units.Count > 0)
            {
                bossLevel.selectedUnit = bossLevel.units[0];
                UnSelected();
                animator.Play("die");
                Destroy(gameObject, 2.1f);
            }
            else
            {
                UnSelected();
                animator.Play("die");
                Invoke("Lose", 2f);
            }           
        }
    }

    void Lose()
    {
        bossLevel.Lose();
        Destroy(gameObject);
    }

    public void Select()
    {
        hexPoint.SetActive(true);
    }

    public void UnSelected()
    {
        hexPoint.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isShowInfo)
        {
            Debug.Log("Hp = " + general.PARAMS[GENERAL_HP_CURRENT] + " Инициатива =" + general.PARAMS[GENERAL_INITIATIVE_CURRENT] + " Лв =" + general.PARAMS[GENERAL_LEVEL]);
            time = 0;
            isShowInfo = false;
        }
        isDown = false;
    }
}
