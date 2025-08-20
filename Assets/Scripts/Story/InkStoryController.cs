using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;

public class InkStoryController : MonoBehaviour
{
    [Header("Ink Integration")]
    public TextAsset inkJSON;
    public Story story;
    
    [Header("UI References")]
    public TextMeshProUGUI storyText;
    public GameObject choiceContainer;
    public Button choiceButtonPrefab;
    
    [Header("Story Flow")]
    public string currentKnot;
    public bool waitingForTaskCompletion;
    public string waitingTaskId;
    
    private void Start()
    {
        InitializeStory();
        BindExternalFunctions();
        SubscribeToGameFlow();
    }
    
    private void InitializeStory()
    {
        if (inkJSON == null)
        {
            Debug.LogError("[InkStoryController] No ink JSON file assigned!");
            return;
        }
        
        story = new Story(inkJSON.text);
        Debug.Log("[InkStoryController] Story initialized successfully");
    }
    
    private void BindExternalFunctions()
    {
        if (story == null) return;
        
        // Bind function để mở task từ Ink
        story.BindExternalFunction("open_task", (string taskId) => {
            OpenTaskFromStory(taskId);
        });
        
        // Bind function để chờ task completion
        story.BindExternalFunction("wait_for_task", (string taskId) => {
            WaitForTaskCompletion(taskId);
        });
        
        // Bind function để tiếp tục story sau task
        story.BindExternalFunction("continue_story", () => {
            ContinueStoryAfterTask();
        });
        
        // Bind function để thêm stats
        story.BindExternalFunction("add_stat", (string statName, int amount) => {
            AddStoryStat(statName, amount);
        });
        
        Debug.Log("[InkStoryController] External functions bound");
    }
    
    private void SubscribeToGameFlow()
    {
        GameFlowManager.OnTaskCompleted += OnTaskCompleted;
    }
    
    private void OnDestroy()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.OnTaskCompleted -= OnTaskCompleted;
        }
    }
    
    public void StartStory(string startingKnot = "chapter_1")
    {
        if (story == null) return;
        
        currentKnot = startingKnot;
        story.ChoosePathString(startingKnot);
        ContinueStory();
    }
    
    private void ContinueStory()
    {
        if (story == null) return;
        
        // Clear previous choices
        ClearChoices();
        
        // Continue story until we hit a choice or end
        while (story.canContinue)
        {
            string text = story.Continue();
            DisplayStoryText(text);
        }
        
        // Display choices if any
        if (story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
    }
    
    private void DisplayStoryText(string text)
    {
        if (storyText != null)
        {
            storyText.text = text;
        }
        Debug.Log($"[Story] {text}");
    }
    
    private void DisplayChoices()
    {
        if (choiceContainer == null || choiceButtonPrefab == null) return;
        
        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            Choice choice = story.currentChoices[i];
            Button button = Instantiate(choiceButtonPrefab, choiceContainer.transform);
            
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choice.text;
            }
            
            int choiceIndex = i;
            button.onClick.AddListener(() => {
                MakeChoice(choiceIndex);
            });
        }
    }
    
    private void ClearChoices()
    {
        if (choiceContainer == null) return;
        
        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void MakeChoice(int choiceIndex)
    {
        if (story == null) return;
        
        story.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
    
    private void OpenTaskFromStory(string taskId)
    {
        Debug.Log($"[InkStoryController] Opening task: {taskId}");
        
        // Chuyển sang Task Selection mode
        GameFlowManager.Instance.ChangeState(GameFlowState.TaskSelection);
        
        // Mở Panel_Task với taskId cụ thể
        TaskPanelController.OpenTask(taskId);
    }
    
    private void WaitForTaskCompletion(string taskId)
    {
        Debug.Log($"[InkStoryController] Waiting for task completion: {taskId}");
        waitingForTaskCompletion = true;
        waitingTaskId = taskId;
        // Story sẽ pause ở đây, chờ task completion
    }
    
    private void ContinueStoryAfterTask()
    {
        Debug.Log("[InkStoryController] Continuing story after task completion");
        waitingForTaskCompletion = false;
        waitingTaskId = "";
        ContinueStory();
    }
    
    private void OnTaskCompleted(string completedTaskId)
    {
        if (waitingForTaskCompletion && completedTaskId == waitingTaskId)
        {
            Debug.Log($"[InkStoryController] Task {completedTaskId} completed, continuing story");
            ContinueStoryAfterTask();
        }
    }
    
    private void AddStoryStat(string statName, int amount)
    {
        // TODO: Implement story stats system
        Debug.Log($"[InkStoryController] Adding {amount} to {statName}");
    }
    
    // Public method để test từ Inspector
    [ContextMenu("Test Start Story")]
    public void TestStartStory()
    {
        StartStory();
    }
}
