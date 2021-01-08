using System.Collections;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public AudioClip boom;

    void Start()
    {
    //    AudioSource.PlayClipAtPoint(boom, transform.position);
        Destroy(gameObject, 0.5f);
    }
}
