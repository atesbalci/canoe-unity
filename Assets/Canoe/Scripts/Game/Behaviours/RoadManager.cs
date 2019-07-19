using System.Collections;
using System.Collections.Generic;
using Canoe.Game.Models.Data;
using Canoe.Screens;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabList; 
    [SerializeField] private TextMeshProUGUI scoreValue; 
    [SerializeField] private TextMeshProUGUI timeValue; 
    [SerializeField] private GameObject gameEndView; 
    private List<GameObject> roadList = new List<GameObject>(); 
    private const int ROAD_LENGTH = 164 * 4;
    private int oldCameraPositionIndex;
    private GameState _gameState;

    [Inject]
    public void Initialize(GameState gameState)
    {
        gameEndView.SetActive(false);
        _gameState = gameState;
        _gameState.OnGameOver += () =>
        {
            gameEndView.SetActive(true);
            gameEndView.transform.Find("scoreValue").GetComponent<TextMeshProUGUI>().SetText(_gameState.Score.ToString());
            DOVirtual.DelayedCall(2, () => { SceneManager.LoadScene(ScreenCodes.Lobby); }, false);
        };
    }
    
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
        scoreValue.SetText(_gameState.Score.ToString());
        timeValue.SetText($"{Mathf.Floor(_gameState.RemainingTime)}");
    }
}
