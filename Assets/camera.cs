using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    private float counter = 0;
    private float multiplier = 1;
    private float direction = 0;
    private Vector3 movedPosition = new Vector3();

    // Update is called once per frame
    void Update()
    {
        direction = Mathf.Sin (counter) * multiplier;
        counter += Time.deltaTime;
        var transformRotation = Camera.main.transform.rotation.eulerAngles;
        Debug.Log("transformRotation : " + transformRotation);
        Camera.main.transform.eulerAngles = new Vector3(transformRotation.x, transformRotation.y, direction);
        
        var transformPosition = Camera.main.transform.position;
        movedPosition.x = transformPosition.x;
        movedPosition.y = transformPosition.y;
        movedPosition.z = transformPosition.z + 12f * Time.deltaTime;
        
        Camera.main.transform.position = movedPosition;
    }
}
