using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnBtn : MonoBehaviour, IPointerDownHandler
{
    public int spawnNumber;
    [HideInInspector]
    public bool isSelected = false;
    Level level;
    SpriteRenderer image;

    private void Start()
    {
        level = Camera.main.transform.GetComponent<Level>();
        image = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        level.SetSpawnPoint(this);
    }

    public void SelectPoint()
    {
        image.color = Colors.GreenColor;
    }

    public void UnSelectPoint()
    {
        image.color = Colors.WhiteColor;
    }
}
