using UnityEngine;
using UnityEngine.UI;

public class TaskPanelController : MonoBehaviour
{
    [SerializeField] private TaskDefinitionSO task;
    [SerializeField] private Button taskButton;
    [SerializeField] private GameObject panelClothing; // assign Clothing panel root

    private bool unlocked;
    private static TaskPanelController currentInstance;

    private void Awake()
    {
        if (taskButton != null)
        {
            taskButton.onClick.RemoveAllListeners();
            taskButton.onClick.AddListener(OnClickTask);
        }
    }

    private void OnEnable()
    {
        UpdateInteractable();
        InitScript.OnStarsChanged += OnStarsChanged;
    }

    private void OnDisable()
    {
        InitScript.OnStarsChanged -= OnStarsChanged;
    }

    // Static method để InkStoryController có thể gọi
    public static void OpenTask(string taskId)
    {
        // Tìm và mở task panel tương ứng
        var taskPanel = FindObjectOfType<TaskPanelController>();
        if (taskPanel != null)
        {
            currentInstance = taskPanel;
            taskPanel.LoadTask(taskId);
        }
        else
        {
            Debug.LogError($"[TaskPanelController] No TaskPanelController found in scene for task: {taskId}");
        }
    }

    private void LoadTask(string taskId)
    {
        // TODO: Load task data từ ScriptableObject dựa trên taskId
        // Hiện tại dùng task đã assign sẵn
        Debug.Log($"[TaskPanelController] Loading task: {taskId}");
        gameObject.SetActive(true);
        
        // Chuyển sang Task Selection mode
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ChangeState(GameFlowState.TaskSelection);
        }
    }

    private void UpdateInteractable()
    {
        if (taskButton == null) return;
        // Task panel uses Stars currency (not Coins)
        int requiredStars = GetRequiredStars();
        int currentStars = InitScript.Instance != null ? InitScript.Instance.GetStars() : PlayerPrefs.GetInt("Stars", 0);
        taskButton.interactable = !unlocked && currentStars >= requiredStars;
    }

    private void OnStarsChanged(int stars)
    {
        UpdateInteractable();
    }

    private void OnClickTask()
    {
        if (unlocked) return;
        int requiredStars = GetRequiredStars();
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
        // Mark one-shot unlock so ClothingPanelGuard can allow opening even if a persistent inspector listener exists
        if (task != null && !string.IsNullOrEmpty(task.taskId))
        {
            PlayerPrefs.SetInt($"TaskUnlocked:{task.taskId}", 1);
            PlayerPrefs.Save();
        }
        OpenClothingPanel();
    }

    private int GetRequiredStars()
    {
        if (task == null) return 0;
        // If starsCost not configured (>0), fallback to coinCost as stars requirement to avoid accidental free open
        int required = task.starsCost > 0 ? task.starsCost : Mathf.Max(0, task.coinCost);
        return required;
    }

    private void OpenClothingPanel()
    {
        // Chuyển sang Dressup Mode
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ChangeState(GameFlowState.DressupMode);
        }

        if (panelClothing != null)
        {
            panelClothing.SetActive(true);
        }
    }
}


