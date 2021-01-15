using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResBtn : Button
{
    public string Name;
    public IMessage message;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        message.Send(Name);
    }
}
