using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
	NewDialogueTrigger dialogue;
    bool init = false;
    public AudioController ac;
    // Start is called before the first frame update
    void Start()
    {
    	dialogue = gameObject.GetComponent<NewDialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!init) {
        	init = true;
        	if(dialogue != null) {
        		dialogue.TriggerDialogue();
        	}
        }
    }

    void mute_music(){
        ac.Mute(3);
    }

    void BackToMenu(){
        SceneManager.LoadScene("Start");
    }
}
