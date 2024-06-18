using LiliputhApp.Model;
using MVVMCore;
using System.Windows.Input;

namespace LiliputhApp.ViewModel;

public sealed class MainViewModel : BaseViewModel
{
    private string m_title;
    public string Title { get => m_title; set => SetValue(ref m_title, value); }

    private Dictionary<string, string> m_files;
    public Dictionary<string, string> Files { get => m_files; set => SetValue(ref m_files, value); }

    private bool m_minify;
    public bool MinifyOption { get => m_minify; set => SetValue(ref m_minify, value); }

    public ICommand SelectFilesCommand { get; private set; }
    public ICommand ClearFilesCommand { get; private set; }
    public ICommand SetOutputCommand { get; private set; }
    public ICommand MergeCommand { get; private set; }
    
    public MainViewModel()
    {
        m_title = "Liliputh";

        m_files = new();

        SelectFilesCommand = new Command(async () => await SelectFiles());
        ClearFilesCommand = new Command(() => ClearFiles());
        SetOutputCommand = new Command(() => SetOutput());
        MergeCommand = new Command(() => Merge());
    }

    public async Task SelectFiles()
    {
        IEnumerable<FileResult> files = await FilePicker
            .Default
            .PickMultipleAsync();

        string[] okExtensions = Enum
            .GetValues(typeof(AcceptedExtensions))
            .Cast<AcceptedExtensions>()
            .Select(e => e.ToString().ToLower())
            .ToArray();

        foreach (FileResult file in files)
        {
            string url = file.FullPath;
            string filename = file.FileName;

            string[] fileParts = filename
                .Split('.')
                .Select(s => s.ToLower())
                .ToArray();

            bool isAccepted = okExtensions.Contains(fileParts.Last());
            bool notDuplicate = !Files.ContainsKey(url);

            if(isAccepted && notDuplicate)
            {
                Files.Add(url, filename);
            }

            NotifyPropertyChanged(nameof(Files));
        }
    }

    public void ClearFiles()
    {
        Files.Clear();
        NotifyPropertyChanged(nameof(Files));
    }

    public void SetOutput()
    {
        
    } 

    public void Merge()
    {

    }
}
