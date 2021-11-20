using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// �g���܂킷 prefab
    /// </summary>
    [SerializeField]
    GameObject  Prefab = null;
    /// <summary>
    /// �����ő吔
    /// </summary>
    [SerializeField]
    int         ObjectMax = 100;
    /// <summary>
    /// �����ő吔�ő���Ȃ���Ύ����I�ɒǉ�
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

        // �������̃I�u�W�F�N�g��p��
        for (int i = 0; i < ObjectMax; i++)
        {
            instantiateObject();
        }

        objectSearchIndex = 0;
    }

    /// <summary>
    /// �g�p���̃I�u�W�F�N�g�����擾
    /// </summary>
    public int GetUsedCount()
    {
        return usedCount;
    }
    
    /// <summary>
    /// �g�p�\�ȃI�u�W�F�N�g�ő吔���擾
    /// </summary>
    public int GetMaxCount()
    {
        return objects.Count;
    }
    
    /// <summary>
    /// �I�u�W�F�N�g�擾
    /// </summary>
    public GameObject AllocObject()
    {
        int no = objectSearchIndex;

        // foreach �ŒP���ɉ񂷂ƁAObjectMax ��������قǌ����ŃR�X�g��������̂ŁA
        // ������Ƃ����d�|���Ō������y���Ȃ�悤����
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
            // �o�b�t�@������Ȃ������ꍇ�A�����I�ɑ��₷
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
    /// �m�ۂ����I�u�W�F�N�g�����
    /// </summary>
    /// <param name="obj">�m�ۂ����I�u�W�F�N�g</param>
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
