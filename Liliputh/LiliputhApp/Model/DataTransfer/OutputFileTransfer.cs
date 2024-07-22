namespace LiliputhApp.Model.DataTransfer;

internal struct OutputFileTransfer
{
    internal readonly string[] Lines;
    internal readonly string Filename;
    internal readonly string Directory;
    internal bool Error;
    internal FileExtensions Extension;

    internal string OutputPath => Path.Combine(Directory, Filename);

    internal OutputFileTransfer(IEnumerable<string> lines, string directory, string filename, FileExtensions extension)
    {
        Lines = lines.ToArray();
        Directory = directory;
        Filename = filename;
        Error = false;
        Extension = extension;
    }

    internal static OutputFileTransfer FailedFile => new([], "", "", FileExtensions.NOTACCEPTED)
    {
        Error = true
    };
}
