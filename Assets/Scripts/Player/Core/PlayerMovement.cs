using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private readonly Player player;

    public PlayerMovement(Player _player)
    {
        this.player = _player;
    }

    public float VerticalVelocity => player.rb.velocity.y;
    public int FacingDirection => player.facingDirection;


    public void Move(float direction)
    {
        player.SetVelocity(direction * player.MoveSpeed, player.rb.velocity.y);
    }

    public void AirMove(float direction)
    {
        player.SetVelocity(direction * player.MoveSpeed * 0.8f, player.rb.velocity.y);
    }

    public void Jump()
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.JumpForce);
    }

    public void Dash(float direction)
    {
        player.SetVelocity(player.DashSpeed * direction, 0f);
    }

    public void WallSlide(float verticalMultiplier)
    {
        Vector2 velocity = player.rb.velocity;

        player.rb.velocity = new Vector2(
            0f,
            velocity.y * verticalMultiplier);
    }

    public void WallJump()
    {
        player.SetVelocity(5 * -player.facingDirection, player.JumpForce);
    }

    public void SetVerticalVelocity(float velocity)
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, velocity);
    }

    public void SetHorizontalVelocity(float velocity)
    {
        player.rb.velocity = new Vector2(velocity, player.rb.velocity.y);
    }

    public void SetGravity(float gravityScale)
    {
        player.rb.gravityScale = gravityScale;
    }

    public float GetGravity()
    {
        return player.rb.gravityScale;
    }
    public void FaceDirection(float direction)
    {
        if (direction == 0f)
            return;

        if (Mathf.Sign(direction) != player.facingDirection)
            player.Flip();
    }

    public void FaceTargetX(float targetX)
    {
        FaceDirection(targetX - player.transform.position.x);
    }


    public void Stop()
    {
        player.SetZeroVelocity();
    }

    public void AttackMove(float x, float y)
    {
        player.SetVelocity(x, y);
    }

}