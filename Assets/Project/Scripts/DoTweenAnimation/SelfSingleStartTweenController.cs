namespace Project.UI
{
    public class SelfSingleStartTweenController : SelfSingleTweenController
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            Play();
        }
    }
}