using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

using Expense_Tracker_v1._0.Core.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;
using Newtonsoft.Json;
using Windows.Storage.Search;

namespace Expense_Tracker_v1._0.Core.Services;

public class FileService : IFileService
{
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }

    public string CleanFileName(string input, string ext = ".db")
    {
        if (input.LastIndexOf('.') == -1)
        {
            return input + ".db";
        }
        else
        {
            return input;
        }
    }

    public static string StripExtension(string input) => input.Substring(0, input.LastIndexOf('.'));

    public bool Exists(string fileName)
    {
        var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);
        if (File.Exists(path))
        {
            return true;
        } else
        {
            return false;
        }
    }
    //split this function so that the pool stuff is in pool.cs and the file handling is here.
    public static List<Pool> GetPoolsList()
    {
        List<Pool> pools = new List<Pool>();
        StorageFolder path = ApplicationData.Current.LocalFolder;
        StorageFileQueryResult results = path.CreateFileQuery();

        var filesInFolder = results.GetFilesAsync();
        foreach (var item in filesInFolder.GetResults())
        {
            Pool p = new Pool(item.DisplayName, item.DateCreated.DateTime);
            pools.Add(p);
        }
        return pools;
    }
}
