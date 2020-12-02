using TMPro;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    public float score;

    [SerializeField]
    Player player;

    [SerializeField]
    GameObject platform;

    [SerializeField]
    private Transform offScreenTop;

    //within players jump range
    private float jumpX = 10f;
    private float jumpY = 3f;

    private Vector2 threadPoint = new Vector2(0f, -9f);

    // is NOT the same as Player like named variables.
    private float offScreenR;
    private float offScreenL;
    private float offScreenDifference = 32;

    // Start is called before the first frame update
    void Start()
    {
        offScreenR = offScreenDifference * 0.5f;
        offScreenL = -offScreenR;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        KeepUpWithPlayer();

        while (threadPoint.y < offScreenTop.position.y)
        {
            SpawnNextPlatform();
        }
    }

    private void KeepUpWithPlayer()
    {
        if (player.transform.position.y > transform.position.y)
        {
            float value = Mathf.Lerp( transform.position.y, player.transform.position.y, 0.1f);
            transform.position = new Vector2(0, value);
            UpdateScore(value);
        }
    }

    private void UpdateScore(float value)
    {
        score = value * 100f;
        scoreText.text = score.ToString("0");
    }

    private void SpawnNextPlatform()
    {
        Vector2 random = RandomPlatformPoint();

        threadPoint += random;

        if (threadPoint.x > offScreenR)
        {
            if (random.x > 8) //8 since when platform goes to opposite side of screen, distance increases by 2, and 10 is around the longest jump I want.
            {
                threadPoint.x = offScreenR;
            }
            else
            {
                threadPoint.x = threadPoint.x - offScreenDifference;
            }            
        }
        else if (threadPoint.x < offScreenL)
        {
            if (random.x < -8)
            {
                threadPoint.x = offScreenL;
            }
            else
            {
                threadPoint.x = threadPoint.x + offScreenDifference;
            }
        }

        Instantiate(platform, threadPoint, Quaternion.identity);
    }

    private Vector2 RandomPlatformPoint()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle;
        Vector2 randomSemiOvalPoint = new Vector2(randomCirclePoint.x * jumpX, Mathf.Abs(randomCirclePoint.y) * jumpY);

        //minimum height increment
        if (randomSemiOvalPoint.y < 1)
        {
            randomSemiOvalPoint.y = 1;
        }
        
        return randomSemiOvalPoint;
    }
}
