using UnityEngine;

public class DamageText : MonoBehaviour
{
    bool move;
    public int speed = 2;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!move) return;
        transform.Translate(Vector2.up * speed * Time.deltaTime, Space.World);
    }

    public void startMitonUnit(string text, Color color)
    {
        GetComponent<TextMesh>().color = color;
        GetComponent<TextMesh>().text = text;
        move = true;
        Invoke("DestroyThis", 3f);
    }
    public void startMitonEnemy(string text, Color color)
    {
        GetComponent<TextMesh>().color = color;
        GetComponent<TextMesh>().text = text;
        transform.Rotate(0, 180, 0);
        move = true;
        Invoke("DestroyThis", 3f);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
