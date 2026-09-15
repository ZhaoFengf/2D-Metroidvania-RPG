using UnityEngine;
public readonly struct PlayerInputSnapshot
{
    public readonly float MoveX;
    public readonly float MoveY;

    public readonly bool JumpPressed;
    public readonly bool AttackPressed;
    public readonly bool DashPressed;
    public readonly bool CounterPressed;
    public readonly bool BlackHolePressed;
    public readonly bool CrystalPressed;
    public readonly bool FlaskPressed;

    public readonly bool AimPressed;
    public readonly bool AimReleased;

    public readonly bool QuitGame;

    public readonly Vector2 MouseScreenPosition;

    public PlayerInputSnapshot(
        float moveX,
        float moveY,
        bool jumpPressed,
        bool attackPressed,
        bool dashPressed,
        bool counterPressed,
        bool blackHolePressed,
        bool crystalPressed,
        bool flaskPressed,
        bool aimPressed,
        bool aimReleased,
        bool quitGame,
        Vector2 mouseScreenPosition)
    {
        MoveX = moveX;
        MoveY = moveY;

        JumpPressed = jumpPressed;
        AttackPressed = attackPressed;
        DashPressed = dashPressed;
        CounterPressed = counterPressed;
        BlackHolePressed = blackHolePressed;
        CrystalPressed = crystalPressed;
        FlaskPressed = flaskPressed;

        AimPressed = aimPressed;
        AimReleased = aimReleased;

        QuitGame = quitGame;

        MouseScreenPosition = mouseScreenPosition;
    }
}