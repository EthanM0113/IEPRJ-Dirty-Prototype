using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] GameObject[] extraNodes;
    [SerializeField] GameObject[] extraObjects;
    // Start is called before the first frame update
    void Awake()
    {
        if (extraNodes.Length > 0)
        {
            for (int i = 0; i < extraNodes.Length; i++)
            {
                generateMisc(extraNodes[i], extraObjects[i]);
            }

        }
    }

    private void generateMisc(GameObject Misc, GameObject obj)
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < Misc.transform.childCount; i++)
        {
            list.Add(Misc.transform.GetChild(i));
        }

        for (int i = 0; i < list.Count; i++)
        {
            int range = Random.Range(1, 101);
            if (range > 0 && range < 76)
            {
                Instantiate(obj, list[i]);
                Debug.Log("Created Item");
            }

        }
    }
}
