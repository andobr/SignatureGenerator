namespace SignatureGenerator.Extensions;

public static class ArrayExtensions
{
    public static int IndexOfOrDefault<T>(this T[] array, T value, int defaultValue)
    {
        var index = Array.IndexOf(array, value);
        return index != -1 ? index : defaultValue;
    }   
    
    public static int? IndexOfOrNull<T>(this T[] array, T value)
    {
        var index = Array.IndexOf(array, value);
        return index != -1 ? index : null;
    }   
}