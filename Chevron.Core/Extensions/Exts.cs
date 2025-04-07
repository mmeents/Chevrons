namespace Chevron.Core.Extensions {
  public static class Exts {

    public const string CommonPathAdd = "\\PrompterFiles";
    public const string SettingsAdd = "\\ChevronSettings.sft";
    public const string LocalHostName = "app.Local";
    public static string DefaultPath {
      get {
        var DefaultDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Exts.CommonPathAdd;
        if (!Directory.Exists(DefaultDir)) {
          Directory.CreateDirectory(DefaultDir);
        }
        return DefaultDir;
      }
    }
    public static string SettingsFileName { get { return DefaultPath + SettingsAdd; } }

  }
}