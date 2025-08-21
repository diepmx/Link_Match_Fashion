using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class WorldGridLayout : MonoBehaviour
{
    [Header("Config")]
    public Vector2 cellSize = new Vector2(1f, 1f); // Kích thước mỗi ô
    public Vector2 spacing = new Vector2(0.2f, 0.2f); // Khoảng cách giữa các ô
    public int columnCount = 3; // Số cột
    public bool autoUpdate = true;

    [Header("Targets")]
    public List<Transform> items = new List<Transform>();
    public void ApplyLayout()
    {
        for (int i = 0; i < items.Count; i++)
        {
            int row = i / columnCount;
            int column = i % columnCount;

            Vector3 position = new Vector3(
                column * (cellSize.x + spacing.x),
                0f,
                -row * (cellSize.y + spacing.y)
            );

            items[i].localPosition = position;
        }
    }

    public void RefreshLayout()
    {
        ApplyLayout();
    }


    void Update()
    {
        if (autoUpdate)
        {
            ApplyLayout();
        }
    }
}
