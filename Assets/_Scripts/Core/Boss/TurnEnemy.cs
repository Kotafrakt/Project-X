using UnityEngine;
using UnityEngine.EventSystems;
using static Defines;

public class TurnEnemy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public General general;
    public TextMesh text;
    BossLevel bossLevel;
    GameObject hexPoint;

    public bool isEndTurn = false;

    float time = 0;
    bool isDown = false;
    bool isShowInfo = false;

    public Animator animator;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isShowInfo)
        {
            Debug.Log("Hp = " + general.PARAMS[GENERAL_HP_CURRENT] + " Инициатива =" + general.PARAMS[GENERAL_INITIATIVE_CURRENT]);
            time = 0;
            isShowInfo = false;
        }
        else
        {
            hexPoint.SetActive(true);
            if (bossLevel.selectedEnemy != this && bossLevel.selectedEnemy != null)
                bossLevel.selectedEnemy.UnSelected();
            bossLevel.selectedEnemy = this;
        }
        isDown = false;
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
    }

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
            //BossLevel.enemyes.Remove(this);
            bossLevel.enemyes.Remove(this);
            bossLevel.enemyesCurrent.Remove(this);
            if (bossLevel.enemyes.Count > 0)
            {
                bossLevel.selectedEnemy = bossLevel.enemyes[0];
            }
            else
            {
                bossLevel.Winner();
            }
            UnSelected();
            animator.Play("die");
            Invoke("Destr", 2f);
        }
    }
    void Destr()
    {
        Destroy(gameObject);
    }

    public void UnSelected()
    {
        hexPoint.SetActive(false);
    }
    public void Select()
    {
        hexPoint.SetActive(true);
    }
}
