public interface IActionState
{
    bool IsLockAnotherState();
    void SetLock(bool islock);
}

public enum ActionType
{
    Idle,
    Move,
    Attack,
    Damage,
    Knockout,
    Carry,
    Death,
}