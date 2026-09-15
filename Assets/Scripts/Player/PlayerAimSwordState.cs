using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.skill.sword.DotsActive(true);
        player.Ability.SetSwordAimDotsActive(true);
    }
    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(.2f));
    }
    public override void Update()
    {
        base.Update();

        player.Movement.Stop();

        if (player.Intent.AimReleased)
        {
            ChangeState(PlayerStateId.Idle);
            return;
        }
        player.Movement.FaceTargetX(player.Intent.AimWorldPosition.x);

        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //player.Movement.FaceTargetX(mousePosition.x);

        //if (player.transform.position.x > mousePosition.x && player.facingDirection == 1)
        //    player.Flip();
        //else if (player.transform.position.x < mousePosition.x && player.facingDirection == -1)
        //    player.Flip();
    }
}