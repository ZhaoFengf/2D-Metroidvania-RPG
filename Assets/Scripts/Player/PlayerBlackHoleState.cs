using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravityScale;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravityScale = player.Movement.GetGravity();
        skillUsed = false;
        stateTimer = flyTime;
        //player.rb.gravityScale = 0f;
        player.Movement.SetGravity(0f);
    }

    public override void Exit()
    {
        base.Exit();

        //player.rb.gravityScale = defaultGravityScale;
        player.Movement.SetGravity(defaultGravityScale);
        player.fx.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            player.Movement.SetVerticalVelocity(15f);
            //player.rb.velocity = new Vector2(0, 15);
        if (stateTimer < 0)
        {
            //player.rb.velocity = new Vector2(0, -.1f);
            player.Movement.SetVerticalVelocity(-.1f);

            //if(player.skill.blackHole.CanUseSkill())
            if(!skillUsed && player.Ability.TryUseBlackHole())
                skillUsed = true;
        }

        //if(player.skill.blackHole.SkillCompleted())
        if(player.Ability.IsBlackHoleCompleted())
        {
            ChangeState(PlayerStateId.Air);
        }

    }
}
