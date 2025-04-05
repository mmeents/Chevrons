using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Chevron.Core.PackageModels
{
  [MessagePackObject]
  public class WatchedEventsPackage
  {
    [Key(0)]
    public IEnumerable<WatchedFolder> WatchedFolderList { get; set; } = new List<WatchedFolder>();

    [Key(1)]
    public IEnumerable<WatchedEvent> WatchedEventList { get; set; } = new List<WatchedEvent>();

  }
}
