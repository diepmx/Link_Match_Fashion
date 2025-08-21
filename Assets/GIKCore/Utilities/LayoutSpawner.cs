using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayoutGround
{
    HorizontalLayoutGround,
    VerticalLayoutGround,
    GridLayoutGround
}

[ExecuteAlways]
public class LayoutSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private LayoutGround m_layoutType;
    [SerializeField] private Vector3 spacing = new Vector3(2, 2, 0);
    [SerializeField] private int columnCount = 3;
    [SerializeField] private bool clearBeforeSpawn = true;

    private List<object> _dataList;

    public void SetData(List<object> dataList)
    {
        _dataList = dataList;
        GenerateLayout();
    }

    public void GenerateLayout()
    {
        if (m_prefab == null || _dataList == null) return;

        if (clearBeforeSpawn)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
                children.Add(child);
            foreach (Transform child in children)
#if UNITY_EDITOR
                if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
        }

        for (int i = 0; i < _dataList.Count; i++)
        {
            GameObject obj = Instantiate(m_prefab, transform);
            Vector3 position = Vector3.zero;

            switch (m_layoutType)
            {
                case LayoutGround.HorizontalLayoutGround:
                    position = new Vector3(i * spacing.x, 0, 0);
                    break;

                case LayoutGround.VerticalLayoutGround:
                    position = new Vector3(0, -i * spacing.y, 0);
                    break;

                case LayoutGround.GridLayoutGround:
                    int row = i / columnCount;
                    int col = i % columnCount;
                    position = new Vector3(col * spacing.x, -row * spacing.y, 0);
                    break;
            }

            obj.transform.localPosition = position;

            var item = obj.GetComponent<ILayoutItem>();
            if (item != null)
            {
                item.SetData(_dataList[i]);
            }
            else
            {
                Debug.LogWarning("Prefab thiếu component implement ILayoutItem");
            }
        }
    }
}