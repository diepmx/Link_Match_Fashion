using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAnywhere : MonoBehaviour, IPointerClickHandler
{
    public StoryController story;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (story == null) return;
        // nếu đang gõ chữ -> skip, xong rồi thì next (theo đúng logic bạn đang có)
        story.OnClickNext();
    }
}
