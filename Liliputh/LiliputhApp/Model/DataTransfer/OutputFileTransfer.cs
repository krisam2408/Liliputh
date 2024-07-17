namespace LiliputhApp.Model.DataTransfer;

internal struct OutputFileTransfer
{
    internal readonly string[] Lines;
    internal readonly string OutputPath;
    internal bool Error;

    internal OutputFileTransfer(List<string> lines, string outputPath)
    {
        Lines = lines.ToArray();
        OutputPath = outputPath;
        Error = false;
    }

    internal static OutputFileTransfer FailedFile => new([], string.Empty)
    {
        Error = true
    };
}
