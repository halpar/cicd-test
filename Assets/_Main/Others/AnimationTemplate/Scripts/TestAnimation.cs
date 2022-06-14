namespace AnimationTemplate
{
    public class TestAnimation : AnimationController
    {
        private void Start()
        {
            SetTrigger(AnimationType.Attack);
        
            //SetBool();
        
            SetFloat(AnimationType.Speed, 1.5f);
        
            SetInt(AnimationType.State, 0);
        }
    }
}