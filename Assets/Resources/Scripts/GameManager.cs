using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyPerfect;

public class GameManager : MonoBehaviour
{

    MonoBehaviour _cameraChangeScript;
    MonoBehaviour _randomCharacterPlacerScript;

    Text _ccuTex;

    public int selectedPlayer = 0;

    public int lastSelectedPlayer = 0;

    public bool resetCamera = false;

    public bool pickAnyPlayer = false;

    public float spawnRadius = 20;
    public int spawnAmount = 20;

    public Dictionary<int, int> selectedPlayerDict = new Dictionary<int, int>();

    [ContextMenu("Spawn Animals")]
    void SpawnAnimals()
    {
        ((RandomCharacterPlacer)_randomCharacterPlacerScript).SpawnAnimals(GameObject.Find("People/AIs"), spawnAmount, spawnRadius);
    }

    public void SelectPlayer(int playerId)
    {
        lastSelectedPlayer = selectedPlayer;
        selectedPlayer = playerId;
        if (lastSelectedPlayer != selectedPlayer)
        {
            PlayerPool.GetInstance().ResetDataExcept(selectedPlayer);
        }
    }

    public Player PickAnyPlayer()
    {
        var player = PlayerPool.GetInstance().GetAnyPlayer();
        if (player != null)
        {
            player.MoveController.takeOver = true;
            SelectPlayer(player.InstanceId);
        }
        return player;
    }

    public void DeselectPlayer(int playerId)
    {
        if (playerId == 0)
        {
            selectedPlayer = playerId;
            lastSelectedPlayer = selectedPlayer;
            PlayerPool.GetInstance().ResetDataExcept(playerId);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _cameraChangeScript = GameObject.Find("CameraGroups").GetComponent<CameraChange>();       
        _randomCharacterPlacerScript = GameObject.Find("People").GetComponent<RandomCharacterPlacer>();
        _ccuTex = GameObject.Find("Counter").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectPlayerTask();

        UpdateUITask();
    }

    private void SelectPlayerTask()
    {
        if (resetCamera)
        {
            pickAnyPlayer = false;
            selectedPlayer = 0;
        }

        if (pickAnyPlayer)
        {
            resetCamera = false;
            if (selectedPlayer == 0)
            {
                selectedPlayer = PickAnyPlayer().InstanceId;
            }
        }

        if (selectedPlayer == 0)
        {
            DeselectPlayer(0);
            ((CameraChange)_cameraChangeScript).CameraSwitch(false);
        }
        else
        {
            var player = PlayerPool.GetInstance().GetPlayer(selectedPlayer);
            if (player != null)
            {
                var follower = player.Follower;
                var target = player.MoveController;
                ((CameraChange)_cameraChangeScript).CameraFollow(follower.transform, target.transform, new Vector3(0, 1.8f, 0));
                ((CameraChange)_cameraChangeScript).CameraSwitch(true);
            }
             
        }
    }

    private void UpdateUITask()
    {
        _ccuTex.text = "CCU: " + PlayerPool.GetInstance().CountPlayer();
    }
    

}
