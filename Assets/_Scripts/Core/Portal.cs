using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroPortal", .6f);
    }

    void DestroPortal()
    {
        Destroy(gameObject);
    }
}
