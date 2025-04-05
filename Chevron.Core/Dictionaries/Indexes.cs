using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.Core.PackageModels;

namespace Chevron.Core.Dictionaries
{
  public class Indexes : ConcurrentDictionary<Guid, IndexModel> {
    private readonly object _lock = new();
    public Indexes() : base() { }
    public Indexes(IEnumerable<IndexModel> asList) : base() {
      AsList = asList;
    }
    public virtual Boolean Contains(Guid id) {
      try {
        return base.ContainsKey(id);
      } catch {
        return false;
      }
    }
    public virtual new IndexModel this[Guid id] {
      get {
        lock (_lock) {          
          return base[id];
        }
      }
      set {
        lock (_lock) {
          if (value != null) {
            if (value.Id != id) value.Id = id;
            base[id] = value;
          } else {
            if (Contains(id)) {
              _ = base.TryRemove(id, out _);
            }
          }
        }
      }
    }
    public virtual void Add(IndexModel item) {
      lock (_lock) {
        if (item != null) {
          if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
          base[item.Id] = item;
        }
      }
    }

    public virtual void Remove(Guid id) {
      lock (_lock) {
        if (Contains(id)) {
          _ = base.TryRemove(id, out _);
        }
      }
    }
    public IEnumerable<IndexModel> AsList {
      get {
        lock (_lock) {
          return base.Values.ToList();
        }
      }
      set {
        lock (_lock) {
          base.Clear();
          foreach (var item in value) {
            base[item.Id] = item;
          }
        }
      }
    }
  }
}
