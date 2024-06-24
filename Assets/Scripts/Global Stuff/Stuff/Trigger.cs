public struct Trigger
{
    bool isTriggered;

    public static implicit operator bool(Trigger instanse)
    {
        if (instanse.isTriggered)
        {
            instanse.isTriggered = false;
            return true;
        }

        return false;
    }

    public void SetTrigger()
    {
        isTriggered = true;
    }
}

