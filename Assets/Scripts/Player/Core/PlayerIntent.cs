using UnityEngine;
public readonly struct PlayerIntent
{
    //public readonly float MoveDirection;
    public readonly float MoveX;
    public readonly float MoveY;

    public readonly bool WantsJump;
    public readonly bool WantsAttack;
    public readonly bool WantsDash;
    public readonly bool WantsCounter;
    public readonly bool WantsBlackHole;
    public readonly bool WantsCrystal;
    public readonly bool WantsFlask;

    public readonly bool WantsAim;
    public readonly bool AimReleased;

    public readonly bool WantsQuit;

    public readonly Vector2 AimWorldPosition;

    public PlayerIntent(
        float moveX,
        float moveY,
        bool wantsJump,
        bool wantsAttack,
        bool wantsDash,
        bool wantsCounter,
        bool wantsBlackHole,
        bool wantsCrystal,
        bool wantsFlask,
        bool wantsAim,
        bool aimReleased,
        bool wantsQuit,
        Vector2 aimWorldPosition)
    {
        MoveX = moveX;
        MoveY = moveY;

        WantsJump = wantsJump;
        WantsAttack = wantsAttack;
        WantsDash = wantsDash;
        WantsCounter = wantsCounter;
        WantsBlackHole = wantsBlackHole;
        WantsCrystal = wantsCrystal;
        WantsFlask = wantsFlask;

        WantsAim = wantsAim;
        AimReleased = aimReleased;

        WantsQuit = wantsQuit;

        AimWorldPosition = aimWorldPosition;
    }
}