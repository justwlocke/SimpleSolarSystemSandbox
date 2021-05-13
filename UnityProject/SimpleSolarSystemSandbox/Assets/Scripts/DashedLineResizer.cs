using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashedLineResizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Resize(float cameraSize)
    {
        //This must be 50 when the cameraSize is 10 and 200 when it is maxed at 100
        float OldRange = 100 - 10;
        float NewRange = 200 - 50;
        float NewValue = (((cameraSize - 10) * NewRange) / OldRange) + 50;

        this.transform.localScale = new Vector3(NewValue, 0.1f, 1);


        //This must be 1 when the cameraSize is 10 and 5 when it is maxed at 100
        OldRange = 100 - 10;
        NewRange = 5 - 1;
        NewValue = (((cameraSize - 10) * NewRange) / OldRange) + 1;
        this.GetComponent<SpriteRenderer>().size = new Vector2(20, NewValue);
    }
}
