using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector3(-1000, 0, 0), new Vector3(1000, 0, 0));
        Debug.DrawLine(new Vector3(0, -1000, 0), new Vector3(0, 1000, 0));
    }
}
