using System;

public interface ISwitchAnimator
{
    void InitializeCallback(Action switchOnEvent, Action switchOffEvent);
    void OnAnimation();
    void OffAnimation();
}
