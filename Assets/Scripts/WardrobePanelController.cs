using UnityEngine;

public class WardrobePanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel; // Panel muốn bật/tắt

    // Bật panel
    public void OpenPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    // Tắt panel
    public void ClosePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // Đảo trạng thái bật/tắt
    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }
}
