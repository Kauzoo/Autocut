namespace ShadowSplit;

public class IO
{
    public static void ReadInputVids()
    {
        // Accept one or more space sperated fully qualified paths
        // Standard rules for delims apply
        // Should also accept folders

        Console.WriteLine("Enter input video file path(s):");
        var paths = Console.ReadLine();
    }
}
