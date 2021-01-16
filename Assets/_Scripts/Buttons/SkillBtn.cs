using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBtn : Button
{
    public SkillName skillName;
    public SkillNameB skillNameB;
    public int skill_lvl;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }
}
