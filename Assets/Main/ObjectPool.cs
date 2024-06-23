using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                StartCoroutine(DestroyEffect(2f, obj));
                return obj;
            }
        }

        // If all objects are in use, create a new one (optional)
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    private IEnumerator DestroyEffect(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        ReturnObject(obj);
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
