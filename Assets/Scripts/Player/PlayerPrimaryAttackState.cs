using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastAttackTime = 0f;
    private float comboWindow = 1f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(0, null);//播放音效，其中的0对应对应的音频索引下标

        if (Time.time - lastAttackTime > comboWindow || comboCounter > 2)
            comboCounter = 0;

        //player.anim.SetInteger("ComboCounter", comboCounter);
        player.Animation.SetComboCounter(comboCounter);

        float attackDirection = player.facingDirection;
        if(player.PlayerInput.XInput != 0)
            attackDirection = player.PlayerInput.XInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();


        player.StartCoroutine(player.BusyFor(.12f));
        comboCounter++;
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            player.Movement.Stop();

        if (triggerCalled)
            stateMachine.ChangeState(PlayerStateId.Idle);
    }
}
