using RemptyTool.ES_MessageSystem;
using UnityEngine;

public class Ch5ChasingPortalActivator : QuestObjectActivator
{
    private bool completed;
    private bool shown;

    [SerializeField] private Portal portal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (completed && !shown && !ES_MessageSystem.instance.IsDoingTextTask)
        {
            // delay-display some message after all key items collected.
            GetComponent<NewDialogueTrigger>().TriggerDialogue();
            shown = true;
            Destroy(gameObject);
            // activate the portal
            portal.BCanPass = true;
        }
    }

    public override void OnComplete()
    {
        completed = true;
    }
}
