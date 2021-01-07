using UnityEngine;

public class ZoomPan : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 0649
    Transform left;
    [SerializeField]
#pragma warning disable 0649
    Transform Rigth;
    [SerializeField]
#pragma warning disable 0649
    Transform Up;
    [SerializeField]
#pragma warning disable 0649
    Transform Down;

    Vector3 touch;
    float zoomMin = 1;
    float zoomMax = 14;
    float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 TouchZeroLastPos = touchZero.position - touchZero.deltaPosition;
            Vector2 TouchOneLastPos = touchOne.position - touchOne.deltaPosition;

            float distTouch = (TouchZeroLastPos - TouchOneLastPos).magnitude;
            float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

            float difference = currentDistTouch - distTouch;

            zoom(difference * 0.01f);
        }

        else if (Input.GetMouseButton(0) && Camera.main.orthographicSize < 9f)
        {
            Vector3 direction = touch - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float newX = Mathf.Clamp(direction.x, left.position.x, Rigth.position.x);
            float newY = Mathf.Clamp(direction.y, Down.position.y, Up.position.y);

            Camera.main.transform.position = Vector3.Lerp(transform.position, new Vector3(newX, newY, Camera.main.transform.position.z), speed * Time.deltaTime);
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float incriment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - incriment, zoomMin, zoomMax);
        if (Camera.main.orthographicSize > 7f)
            Camera.main.transform.position = new Vector3(-18f, 8f, Camera.main.transform.position.z);
    }
}
