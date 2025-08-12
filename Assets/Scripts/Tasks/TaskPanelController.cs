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
        taskButton.interactable = !unlocked && InitScript.Coins >= task.coinCost;
    }

    private void OnClickTask()
    {
        if (unlocked) return;
        if (!InitScript.Instance.SpendCoins(task.coinCost))
        {
            // TODO: flash not enough coins UI
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


