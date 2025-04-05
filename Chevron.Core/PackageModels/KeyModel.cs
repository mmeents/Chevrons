using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Chevron.Core.PackageModels
{

  [MessagePackObject]
  public class KeyModel {

    [Key(0)]
    public Guid Id { get; set; } = Guid.Empty;

    [Key(1)]
    public string Name { get; set; } = "";

    [Key(2)]
    public string Value { get; set; } = "";

  }
}
