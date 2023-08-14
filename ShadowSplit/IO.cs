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
            ReadInputVids();
        }
    }

    public static void ParseArguments(string[] arguments)
    {
        // TODO Add options parsing
        /**
            -r: Search trough subdirectories
            -f:<extension>: Filter for specified file extension (maybe later)
        */
        List<string> args = new List<string>();
        string current_filter_extension = ".mp4";
        bool include_sub_dir = false;
        for (int i = 0; i < arguments.Length; i++)
        {
            /* if (arguments[i] == "-r")
            {
                include_sub_dir = true;
                i++;
            } */

            if (Directory.Exists(arguments[i]))
            {
                args.AddRange(Directory.EnumerateFiles(arguments[i], "*.mp4"));
                continue;
            }
            if (Path.GetExtension(arguments[i]) == current_filter_extension)
            {
                args.Add(arguments[i]);
            }
        }
    }
}


