using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //variables
    [SerializeField] private Text highScore = null;

    // Start is called before the first frame update
    void Start()
    {
        //Convert the saved highscore playerprefs in to String in order to display it
        highScore.text = PlayerPrefs.GetInt("highScore").ToString();

    }

    //Start the Game Scene when Play Button is pressed.
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Quit the game when Exit Button is pressed
    public void ExitGame()
    {
        Application.Quit();
    }
}
