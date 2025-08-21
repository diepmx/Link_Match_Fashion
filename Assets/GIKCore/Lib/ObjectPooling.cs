using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    private readonly Dictionary<GameObject, List<GameObject>> _listObject = new Dictionary<GameObject, List<GameObject>>();
    public GameObject GetGameObject(GameObject obj)
    {
        if (_listObject.ContainsKey(obj))
        {
            foreach (var go in _listObject[obj].Where(go => !go.activeSelf))
            {
                return go;
            }
            var g = Instantiate(obj, this.transform.position, Quaternion.identity);
            _listObject[obj].Add(g);
            g.SetActive(false);

            return g;
        }

        var list = new List<GameObject>();
        var g2 = Instantiate(obj, this.transform.position, Quaternion.identity);
        list.Add(g2);
        g2.SetActive(false);
        _listObject.Add(obj, list);

        return g2;

    }

    public GameObject GetGameObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        if (_listObject.ContainsKey(obj))
        {
            foreach (var go in _listObject[obj].Where(go => !go.activeSelf))
            {
                go.transform.position = position;
                go.transform.rotation = rotation;
                return go;
            }
            var g = Instantiate(obj, position, rotation);
            _listObject[obj].Add(g);
            g.SetActive(false);

            return g;
        }

        var list = new List<GameObject>();
        var g2 = Instantiate(obj, position, rotation);
        list.Add(g2);
        g2.SetActive(false);
        _listObject.Add(obj, list);

        return g2;

    }
}
