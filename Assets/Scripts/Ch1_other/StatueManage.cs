using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueManage : MonoBehaviour
{
    [SerializeField]
    protected StatueTarget[] targets;
    [SerializeField]
    protected StatueBehavior[] statues;
    bool toMonitor = true;

    // Update is called once per frame
    void Update()
    {
        if (toMonitor)
        {
            bool complete = true;
            foreach (StatueTarget target in targets)
            {
                if (target.OnTarget == false)
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                QuestManager.instance.MarkQuestComplete("Statue");
                toMonitor = false;
                foreach(StatueBehavior statue in statues)
                {
                    statue.canPush = false;
                }
            }
        }
    }
}
