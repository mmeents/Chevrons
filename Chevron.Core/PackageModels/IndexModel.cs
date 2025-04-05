using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using MessagePack;

namespace Chevron.Core.PackageModels
{
  [MessagePackObject]
  public class IndexModel
  {
      [Key(0)]
      public Guid Id { get; set; } = Guid.Empty; //: Unique identifier for each file.  multiple apps means int is not enough...

      [Key(1)]
      public Guid ParentId { get; set; } = Guid.Empty; //: For folders, the parent folder's Id.

      [Key(2)]
      public Guid KeyId { get; set; } = Guid.Empty; //(Guid): Which key was used to lock the zip.

      [Key(3)]
      public bool IsFolder { get; set; } = false; //(bool): Whether the entry is a folder or a file.

      [Key(4)]
      public string Location { get; set; }=""; //(string)//: Original file path/ Virtual file path.

      [Key(5)]
      public string FileName { get; set; } = "";   // (string): Name of the file.

      [Key(6)]
      public string FileHash { get; set; } = ""; //(string): SHA256 hash to detect duplicates or changes.

      [Key(7)]
      public long FileSize { get; set; } = 0;  //(long): Size of the file(handy for quick checks).

      [Key(8)]
      public DateTime LastModified { get; set; } //(DateTime) : For change detection.

      [Key(9)]
      public string ZipPart { get; set; }=""; //(string, nullable): The name of the zip file(if already archived).

      [Key(10)]
      public bool IsArchived { get; set; }  //(bool) : Whether the file is packed into a zip.

      [Key(11)]
      public string Notes { get; set; } = ""; //(string): 


  }
}
