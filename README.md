# Autocut v0.1.0

This is a Tool powered by ffmpeg intended to automate the extraction of video segments based of Timestamps.
More features to come (probably).

For usage info see Misc/README.txt (comes bundled with Release).

Autocut v0.1.0

@Author Nyr
Git Repo:	https://github.com/Kauzoo/Autocut
Discord:	Kauzoo#2997

This is a Tool powered by FFmpeg intended to automate the extraction of video segments based of Timestamps. More features to come (probably).
For now Autocut (Release) comes bundled with a prebuilt FFmpeg executable (for which I probably don't pass the LGPL Requirements rn, oops),
but this might change in the future. (Also I'll have to sort out the license Issue).
I have only tested this on Windows 10 for now using .NET 7.0. This should probably be downward compatible but idk.
Support for Linux is planned down the line as well as more features and bugfixes.
If you have a bug to report / feature request please open a Issue on GitHub.
If you have questions / need help contact me on Discord.
Recommended / Required: .NET 7.0



Usage
-Required Parameters:	<InputVideo>	<TimespampFile>		<OutputPath>
-For all required parameters a full path should be provided
-InputVideo:	Full path to a video file from which you want to extract segments
-TimestampFile:	Files are expected as .tsv files (Google Sheets can be exported as .tsv)
-OutputPath:	Full path of a directory into which the output files should be written
(-Pro Tip: If you Drag&Drop Files onto a Terminal Window the Path to that file will automatically be copied)


Timespamp File (Structure)
<Filename/Path> <Start> <End>

A Timestamp file is made up of three values per row.
First Column <Filename/Path>:		Either a the filename that should be used for the generated Segment output file or a fully qualified path.
									When only a Filename is entered the output will be saved to the provided output directory.
									If a FullyQualifiedPath is entered Autocut will attempt to use it as ouput location.
									Filenames or Paths must not include tabs.
									(Note: FullyQualifiedPaths need to match the extension of the input video file for now)
								
Second Column <Start-Timespamp>:	Timestamp signifying the start of a segment that sould be extracted. Needs to be lower than End-Timespamp.
									(See Timestamp (Structue))
									
Third Column <End-Timespamp>:		Timestamp signifying the start of a segment that sould be extracted. Needs to be higher than Start-Timespamp.
									(See Timestamp (Structue))
									
	
	
Timestamp (Structue)
<HH>:<MM>:<SS>

A Timestamp is made up of Hours, Minutes and Seconds.



Example (TimestampFile):	
Nyr-Vs-Woodzie	00:19:15	00:32:00
Cyber-Vs-Ford	00:35:40	00:45:21
"D:\Videos\Riders-Underground\TourneyVids\Roxim-Vs-Ludi.mp4"	02:16:31	02:27:59

(see also provided ExmapleTimestampsFile.png)
