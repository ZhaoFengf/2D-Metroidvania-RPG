public class PlayerAnimation
{
    private readonly Player player;

    public PlayerAnimation(Player _player)
    {
        player = _player;
    }

    public void PlayState(string animationName)
    {
        player.anim.SetBool(animationName, true);
    }

    public void StopState(string animationName)
    {
        player.anim.SetBool(animationName, false);
    }

    public void SetYVelocity(float velocity)
    {
        player.anim.SetFloat("yVelocity", velocity);
    }

    public void SetComboCounter(int counter)
    {
        player.anim.SetInteger("ComboCounter", counter);
    }

    public void SetCounterSuccess(bool success)
    {
        player.anim.SetBool("SuccessfulCounterAttack", success);
    }
}
