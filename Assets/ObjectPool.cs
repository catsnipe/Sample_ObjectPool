using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// 使いまわす prefab
    /// </summary>
    [SerializeField]
    GameObject  Prefab = null;
    /// <summary>
    /// 初期最大数
    /// </summary>
    [SerializeField]
    int         ObjectMax = 100;
    /// <summary>
    /// 初期最大数で足りなければ自動的に追加
    /// </summary>
    [SerializeField]
    bool        AutomaticallyAdd = false;

    static List<GameObject> objects;
    static int              objectSearchIndex;

    static int              usedCount;

    /// <summary>
    /// awake
    /// </summary>
    void Awake()
    {
        objects = new List<GameObject>();

        // 初期数のオブジェクトを用意
        for (int i = 0; i < ObjectMax; i++)
        {
            instantiateObject();
        }

        objectSearchIndex = 0;
    }

    /// <summary>
    /// 使用中のオブジェクト数を取得
    /// </summary>
    public int GetUsedCount()
    {
        return usedCount;
    }
    
    /// <summary>
    /// 使用可能なオブジェクト最大数を取得
    /// </summary>
    public int GetMaxCount()
    {
        return objects.Count;
    }
    
    /// <summary>
    /// オブジェクト取得
    /// </summary>
    public GameObject AllocObject()
    {
        int no = objectSearchIndex;

        // foreach で単純に回すと、ObjectMax が増えるほど検索でコストがかかるので、
        // ちょっとした仕掛けで検索が軽くなるよう実装
        for (int i = 0; i < objects.Count; i++)
        {
            if (++no >= objects.Count)
            {
                no = 0;
            }

            var obj = objects[no];

            if (obj.gameObject.activeSelf == false)
            {
                obj.SetActive(true);
                objectSearchIndex = no;
                usedCount++;

                return obj;
            }
        }

        if (AutomaticallyAdd == true)
        {
            // バッファが足りなかった場合、自動的に増やす
            var newobj = instantiateObject();
            newobj.SetActive(true);
            usedCount++;

            return newobj;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 確保したオブジェクトを解放
    /// </summary>
    /// <param name="obj">確保したオブジェクト</param>
    public void FreeObject(GameObject obj)
    {
        if (obj.gameObject.activeSelf == true)
        {
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);

            usedCount--;
        }
    }

    /// <summary>
    /// object instantiate
    /// </summary>
    GameObject instantiateObject()
    {
        var obj = Instantiate(Prefab, this.transform);
        obj.SetActive(false);
        objects.Add(obj);
        
        return obj;
    }
}
