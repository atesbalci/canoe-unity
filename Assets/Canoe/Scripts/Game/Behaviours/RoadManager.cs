using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabList; 
    private List<GameObject> roadList = new List<GameObject>(); 
    private const int ROAD_LENGTH = 164 * 4;
    private int oldCameraPositionIndex;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            roadList.Add(Instantiate(prefabList[i], new Vector3(0, 0, ROAD_LENGTH * i), Quaternion.identity));
        }    
    }

    // Update is called once per frame
    void Update()
    {
        var newCameraPositionIndex = (int) Camera.main.transform.position.z / ROAD_LENGTH;
        if (newCameraPositionIndex != oldCameraPositionIndex)
        {
            roadList[oldCameraPositionIndex % roadList.Count].transform.position = new Vector3(0, 0, roadList[(oldCameraPositionIndex + roadList.Count - 1) % roadList.Count].transform.position.z + ROAD_LENGTH);
        }

        oldCameraPositionIndex = (int) UnityEngine.Camera.main.transform.position.z / ROAD_LENGTH;
    }
}
