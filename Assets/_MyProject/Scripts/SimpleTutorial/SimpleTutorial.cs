using System;

public class SimpleTutorial : TutorialImages
{
    public override void Setup(Action _callback)
    {
        Callback = _callback;
        Show();
    }
    
    protected override void Close()
    {
        SceneManager.Instance.LoadMainMenu();
    }
}
