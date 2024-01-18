using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

//Note To Viewer :: code is not refactored at all,
//please ignore as I tried to make it as fast as possible with my fulltime job

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GameManager).Name;
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {

        if (instance == null)
        {
            instance = this as GameManager;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private GameObject rollingDie, playerChip;
    public Sprite[] dieSprites;
    public GameObject dieRollAnimated, rollButtonUI, resetButtonUI;
    public static int player1StartWaypoint = 0;
    public static int dieRolledNum = 0;
    public Transform[] waypoints;
    public float moveSpeed = 1f;
    public int waypointIndex = 0;
    public bool startMovingPlayerChip = false;
    private bool isPlayerDownloaded = false;

    public void RollDie() 
    {
        StartRoll();
    }

    private void Update()
    {
        if (!isPlayerDownloaded) return;

        if (waypointIndex > player1StartWaypoint + dieRolledNum)
        {
            startMovingPlayerChip = false;
            player1StartWaypoint = waypointIndex - 1;
        }

        if (startMovingPlayerChip) MovePlayer();
    }
    private void MovePlayer()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            playerChip.transform.position = Vector2.MoveTowards(playerChip.transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);

            if (playerChip.transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }

    #region Async Call
    public async void StartRoll()
    {
        dieRollAnimated.SetActive(true);
        await GetRandomNumApiAsync();
    }

    private async Task GetRandomNumApiAsync()
    {
        int num = await GetRandomValueFromAPI();
        AfterGetRandomSuccess(num);
    }

    private async Task<int> GetRandomValueFromAPI()
    {
        await Task.Delay(3000); //simulating 3sec delay
        int compiledValue = GetRandomN();
        return compiledValue;
    }
    private int GetRandomN()
    {
        int rollNumber = RandomAPI.GetRandomNumber();
        return rollNumber;
    }
    #endregion 

    private void AfterGetRandomSuccess(int num)
    {
        dieRollAnimated.SetActive(false);
        rollingDie.GetComponent<SpriteRenderer>().sprite = dieSprites[num - 1];
        dieRolledNum = num;
        startMovingPlayerChip = true;
        print("You rolled :: " + num);
    }

    public void Reset()
    {
        playerChip.transform.position = waypoints[0].transform.position;
        waypointIndex = 0; 
        player1StartWaypoint = 0;
    }
    public void RollingDieLoadedSuccessfully(GameObject go) 
    {
        rollingDie = go;
        rollButtonUI.SetActive(true);
    }
    public void PlayerChipLoadedSuccessfully(GameObject go)
    {
        playerChip = go;
        playerChip.transform.position = waypoints[waypointIndex].transform.position;
        resetButtonUI.SetActive(true);
        isPlayerDownloaded = true;
    }
}
