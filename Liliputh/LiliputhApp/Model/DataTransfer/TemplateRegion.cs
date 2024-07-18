namespace LiliputhApp.Model.DataTransfer;

public sealed class TemplateRegion
{
    public string Name { get; set; }
    public string FullString { get; set; }
    public FileExtensions Extension { get; set; }

    public TemplateRegion(string name, string fullString, FileExtensions extension)
    {
        Name = name;
        FullString = fullString;
        Extension = extension;
    }

    public static TemplateRegion Empty(string text = "-------") => new(text, "", FileExtensions.NOTACCEPTED);
}
