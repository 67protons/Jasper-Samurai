using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuSelection : MonoBehaviour 
{
    public GameObject startMenu;
    public GameObject mainMenu;

    private bool onStartMenu = true;


    void Start()
    {
        getStartMenu();
    }

    void Update()
    {
        if(Input.GetKeyDown("return") && onStartMenu == true)
        {
            pressStart();
           
        }
    }

	public void pressStart()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        onStartMenu = false;
    }

    public void getStartMenu()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
        onStartMenu = true;
    }

    public void startGame()
    {        
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
