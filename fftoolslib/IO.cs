using System;

namespace fftoolslib;

public class PathHelpers
{
    public static bool ValidateFilePath(ref string? path)
    {
        if (path is null)
        {
            Console.WriteLine("ERROR: Missing Path");
            return false;
        }

        path = RemoveQuotes(path);

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

    public static bool ValidatePath(ref string? path)
    {
        if (path is null)
        {
            Console.WriteLine("ERROR: Missing Path");
            return false;
        }

        path = RemoveQuotes(path);

        if (!Path.Exists(path))
        {
            Console.WriteLine("ERROR: Path does not exist");
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

    public static bool ValidateVidPath(ref string? path)
    {
        if (!ValidateFilePath(ref path))
            return false;

        // TODO Add validation for supported video file extensions
        if (!Path.HasExtension(path))
        {
            Console.WriteLine("ERROR: File or Path does not have an extension");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Removes leading and trailing quotes from a path string if they exist
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string RemoveQuotes(string path)
    {
        if (path[0] == '"' && path[^1] == '"')
        {
            return path[1..^1];
        }
        return path;
    }

    /// <summary>
    /// Add quotes to a path string
    /// This is best practice in order for use in cojunction with ffmpeg
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string AddQuotes(string path)
    {
        if (path[0] != '"')
            path = '"' + path;
        if (path[^1] != '"')
            path += '"';
        return path;
    }
}
