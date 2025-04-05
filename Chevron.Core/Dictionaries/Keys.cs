using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.Core.PackageModels;

namespace Chevron.Core.Dictionaries
{
  public class Keys : ConcurrentDictionary<Guid, KeyModel> {
    private readonly object _lock = new();
    public Keys() : base() { }
    public Keys(IEnumerable<KeyModel> asList) : base() {
      AsList = asList;
    }
    public virtual Boolean Contains(Guid key) {
      try {
        return base.ContainsKey(key);
      } catch {
        return false;
      }
    }
    public virtual new KeyModel this[Guid key] {
      get {
        lock (_lock) {
          if (!Contains(key)) base[key] = new KeyModel() { Id = key, Name = "" };
          return base[key];
        }
      }
      set {
        lock (_lock) {
          if (value != null) {
            base[key] = value;
          } else {
            if (Contains(key)) {
              _ = base.TryRemove(key, out _);
            }
          }
        }
      }
    }
    public virtual void Add(KeyModel item) {
      lock (_lock) {
        if (item != null) {
          if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
          base[item.Id] = item;
        }
      }
    }
    public virtual void Remove(Guid key) {
      lock (_lock) {
        if (Contains(key)) {
          _ = base.TryRemove(key, out _);
        }
      }
    }
    public IEnumerable<KeyModel> AsList {
      get {
        lock (_lock) {
          return base.Values.ToList();
        }
      }
      set { 
        lock (_lock) {
          if (value != null) {
            base.Clear();
            foreach (var item in value) {
              Add(item);
            }
          }
        }
      }
    }

  }
}
