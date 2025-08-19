using UnityEngine;
using UnityEngine.UI;

public class TaskPanelController : MonoBehaviour
{
    [SerializeField] private TaskDefinitionSO task;
    [SerializeField] private Button taskButton;
    [SerializeField] private GameObject panelClothing; // assign Clothing panel root

    private bool unlocked;

    private void Awake()
    {
        if (taskButton != null)
        {
            taskButton.onClick.AddListener(OnClickTask);
        }
    }

    private void OnEnable()
    {
        UpdateInteractable();
    }

    private void UpdateInteractable()
    {
        if (taskButton == null) return;
        // Task panel uses Stars currency (not Coins)
        int requiredStars = Mathf.Max(0, task != null ? task.starsCost : 0);
        taskButton.interactable = !unlocked && PlayerPrefs.GetInt("Stars", 0) >= requiredStars;
    }

    private void OnClickTask()
    {
        if (unlocked) return;
        int requiredStars = Mathf.Max(0, task != null ? task.starsCost : 0);
        // Spend Stars instead of Coins
        var init = InitScript.Instance;
        if (init == null)
        {
            Debug.LogError("InitScript instance not found.");
            return;
        }
        if (!init.SpendStars(requiredStars))
        {
            // TODO: feedback UI: not enough stars
            return;
        }
        unlocked = true;
        OpenClothingPanel();
    }

    private void OpenClothingPanel()
    {
        if (panelClothing != null)
        {
            panelClothing.SetActive(true);
        }
    }
}


