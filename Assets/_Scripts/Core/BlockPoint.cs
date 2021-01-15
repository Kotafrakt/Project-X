using UnityEngine;

public class BlockPoint : MonoBehaviour
{
    [SerializeField]
    private float hp;
    public float currentHp;
    public Sprite image;
    public bool isDead = false;
    public float damage = 0.3f;

    private void Start()
    {
        currentHp = hp;
    }
    private void Update()
    {
        if (currentHp <= 0)
        {
            isDead = true;
            Destroy(gameObject, 0.1f);
        }
    }
}
