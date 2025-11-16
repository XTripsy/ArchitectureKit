public readonly struct SLevelRequest : IEvent
{
    public readonly string level;
    public SLevelRequest(string temp_level)
    {
        level = temp_level;
    }
}

public readonly struct SLevelLoad : IEvent
{
    public readonly string level;
    public SLevelLoad(string temp_level)
    {
        level = temp_level;
    }
}