using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sample : MonoBehaviour
{
    [SerializeField]
    Canvas          canvas;
    [SerializeField]
    ObjectPool      objectPool;
    [SerializeField]
    TextMeshProUGUI DisplayCounter;

    int             counter;

    void Start()
    {
        
    }

    void Update()
    {
        DisplayCounter.SetText($"count: {objectPool.GetUsedCount()} / {objectPool.GetMaxCount()}");

        if (Input.anyKey == true)
        {
            GameObject obj = objectPool.AllocObject();
            if (obj != null)
            {
                StartCoroutine(proc(obj));
            }
        }
    }

    IEnumerator proc(GameObject obj)
    {
        Vector3 v = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), 0);
        obj.transform.SetParent(canvas.transform);
        obj.transform.localPosition = v;

        var text = obj.GetComponent<TextMeshProUGUI>();
        text.SetText($"{counter++}");

        // 3ïbï\é¶ÇµÇΩÇÁè¡Ç∑
        yield return new WaitForSeconds(3);

        objectPool.FreeObject(obj);
    }
}
