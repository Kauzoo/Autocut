using fftoolslib;

namespace ShadowSplit;

public class IO
{
    public enum Option
    {
        None, Subfolder
    }

    public struct FileOptionPair
    {
        string path;
        FileExtension fileExtension;
    }

    public struct PathOptionPair
    {
        public string path;
        public Option[] Options;
    }

    public static void ReadInputVids()
    {
        // Accept one or more space sperated fully qualified paths
        // Standard rules for delims apply
        // Should also accept folders

        Console.WriteLine("Enter input video file path(s):");
        try
        {
            var paths = PathHelpers.SplitPaths(Console.ReadLine());
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex);
        }
    }

    public static void ParseArguments()
    {
        /**
            -r: Search trough subdirectories
            -f:<extension>: Filter for specified file extension (maybe later)
        */
    }
}


