namespace Extensions;

public static class RandomExtensions
{
    public static float NextSingle(this Random random, float from, float to)
    {
        return random.NextSingle() * (to - from) + from;
    }
}