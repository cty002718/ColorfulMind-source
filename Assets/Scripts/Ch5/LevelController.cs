using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
	public HeroController hero;
	public GameObject init_position;
    public QuestManager questManager;
	NewDialogueTrigger dialogue;
	bool initDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
    	hero.inventory_open = true;
    	dialogue = gameObject.GetComponent<NewDialogueTrigger>();
        if(SceneInformation.chasing) {
            initDialogue = true;
        	hero.transform.position = init_position.transform.position;
        	hero.lookDirection.x = 1;
            SceneInformation.chasing = false;
        } else if(SceneInformation.maze) {
            initDialogue = true;
            hero.transform.position = init_position.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!initDialogue) {
        	initDialogue = true;
        	if(dialogue != null && !SceneInformation.chasing) {
        		dialogue.TriggerDialogue();
        	}
        }

        if(SceneInformation.maze) {
            questManager.MarkQuestComplete("Stage2_Begin");
            SceneInformation.maze = false;
        }
    }

    void OnStartGame() {
    	hero.inventory_open = false;
    }

    void OnFadeComplete() {
    	SceneManager.LoadScene("Ch5");
    }

    void OnFadeWhiteComplete() {
        PlayerPrefs.SetInt("CH5_Complete", 1);
        SceneManager.LoadScene("End");
    }

    void OnLevelChange() {
        if (SceneInformation.level5)
        {
            PlayerPrefs.SetInt("CH4_Complete", 1);
            SceneManager.LoadScene("Ch5");
        }
        else
        {
            PlayerPrefs.SetInt("CH1_Complete", 1);
            SceneManager.LoadScene("Ch4");
        }
    }
}
