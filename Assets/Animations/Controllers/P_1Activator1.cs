
public class P_1Activator1 : QuestObjectActivator
{
    public override void OnComplete() {
        gameObject.GetComponent<Portal>().BCanPass = true;
    }
}