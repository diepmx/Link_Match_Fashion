using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Story Panels")]
    public GameObject storyPanel;
    
    [Header("Task Panels")]
    public GameObject taskPanel;
    
    [Header("Dressup Panels")]
    public GameObject dressupPanel;
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    private void OnEnable()
    {
        GameFlowManager.OnStateChanged += HandleStateChange;
    }
    
    private void OnDisable()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.OnStateChanged -= HandleStateChange;
        }
    }
    
    private void HandleStateChange(GameFlowState newState)
    {
        if (showDebugInfo)
        {
            Debug.Log($"[UIManager] State changed to: {newState}");
        }
        
        switch (newState)
        {
            case GameFlowState.StoryMode:
                ShowStoryPanel();
                break;
            case GameFlowState.TaskSelection:
                ShowTaskPanel();
                break;
            case GameFlowState.DressupMode:
                ShowDressupPanel();
                break;
            case GameFlowState.StoryTransition:
                ShowTransitionPanel();
                break;
        }
    }
    
    private void ShowStoryPanel()
    {
        SetPanelActive(storyPanel, true);
        SetPanelActive(taskPanel, false);
        SetPanelActive(dressupPanel, false);
    }
    
    private void ShowTaskPanel()
    {
        SetPanelActive(storyPanel, false);
        SetPanelActive(taskPanel, true);
        SetPanelActive(dressupPanel, false);
    }
    
    private void ShowDressupPanel()
    {
        SetPanelActive(storyPanel, false);
        SetPanelActive(taskPanel, false);
        SetPanelActive(dressupPanel, true);
    }
    
    private void ShowTransitionPanel()
    {
        // Có thể hiển thị loading screen hoặc transition effect
        Debug.Log("[UIManager] Showing transition panel");
    }
    
    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
    
    // Public methods để test từ Inspector
    [ContextMenu("Test Story Mode")]
    public void TestStoryMode()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ChangeState(GameFlowState.StoryMode);
        }
    }
    
    [ContextMenu("Test Task Selection")]
    public void TestTaskSelection()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ChangeState(GameFlowState.TaskSelection);
        }
    }
    
    [ContextMenu("Test Dressup Mode")]
    public void TestDressupMode()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ChangeState(GameFlowState.DressupMode);
        }
    }
}
