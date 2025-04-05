using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.Core.PackageModels
{
  [MessagePackObject]
  public class SettingsPackage {

    [Key(0)]
    public string Name { get; set; } = "";

    [Key(1)]
    public IEnumerable<SettingProperty> SettingsList { get; set; } = [];

  }
}
