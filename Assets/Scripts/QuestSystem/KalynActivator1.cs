public class KalynActivator1 : QuestObjectActivator
{
    public override void OnComplete() {
        gameObject.GetComponent<NewDialogueTrigger>().dialoguePath = "Level1/kalyn2";
    }
}