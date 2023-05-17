using System.Diagnostics;

namespace Autocut;

internal static class IO
{
    // TODO Make sure Console Fore- and Background color are always contrasting

    #region ConsoleIO

    internal static void ReadConsole()
    {
        Console.WriteLine("Welcome to Autocut");


        Console.WriteLine("Enter timestamp file path:");
        var tmp_path = Console.ReadLine();
        if (tmp_path is null)
        {
            Console.WriteLine("Missing Timestamp File Path. Exiting");
            return;
        }

        Console.WriteLine("Enter output path:");
        var out_path = Console.ReadLine();
        if (out_path is null)
        {
        }

        run(vid_path, ParseInput(tmp_path));
    }

    internal static string ReadInputVid()
    {
        Console.WriteLine("Enter input video file path:");
        var vidPath = Console.ReadLine();
        if (!ValidateVidPath(vidPath))
            ReadInputVid();
        return vidPath;
    }

    internal static string ReadTimestampFile()
    {
        while (true)
        {
            Console.WriteLine("Enter input video file path:");
            var tmpPath = Console.ReadLine();
            if (tmpPath != null)
            {
                if (!ValidateVidPath(tmpPath)) ReadInputVid();
                return tmpPath;
            }

            Console.WriteLine("Missing Video File Path. Exiting");
        }

        throw new NotImplementedException();
    }

    internal static string ReadOutputPath()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Validation

    private static bool ValidatePath(string? path)
    {
        if (path is null)
        {
            Console.WriteLine("ERROR: Missing Path");
            return false;
        }

        if (!Path.Exists(path) || !File.Exists(path))
        {
            Console.WriteLine("ERROR: File or Path does not exist");
            return false;
        }

        if (!Path.IsPathFullyQualified(path))
        {
            // TODO Add examples of fully qualified paths
            Console.WriteLine("ERROR: Path is not fully qualified");
            return false;
        }

        return true;
    }

    private static bool ValidateVidPath(string? path)
    {
        if (!ValidatePath(path))
            return false;

        // TODO Add validation for supported video file extensions
        if (!Path.HasExtension(path))
        {
            Console.WriteLine("ERROR: File or Path does not have an extension");
            return false;
        }

        return true;
    }

    private static bool ValidateTimestampPath(string path)
    {
    }

    #endregion

    internal static void ParseCommandLineArguments(string[] args)
    {
        /**
         * Planned args:
         * -h --help    :   Show help print
         * -i <Path>    :   Input vid path
         * -o <Path>    :   Output (Folder) path
         * -t <Path>    :   Timestamp file path
         */
        throw new NotImplementedException();
    }
}