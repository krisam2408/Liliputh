namespace LiliputhApp.Model.DataTransfer;

public sealed class FileData
{
    private readonly AcceptedExtensions[] m_okExtensions =
    [
        AcceptedExtensions.TXT,
        AcceptedExtensions.JSON,
        AcceptedExtensions.JS,
        AcceptedExtensions.CSS,
        AcceptedExtensions.HTML,
    ];

    public string Filename { get; set; }
    public string Url { get; set; }
    public AcceptedExtensions Extension { get; set; }
    public bool IsValid => m_okExtensions.Contains(Extension);

    public FileData()
    {
        Filename = "";
        Url = "";
        Extension = AcceptedExtensions.NOTACCEPTED;
    }

    public FileData(FileResult file)
    {
        Filename = file.FileName;
        Url = file.FullPath;

        string? extension = Filename
            .Split('.')
            .Select(s => s.ToLower())
            .LastOrDefault();

        if(extension == null)
        {
            Extension = AcceptedExtensions.NULL;
            return;
        }

        foreach(AcceptedExtensions ext in m_okExtensions)
        {
            if(extension.ToLower() == ext.ToString().ToLower())
            {
                Extension = ext;
                return;
            }
        }

        Extension = AcceptedExtensions.NOTACCEPTED;
    }
}
