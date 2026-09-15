using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    public float XInput { get; private set; }
    public float YInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool CounterPressed { get; private set; }
    public bool BlackHolePressed { get; private set; }
    public bool CrystalPressed { get; private set; }
    public bool FlaskPressed { get; private set; }
    public bool AimPressed { get; private set; }
    public bool AimReleased { get; private set; }
    public bool QuitGame { get; private set; }
    public Vector2 MouseScreenPosition { get; private set; }

    public PlayerInputSnapshot Current
    {
        get
        {
            return new PlayerInputSnapshot(
                XInput,
                YInput,
                JumpPressed,
                AttackPressed,
                DashPressed,
                CounterPressed,
                BlackHolePressed,
                CrystalPressed,
                FlaskPressed,
                AimPressed,
                AimReleased,
                QuitGame,
                MouseScreenPosition);
        }
    }
    public void UpdateInput()
    {
        XInput = Input.GetAxisRaw("Horizontal");
        YInput = Input.GetAxisRaw("Vertical");

        JumpPressed = Input.GetButtonDown("Jump");
        AttackPressed = Input.GetKeyDown(KeyCode.Mouse0);
        DashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        CounterPressed = Input.GetKeyDown(KeyCode.Q);
        BlackHolePressed = Input.GetKeyDown(KeyCode.R);
        CrystalPressed = Input.GetKeyDown(KeyCode.F);
        FlaskPressed = Input.GetKeyDown(KeyCode.Alpha1);

        AimPressed = Input.GetKeyDown(KeyCode.Mouse1);
        AimReleased = Input.GetKeyUp(KeyCode.Mouse1);

        QuitGame = Input.GetKeyDown(KeyCode.Escape);

        MouseScreenPosition = Input.mousePosition;
    }

    //private void Update()
    //{
    //    XInput = Input.GetAxisRaw("Horizontal");
    //    YInput = Input.GetAxisRaw("Vertical");

    //    JumpPressed = Input.GetButtonDown("Jump");
    //    AttackPressed = Input.GetKeyDown(KeyCode.Mouse0);
    //    DashPressed = Input.GetKeyDown(KeyCode.LeftShift);
    //    CounterPressed = Input.GetKeyDown(KeyCode.Q);
    //    BlackHolePressed = Input.GetKeyDown(KeyCode.R);

    //    CrystalPressed = Input.GetKeyDown(KeyCode.F);
    //    FlaskPressed = Input.GetKeyDown(KeyCode.Alpha1);

    //    AimPressed = Input.GetKeyDown(KeyCode.Mouse1);
    //    AimReleased = Input.GetKeyUp(KeyCode.Mouse1);

    //    QuitGame = Input.GetKeyDown(KeyCode.Escape);
    //}
}