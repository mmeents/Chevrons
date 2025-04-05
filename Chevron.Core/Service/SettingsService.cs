using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.Core.Extensions;
using Chevron.Core.Dictionaries;
using Chevron.Core.PackageModels;
using MessagePack;

namespace Chevron.Core.Service
{
  public interface ISettingsService {
    string FileName { get; set; }
    bool FileLoaded { get; }
    SettingsPackage Package { get; set; }
    Settings Settings { get; set; }

    void Load();
    Task LoadAsync();
    void Save();
    Task SaveAsync();

    string DefaultPath { get; set; }
    string WatchedEventsFileName { get; set; }
    string IndexesFileName { get ; set;}
    bool EncryptZip { get ; set;}
    string EncryptPassword { get ; set; }
    int FileTargetBatchSize { get ; set; }
    long ZipBatchSize { get ; }
  }

  public class SettingsService : ISettingsService {
    public string FileName { get; set; }

    private bool _FileLoaded = false;
    public bool FileLoaded { get { return _FileLoaded; } }
    public SettingsPackage Package { get; set; }
    public SettingsService(bool DoConfirmResults = true) {      
      FileName = Exts.SettingsFileName;

      Package = new SettingsPackage {
        Name = FileName
      };
      if (File.Exists(FileName)) {
        Load();
      }
      if (DoConfirmResults) ConfirmDefaults();
    }   

    public void ConfirmDefaults() { 
      Settings setCheck =  this.Settings;
      setCheck[Ss.WatchedEventsFileName].Value = (setCheck[Ss.WatchedEventsFileName].Value == "" ? 
          Exts.DefaultPath+ "\\WatchedEvents"+Ss.WatchedEventsFileExt : setCheck[Ss.WatchedEventsFileName].Value);
      setCheck[Ss.IndexesFileName].Value = (setCheck[Ss.IndexesFileName].Value == "" ? Exts.DefaultPath+"\\Indexes"+Ss.IndexExt : setCheck[Ss.IndexesFileName].Value);
      setCheck[Ss.DefaultPath].Value = (setCheck[Ss.DefaultPath].Value == "" ?  Exts.DefaultPath : setCheck[Ss.DefaultPath].Value);
      setCheck[Ss.EncryptZip].Value = (setCheck[Ss.EncryptZip].Value == "" ? "false" : setCheck[Ss.EncryptZip].Value);
      setCheck[Ss.EncryptPassword].Value = (setCheck[Ss.EncryptPassword].Value == "" ? "sg"+Guid.NewGuid().ToString() : setCheck[Ss.EncryptPassword].Value);
      this.Settings = setCheck;
    }
    public Settings Settings { 
      get { return new Settings(Package.SettingsList); } 
      set { Package.SettingsList = value.AsList; } }

    public void Load() {
      Task.Run(async () => await this.LoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task LoadAsync() {
      if (File.Exists(FileName)) {        
        var encoded = await FileName.ReadAllTextAsync();
        var decoded = Convert.FromBase64String(encoded.Replace('?', '='));
        this.Package = MessagePackSerializer.Deserialize<SettingsPackage>(decoded);
        _FileLoaded = true;        
      }
    }

    public void Save() {
      Task.Run(async () => await this.SaveAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task SaveAsync() {      
      byte[] WirePacked = MessagePackSerializer.Serialize(this.Package);
      string encoded = Convert.ToBase64String(WirePacked);
      await encoded.WriteAllTextAsync(FileName);                  
    }

    public string DefaultPath { 
      get { return this.Settings[Ss.DefaultPath].Value; }
      set { this.Settings[Ss.DefaultPath].Value = value;}
    }
    public string WatchedEventsFileName { 
      get { return this.Settings[Ss.WatchedEventsFileName].Value; }
      set { this.Settings[Ss.WatchedEventsFileName].Value = value; }
    }
    public string IndexesFileName { 
      get { return this.Settings[Ss.IndexesFileName].Value; }
      set { this.Settings[Ss.IndexesFileName].Value = value; }
    }
    public bool EncryptZip { 
      get { return this.Settings[Ss.EncryptZip].Value.AsBoolean(); }
      set { this.Settings[Ss.EncryptZip].Value = value.ToString(); }
    }
    public string EncryptPassword { 
      get { return this.Settings[Ss.EncryptPassword].Value; }
      set { this.Settings[Ss.EncryptPassword].Value = value; }
    }
    public int FileTargetBatchSize { 
      get { return this.Settings[Ss.FileTargetBatchSize].Value == ""? 25 : this.Settings[Ss.FileTargetBatchSize].Value.AsInt32(); }
      set { this.Settings[Ss.FileTargetBatchSize].Value = value.ToString(); }
    }
    public long ZipBatchSize { get { return (this.FileTargetBatchSize * (1024*1024) ).AsInt64(); } }

  }

  public static class Ss {

    public static string IndexExt = ".inpak";
    public static string WatchedEventsFileExt = ".wepak";

    public static string DefaultPath = "DefaultPath";
    public static string WatchedEventsFileName = "WatchedEventsFileName";
    public static string IndexesFileName = "IndexesFileName";

    public static string FileTargetBatchSize = "FileTargetBatchSize";
    public static string PreZipMaxSizeTimes1M = "25";
    public static string TargetZipPartCount = "TargetZipPartCount";
    public static string EncryptZip = "EncryptZip";
    public static string EncryptPassword = "EncryptPassword";
  }

}


