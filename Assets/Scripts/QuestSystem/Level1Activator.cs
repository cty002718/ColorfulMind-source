using ColorSystem;

public class Level1Activator : QuestObjectActivator
{
    public override void OnComplete() {
        gameObject.GetComponent<NewDialogueTrigger>().dialoguePath = "Level1/kalyn4";
        ColorProfile.instance.IsBlueUnlocked = true;
    }
}