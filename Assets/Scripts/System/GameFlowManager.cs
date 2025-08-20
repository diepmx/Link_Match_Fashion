using UnityEngine;
using System;

public enum GameFlowState
{
    StoryMode,      // Đang hiển thị Ink story
    TaskSelection,  // Chọn task (Panel_Task)
    DressupMode,    // Chọn item (Panel_Clothing)
    StoryTransition // Chuyển cảnh, loading
}

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;
    public GameFlowState CurrentState { get; private set; }
    
    // Events để UI components subscribe
    public static event Action<GameFlowState> OnStateChanged;
    public static event Action<string> OnTaskCompleted;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentState = GameFlowState.StoryMode; // Mặc định bắt đầu với Story
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ChangeState(GameFlowState newState)
    {
        if (CurrentState == newState) return;
        
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
        Debug.Log($"[GameFlowManager] State changed to: {newState}");
    }
    
    public void CompleteTask(string taskId)
    {
        OnTaskCompleted?.Invoke(taskId);
        Debug.Log($"[GameFlowManager] Task completed: {taskId}");
    }
    
    public void ReturnToStory()
    {
        ChangeState(GameFlowState.StoryMode);
    }
}
