using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceLabel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        int count = this.transform.childCount;
        for (int x = 0; x < count; x++)
        {
            this.transform.GetChild(x).GetComponent<TextMeshProUGUI>().text = Mathf.Abs((this.transform.GetChild(x).position.x) / 10) + " AU";
        }

    }

}
