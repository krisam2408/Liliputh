using MVVMCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LiliputhApp.Model.DataTransfer;

public sealed class FileDataDictionary : ObservableDictionary<FileData, FileDataViewModel>, IEnumerable<FileDataItem>
{
    public FileDataDictionary() : base()
    {

    }

    public new FileDataItem this[FileData key]
    {
        get
        {
            FileDataViewModel value = base[key];
            return new FileDataItem(key, value);
        }
        set
        {
            base[value.File] = value.Model;
        }
    }

    public bool HasSingleExtension(out FileExtensions extension)
    {
        extension = FileExtensions.NULL;

        List<FileExtensions> extensions = new();

        foreach(KeyValuePair<FileData, FileDataViewModel> kvp in Collection)
        {
            if (!extensions.Contains(kvp.Key.Extension))
                extensions.Add(kvp.Key.Extension);
        }

        if(extensions.Count == 1)
        {
            extension = extensions[0];
            return true;
        }

        return false;
    }

    public void Add(FileDataItem item) => Add(item.ToKeyValuePair());

    public bool Contains(FileDataItem item) => ContainsKey(item.File);

    public new IEnumerator<FileDataItem> GetEnumerator()
    {
        foreach(KeyValuePair<FileData, FileDataViewModel> kvp in Collection)
        {
            FileDataItem item = new(kvp);
            yield return item;
        }
    }

    public bool Remove(Guid fileId)
    {
        foreach(KeyValuePair<FileData, FileDataViewModel> kvp in Collection)
        {
            if(kvp.Key.Id == fileId)
                return Remove(kvp.Key);
        }

        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator(); 
    }
}

public sealed class FileDataItem : ObservableModel
{
    private FileData m_file;
    public FileData File { get => m_file; set => SetValue(ref m_file, value); }

    private FileDataViewModel m_model;
    public FileDataViewModel Model { get => m_model; set => SetValue(ref m_model, value); }

    public ICommand RemoveItemCommand { get; private set; }

    public FileDataItem(FileData file)
    {
        m_file = file;
        m_model = new();

        RemoveItemCommand = new Command(RemoveThisItem);
    }

    public FileDataItem(FileData file, FileDataViewModel model) : this(file)
    {
        m_model = model;
    }

    public FileDataItem(KeyValuePair<FileData, FileDataViewModel> item) : this(item.Key, item.Value) { }

    public KeyValuePair<FileData, FileDataViewModel> ToKeyValuePair() => new(File, Model);

    private void RemoveThisItem()
    {
        Terminal.Instance?.Main?.RemoveFile(File.Id);
    }
}

public sealed class FileDataViewModel : ObservableModel
{
    private bool m_regionVisibility;
    public bool RegionVisibility { get => m_regionVisibility; set => SetValue(ref m_regionVisibility, value); }

    private ObservableCollection<TemplateRegion> m_regions;
    public ObservableCollection<TemplateRegion> Regions { get => m_regions; set => SetValue(ref m_regions, value); }

    private int m_selectedRegion;
    public int SelectedRegion { get => m_selectedRegion; set => SetValue(ref m_selectedRegion, value); }

    public FileDataViewModel()
    {
        m_regions = new()
        {
            TemplateRegion.Empty()
        };
        m_regionVisibility = false;
        SelectedRegion = 0;
    }

    public void AddRegions(IEnumerable<TemplateRegion> regions)
    {
        foreach (TemplateRegion region in regions)
        {
            if(!Regions.Contains(region))
                Regions.Add(region);
        }
    }

    public void ClearRegions()
    {
        Regions.Clear();
        Regions.Add(TemplateRegion.Empty());
        SelectedRegion = 0;
    }

    public void CheckRegions(IEnumerable<TemplateRegion> regions)
    {
        ClearRegions();
        AddRegions(regions);
    }
}
