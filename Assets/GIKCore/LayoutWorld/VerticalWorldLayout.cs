using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode, ExecuteAlways]
public class VerticalWorldLayout : MonoBehaviour
{
    [Header("Layout Settings")]
    public float itemHeight = 1f;          // Chiều cao mỗi item
    public float spacing = 0.2f;           // Khoảng cách giữa các item
    public bool invert = false;            // Sắp từ trên xuống hoặc từ dưới lên

    [Header("Target Settings")]
    public bool autoCollectChildren = true;
    public List<Transform> items = new List<Transform>();

    void OnValidate()
    {
        if (autoCollectChildren)
            CollectChildren();
        ApplyLayout();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            ApplyLayout();
#endif
    }

    public void CollectChildren()
    {
        items.Clear();
        foreach (Transform child in transform)
        {
            items.Add(child);
        }
    }

    public void AddItemForTransform(Transform child)
    {
        items.Add(child);
    }

    public void ApplyLayout()
    {
        for (int i = 0; i < items.Count; i++)
        {
            float yPos = (itemHeight + spacing) * i;
            if (invert)
                yPos = -yPos;

            Vector3 localPos = new Vector3(0f, yPos, 0f);
            items[i].localPosition = localPos;
        }
    }
}
