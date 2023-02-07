using System.Text;

using Expense_Tracker_v1._0.Core.Contracts.Services;

using Newtonsoft.Json;

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

    public string cleanFilename(string input, string ext = ".db")
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

    public bool IsExists(string fileName)
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
}
