//The game controller responsible for main game mechanics such as moving, score syste, dying and etc
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //variables
    [Header("Screen Bounds")]
    [SerializeField] private int xBound = 0;
    [SerializeField] private int yBound = 0;

    private int score;

    [Header("Food parameters")]
    [SerializeField] private GameObject foodPrefab = null;
    [SerializeField] private GameObject currentFood;
    [SerializeField] private bool isFoodSpawned;

    [Header("Snake parameters")]
    [SerializeField] private GameObject snakePrefab = null;
    [SerializeField] private Snake head;
    [SerializeField] private Snake tail;
    [SerializeField] private int maxSize;
    [SerializeField] private int currentSize;

    [Header("Movement")]
    [SerializeField] private int direction;
    [SerializeField] private Vector2 nextPos;

    [Header("Score")]
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text highestScoreText = null;


    // If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
    public const float MAX_SWIPE_TIME = 0.5f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    public const float MIN_SWIPE_DISTANCE = 0.17f;

    public static bool swipedRight = false;
    public static bool swipedLeft = false;
    public static bool swipedUp = false;
    public static bool swipedDown = false;


    public bool debugWithArrowKeys = true;

    Vector2 startPos;
    float startTime;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Snake.hit += Hit;
    }

    private void OnDisable()
    {
        Snake.hit -= Hit;
    }
    void Start()
    {
        InvokeRepeating("TimerInvoke",0,.5f);
        FoodFunction();
    }

    // Update is called once per frame
    void Update()
    {
        PCDirection();
        TouchInput();

        highestScoreText.text = PlayerPrefs.GetInt("highScore").ToString();

        //set up the score
        if (score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        //Destroy if Apples if there are more than 1
        if(GameObject.FindGameObjectsWithTag("Apple").Length > 1) 
        {
            Destroy(GameObject.FindWithTag("Apple"));
        }
    }

    //Check if the current size >= maxSize and destroy the body parts so the currentSize can be correct
    void TimerInvoke()
    {
        Movement();
        if(currentSize >= maxSize)
        {
            TailFunc();
        }
        else
        {
            currentSize++;
        }
    }

    //Instantiating the snakePrefab and attaching it to the Canvas as the snake is an UI Image
    void Movement()
    {
        
        nextPos = head.transform.position;

        switch (direction)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + .5f);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + .5f, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - .5f);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - .5f, nextPos.y);
                break;
        }

            GameObject temp;

            temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
            temp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

            head.SetNext(temp.GetComponent<Snake>());
            head = temp.GetComponent<Snake>();

        return;
    }

    //PC movement controller using WSAD
    void PCDirection()
    {
        if(direction != 2 && Input.GetKeyDown(KeyCode.W))
        {
            direction = 0;
        }
        if (direction != 3 && Input.GetKeyDown(KeyCode.D))
        {
            direction = 1;
        }
        if (direction != 0 && Input.GetKeyDown(KeyCode.S))
        {
            direction = 2;
        }
        if (direction != 1 && Input.GetKeyDown(KeyCode.A))
        {
            direction = 3;
        }
    }

    //Get next body part and Destroy it when the TailFunc is called.
    void TailFunc()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    //Spawning the Food randomly in the given Bounds
    void FoodFunction()
    {
        float xPos = Random.Range(-xBound, xBound) - .25f;
        float yPos = Random.Range(-yBound, yBound) - .25f;

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        currentFood.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        
    } 

    //Check if the Player is colliding with the Apple, Wall or Himself
    void Hit(string WhatWasSent)
    {
        if (WhatWasSent == "Apple")
        {
            FoodFunction();       
            maxSize++;
            score++;
            scoreText.text = score.ToString();
            SoundManager.PlaySound("eat");
        }
        if (WhatWasSent == "Player" || WhatWasSent == "Wall")
        {
            CancelInvoke("TimerInvoke");
            SoundManager.PlaySound("die");
            StartCoroutine(ExitCoroutine());
        }

        //If the player is colliding with the Upper or Lower Wall 
        //the player's transform.position.y is changed to the opposite one (teleportation)
        if(WhatWasSent == "UpperWall")
        {
            head.transform.position = new Vector2(head.transform.position.x, -head.transform.position.y - 1);
        }
        if(WhatWasSent == "DownWall")
        {
            head.transform.position = new Vector2(head.transform.position.x, -head.transform.position.y - 2);
        }
    }

    /*Exit the game by using Coroutine so the GameOver sound 
    can be played properly before changing the scene*/
    IEnumerator ExitCoroutine()
    {
        SoundManager.PlaySound("die");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    //Touch Screen Input with Arrow keys for debugging
    private void TouchInput()
    {
        swipedRight = false;
        swipedLeft = false;
        swipedUp = false;
        swipedDown = false;

        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
                    return;

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                    return;

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                { // Horizontal swipe
                    if (swipe.x > 0)
                    {
                        swipedRight = true;
                    }
                    else
                    {
                        swipedLeft = true;
                    }
                }
                else
                { // Vertical swipe
                    if (swipe.y > 0)
                    {
                        swipedUp = true;
                    }
                    else
                    {
                        swipedDown = true;
                    }
                }
            }
        }

        if (debugWithArrowKeys)
        {

            if (direction != 2 && (swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                direction = 0;
            }
            if (direction != 3 && (swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                direction = 1;
            }
            if (direction != 0 && (swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                direction = 2;
            }
            if (direction != 1 && (swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                direction = 3;
            }
        }
    }
}
