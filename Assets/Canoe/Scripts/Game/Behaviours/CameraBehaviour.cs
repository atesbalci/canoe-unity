using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private float counter = 0;
    private float multiplier = 1;
    private float direction = 0;
    private Vector3 movedPosition = new Vector3();
    [SerializeField] private GameObject canoe; 

    // Update is called once per frame
    void Update()
    {
        direction = Mathf.Sin (counter) * multiplier;
        counter += Time.deltaTime;
        var transformRotation = UnityEngine.Camera.main.transform.rotation.eulerAngles;
        UnityEngine.Camera.main.transform.eulerAngles = new Vector3(transformRotation.x, transformRotation.y, direction);
        
        var transformPosition = UnityEngine.Camera.main.transform.position;
        movedPosition.x = transformPosition.x;
        movedPosition.y = transformPosition.y;
        movedPosition.z = canoe.transform.position.z - 140;
        
        UnityEngine.Camera.main.transform.position = movedPosition;
    }
}
