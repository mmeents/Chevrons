using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Chevron.Core.PackageModels
{
  [MessagePackObject]
  public class WatchedEvent
  {
    [Key(0)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Key(1)]
    public Guid WatchedFolderId { get; set; } = Guid.Empty;    

    [Key(2)]
    public int EventTypeId { get; set; }    

    [Key(3)]
    public string SourceLocation { get; set; } = string.Empty;    

    [Key(4)]
    public string FileHash { get; set; } = string.Empty;    

    [Key(5)]
    public string FileName { get; set; } = string.Empty;    

    [Key(6)]
    public long FileSize { get; set; } = 0;
    
    [Key(7)]
    public DateTime FileLastModified { get; set; } = DateTime.UtcNow;
    
    [Key(8)]
    public DateTime EventTime { get; set; } = DateTime.UtcNow;

    [Key(9)]
    public bool IsArchived { get; set; } = false;

  }
}
