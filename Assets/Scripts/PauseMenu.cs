using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject buttons;
    public GameObject manual;
    [SerializeField]
    Scrollbar soundBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
        	if(GameIsPaused) {
        		Resume();
        	} else {
        		Pause();
        	}
        }
    }

    public void Resume() {
    	buttons.SetActive(true);
    	manual.SetActive(false);
    	pauseMenuUI.SetActive(false);
    	Time.timeScale = 1f;
    	GameIsPaused = false;
    }

    void Pause() {
    	pauseMenuUI.SetActive(true);
    	Time.timeScale = 0f;
    	GameIsPaused = true;
    }

    public void LoadMenu() {
    	Time.timeScale = 1f;
    	SceneManager.LoadScene("Start");
    }

    public void QuitGame() {
    	Application.Quit();
    }

    public void SetSound()
    {
        PlayerPrefs.SetFloat("Volume", soundBar.value);
    }

    public void toManual() {
    	buttons.SetActive(false);
    	manual.SetActive(true);
    }

    public void open_manual() {
        pauseMenuUI.SetActive(true);
        buttons.SetActive(false);
        manual.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }


}
