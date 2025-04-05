using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.Core.PackageModels
{
    [MessagePackObject]
    public class IndexPackage
    {

      [Key(0)]
      public IEnumerable<SettingProperty> SettingsList { get; set; } = [];
    

      [Key(1)]
      public IEnumerable<KeyModel> KeyList { get; set; } = [];
    

      [Key(2)]
      public IEnumerable<IndexModel> IndexList { get; set; } = [];

    }
}
