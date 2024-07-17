using MVVMCore;
using System.Collections;

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

    public FileDataItem(FileData file, FileDataViewModel model)
    {
        m_file = file;
        m_model = model;
    }

    public FileDataItem(KeyValuePair<FileData, FileDataViewModel> item)
    {
        m_file = item.Key;
        m_model = item.Value;
    }

    public KeyValuePair<FileData, FileDataViewModel> ToKeyValuePair() => new(File, Model);
}

public sealed class FileDataViewModel : ObservableModel
{
    private string m_region;
    public string Region { get => m_region; set => SetValue(ref m_region, value); }

    private bool m_regionVisibility;
    public bool RegionVisibility { get => m_regionVisibility; set => SetValue(ref m_regionVisibility, value); }
}
