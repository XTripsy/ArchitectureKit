namespace Namespace_Level
{
    internal readonly struct LevelRequest : IEvent
    {
        public readonly string level;
        public LevelRequest(string temp_level)
        {
            level = temp_level;
        }
    }

    internal readonly struct LevelLoad : IEvent
    {
        public readonly string level;
        public LevelLoad(string temp_level)
        {
            level = temp_level;
        }
    }
}