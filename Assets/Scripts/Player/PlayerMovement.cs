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

    public void Move(float direction)
    {
        player.SetVelocity(direction * player.moveSpeed, player.rb.velocity.y);
    }

    public void AirMove(float direction)
    {
        player.SetVelocity(direction * player.moveSpeed * 0.8f, player.rb.velocity.y);
    }

    public void Jump()
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
    }

    public void Dash(float direction)
    {
        player.SetVelocity(player.dashSpeed * direction, 0f);
    }

    public void Stop()
    {
        player.SetZeroVelocity();
    }
}