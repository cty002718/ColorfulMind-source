using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [SerializeField]
    protected GameObject InitBoard;
    [SerializeField]
    protected GameObject CHBoard;
    [SerializeField]
    protected GameObject SettingBoard;
    [SerializeField]
    Button ch1, ch4, ch5, end;
    [SerializeField]
    Scrollbar soundBar;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Init"))
        {
            Init();
        }
    }

    private void Init()
    {
        PlayerPrefs.SetInt("Init", 1);
        PlayerPrefs.SetFloat("Volume", soundBar.value);
        PlayerPrefs.SetInt("CH1_Complete", 1);
        PlayerPrefs.SetInt("CH4_Complete", 1);
        PlayerPrefs.SetInt("CH5_Complete", 1);
    }

    public void startGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ToCHBoard()
    {
        InitBoard.SetActive(false);
        CHBoard.SetActive(true);
        if (PlayerPrefs.GetInt("CH1_Complete") == 1)
        {
            ch4.enabled = true;
            ch4.image.sprite = ch1.image.sprite;
            ch4.transform.GetChild(1).GetComponent<Text>().enabled = true;
        }
        if (PlayerPrefs.GetInt("CH4_Complete") == 1)
        {
            ch5.enabled = true;
            ch5.image.sprite = ch1.image.sprite;
            ch5.transform.GetChild(1).GetComponent<Text>().enabled = true;
        }
        if (PlayerPrefs.GetInt("CH5_Complete") == 1)
        {
            end.enabled = true;
            end.image.sprite = ch1.image.sprite;
            end.transform.GetChild(1).GetComponent<Text>().enabled = true;
        }
    }
    public void ToInitBoard()
    {
        CHBoard.SetActive(false);
        SettingBoard.SetActive(false);
        InitBoard.SetActive(true);
    }
    public void ToSettingBoard()
    {
        InitBoard.SetActive(false);
        SettingBoard.SetActive(true);
    }
    public void EndGame()
    {
        Application.Quit();
    }

    public void LoadCh1()
    {
        SceneManager.LoadScene("Ch1");
    }
    public void LoadCh2()
    {
        SceneManager.LoadScene("Ch4");
    }
    public void LoadCh3()
    {
        SceneManager.LoadScene("Ch5");
    }
    public void LoadEnd()
    {
        SceneManager.LoadScene("End");
    }
    public void SetSound()
    {
        PlayerPrefs.SetFloat("Volume", soundBar.value);
    }


    public void toMaze() {
        SceneInformation.maze = true;
        SceneManager.LoadScene("Ch4");
    }

    public void toChasing() {
        SceneInformation.chasing = true;
        SceneManager.LoadScene("Ch5");
    }
}
