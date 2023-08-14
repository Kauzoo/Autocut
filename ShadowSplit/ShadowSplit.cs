
using System;
using System.Diagnostics;
using fftoolslib;

namespace Autocut;

public class ShadowSplit
{
    public static void Main()
    {

    }

    public static void Run(string[] files)
    {
        foreach (var file in files)
        {
            var p1 = new Process();
            // Configure the process using the StartInfo properties.
            p1.StartInfo.FileName = FFmpeg.ffmpegPath;
            p1.StartInfo.Arguments = $"- i {PathHelpers.AddQuotes(file)} -map 0:0 -codec copy %_vp%\%_vn%\vid_%_vn%.mp4";

/* ffmpeg - i "%_fvp%" - map 0:0 - codec copy "%_vp%\%_vn%\vid_%_vn%.mp4"
ffmpeg - i "%_fvp%" - map 0:1 - codec copy "%_vp%\%_vn%\sys_%_vn%.m4a"
ffmpeg - i "%_fvp%" - map 0:2 - codec copy "%_vp%\%_vn%\mic_%_vn%.m4a" */
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
            p1.WaitForExit(); // Waits here for the process to exit.    
        }
    }


}