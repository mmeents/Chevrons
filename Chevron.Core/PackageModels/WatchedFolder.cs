using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Chevron.Core.PackageModels
{
  [MessagePackObject]  
  public class WatchedFolder
  {
    [Key(0)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Key(1)]
    public string FolderPath { get; set; } = string.Empty;

    [Key(2)]
    public bool IncludeSubFolders { get; set; } = false;

    [Key(3)]
    public string FileFilter { get; set; } = "*.*";


  }
}
