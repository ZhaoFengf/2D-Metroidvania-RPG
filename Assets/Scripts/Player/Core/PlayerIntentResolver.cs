using UnityEngine;

public class PlayerIntentResolver
{
    private readonly Camera camera;

    public PlayerIntentResolver(Camera camera)
    {
        this.camera = camera;
    }

    public PlayerIntent Resolve(PlayerInputSnapshot input)
    {
        Vector2 aimWorldPosition = camera.ScreenToWorldPoint(input.MouseScreenPosition);

        return new PlayerIntent(
            input.MoveX,
            input.MoveY,
            input.JumpPressed,
            input.AttackPressed,
            input.DashPressed,
            input.CounterPressed,
            input.BlackHolePressed,
            input.CrystalPressed,
            input.FlaskPressed,
            input.AimPressed,
            input.AimReleased,
            input.QuitGame,
            aimWorldPosition);
    }
}