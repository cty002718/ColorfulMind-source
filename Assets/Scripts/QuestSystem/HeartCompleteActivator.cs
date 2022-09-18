public class HeartCompleteActivator : QuestObjectActivator
{
    public override void OnComplete() {
        QuestManager.instance.MarkQuestComplete("HeartComplete");
    }
}