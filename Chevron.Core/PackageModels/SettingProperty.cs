using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.Core.PackageModels
{
  [MessagePackObject]
  public class SettingProperty {

    [Key(0)]
    public string Key { get; set; } = "";

    [Key(1)]
    public string Value { get; set; } = "";

  }
}
