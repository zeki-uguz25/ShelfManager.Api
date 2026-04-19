namespace Core.Extensions;

public static class BooleanExtensions
{
    public static void IfTrueThrow<TException>(this bool condition, Func<TException> ex)
        where TException : Exception
    {
        if (condition)
            throw ex();
    }
}
