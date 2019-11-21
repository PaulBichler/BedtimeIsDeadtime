using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //canvas main components
    public Canvas canvas_main;
    public Button button_start;
    public Button button_credits;

    //canvas credits components
    public Canvas canvas_credits;
    public Button button_credits_back;

    //canvas picks components
    public Canvas canvas_picks;
    public Button button_boy;
    public Button button_girl;
    public Button button_picks_back;
    public Button button_quit;


    // Start is called before the first frame update
    void Start()
    {
        //Add function to the menu buttons
        button_start.onClick.AddListener(PickBoy);
        button_credits.onClick.AddListener(ShowCredits);
        button_credits_back.onClick.AddListener(LeaveCredits);
        button_picks_back.onClick.AddListener(LeavePicks);
        button_boy.onClick.AddListener(PickBoy);
        button_girl.onClick.AddListener(PickGirl);
        button_quit.onClick.AddListener(QuitGame);

        //show main, hide rest
        canvas_main.gameObject.SetActive(true);
        canvas_credits.gameObject.SetActive(false);
        canvas_picks.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void QuitGame()
    {
        Application.Quit();
    }
    
    //Start the actual game
    void StartGame()
    {
        canvas_main.gameObject.SetActive(false);
        canvas_picks.gameObject.SetActive(true);
    }

    //Show credits and hide main menu
    void ShowCredits()
    {
        canvas_main.gameObject.SetActive(false);
        canvas_credits.gameObject.SetActive(true);
    }

    //Hide credits and show main menu
    void LeaveCredits()
    {
        canvas_credits.gameObject.SetActive(false);
        canvas_main.gameObject.SetActive(true);
    }

    //hide picks and show main menu
    void LeavePicks()
    {
        canvas_picks.gameObject.SetActive(false);
        canvas_main.gameObject.SetActive(true);
    }

    //pick boy and start game
    void PickBoy()
    {
        SceneManager.LoadScene("scene", LoadSceneMode.Single);
    }

    //pick girl and start game
    void PickGirl()
    {
        SceneManager.LoadScene("scene", LoadSceneMode.Single);
    }
}
