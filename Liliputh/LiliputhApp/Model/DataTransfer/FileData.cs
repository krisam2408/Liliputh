using System.Text.RegularExpressions;

namespace LiliputhApp.Model.DataTransfer;

public sealed class FileData
{
    private readonly FileExtensions[] m_accepted =
    [
        FileExtensions.TXT,
        FileExtensions.JS,
        FileExtensions.CSS,
        FileExtensions.HTML,
    ];

    private readonly FileExtensions[] m_xml =
    [
        FileExtensions.HTML,
    ];

    private readonly FileExtensions[] m_programming =
    [
        FileExtensions.JS,
    ];

    private const string m_xmlRegionPattern = @"<!--([ ]?){{([ ]?)([aA-zZ0-9]+)([ ]?)}}([ ]?)-->";
    private const string m_programmingRegionPattern = @"//([ ]?){{([ ]?)([aA-zZ0-9]+)([ ]?)}}";

    public Guid Id { get; private set; }
    public string Filename { get; set; }
    public string Url { get; set; }
    public FileExtensions Extension { get; set; }
    public bool IsValid => m_accepted.Contains(Extension);

    public FileData()
    {
        Id = Guid.Empty;
        Filename = "";
        Url = "";
        Extension = FileExtensions.NOTACCEPTED;
    }

    public FileData(FileResult file)
    {
        Id = Guid.NewGuid();
        Filename = file.FileName;
        Url = file.FullPath;

        string? extension = Filename
            .Split('.')
            .Select(s => s.ToLower())
            .LastOrDefault();

        if(extension == null)
        {
            Extension = FileExtensions.NULL;
            return;
        }

        foreach(FileExtensions ext in m_accepted)
        {
            if(extension.ToLower() == ext.ToString().ToLower())
            {
                Extension = ext;
                return;
            }
        }

        Extension = FileExtensions.NOTACCEPTED;
    }

    public async Task<List<TemplateRegion>> CheckTemplateRegions()
    {
        List<TemplateRegion> result = new();

        if (!m_accepted.Contains(Extension))
            return result;

        string[] lines = await File.ReadAllLinesAsync(Url);

        if(m_xml.Contains(Extension))
        {
            List<TemplateRegion> regions = GetXMLRegions(lines);
            result.AddRange(regions);
            return result;
        }

        if(m_programming.Contains(Extension))
        {
            List<TemplateRegion> regions = GetProgrammingRegions(lines);
            result.AddRange(regions);
            return result;
        }

        return result;
    }

    public List<TemplateRegion> MatchRegion(string line)
    {
        List<TemplateRegion> result = new();

        if (!m_accepted.Contains(Extension))
            return result;

        if (m_xml.Contains(Extension))
        {
            List<TemplateRegion> regions = GetXMLRegions(line);
            result.AddRange(regions);
            return result;
        }

        if (m_programming.Contains(Extension))
        {
            List<TemplateRegion> regions = GetProgrammingRegions(line);
            result.AddRange(regions);
            return result;
        }

        return result;
    }

    private static List<TemplateRegion> GetXMLRegions(params string[] lines)
    {
        List<TemplateRegion> result = new();

        Regex regex = new(m_xmlRegionPattern);

        foreach (string line in lines)
        {
            MatchCollection matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                string section = match.Value;
                string name = section
                    .Replace("<!--{{", "")
                    .Replace("}}-->", "")
                    .Trim()
                    .Replace(" ", "")
                    .ToUpper();

                TemplateRegion region = new(name, section, FileExtensions.HTML);
                result.Add(region);
            }
        }

        return result;
    }

    private static List<TemplateRegion> GetProgrammingRegions(params string[] lines)
    {
        List<TemplateRegion> result = new();

        Regex regex = new(m_programmingRegionPattern);

        foreach (string line in lines)
        {
            MatchCollection matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                string section = match.Value;
                string name = section
                    .Replace("//", "")
                    .Trim()
                    .Replace(" ", "")
                    .ToUpper();

                TemplateRegion region = new(name, section, FileExtensions.HTML);
                result.Add(region);
            }
        }

        return result;
    }
}
