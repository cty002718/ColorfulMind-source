using UnityEngine;

public abstract class QuestObjectActivator : MonoBehaviour
{
	//public GameObject objectToActivate;

	public string[] questToCheck;
	public bool isActive = false;

    public bool CheckActivation() {
    	bool activation = true;
    	for(int i=0;i<questToCheck.Length;i++)
	    	if(!QuestManager.instance.CheckIfComplete(questToCheck[i]))
	    		activation = false;

	    return activation;

    }

    public abstract void OnComplete();
}
