
namespace PSMemo;

internal static class Constants
{
    internal const string PSMemoFolderName = "psmemo";

    internal static string PSMemoFolderPath
    {
        get
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Join(userFolder, PSMemoFolderName);
        }
    }
}