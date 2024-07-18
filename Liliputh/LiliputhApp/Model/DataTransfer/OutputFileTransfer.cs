namespace LiliputhApp.Model.DataTransfer;

internal struct OutputFileTransfer
{
    internal readonly string[] Lines;
    internal readonly string OutputPath;
    internal bool Error;
    internal FileExtensions Extension;

    internal OutputFileTransfer(IEnumerable<string> lines, string outputPath, FileExtensions extension)
    {
        Lines = lines.ToArray();
        OutputPath = outputPath;
        Error = false;
    }

    internal static OutputFileTransfer FailedFile => new([], "", FileExtensions.NOTACCEPTED)
    {
        Error = true
    };
}
