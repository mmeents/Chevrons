using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.Core.PackageModels;

namespace Chevron.Core.Dictionaries
{
    public class WatchedFolders : ConcurrentDictionary<Guid, WatchedFolder> {

      private readonly object _lock = new();
      public WatchedFolders() : base() { }
      public WatchedFolders(IEnumerable<WatchedFolder> asList) : base() {
        AsList = asList;
      }

      public virtual Boolean Contains(Guid id) {
        try {
          return base.ContainsKey(id);
        } catch {
          return false;
        }
      }
      public virtual Boolean Remove(Guid id) {
        try {
          return base.TryRemove(id, out _);
        } catch {
          return false;
        }
      }
      public virtual new WatchedFolder this[Guid id] {
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

    public virtual void Add(WatchedFolder item) {
      lock (_lock) {
        if (item != null) {
          if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
          base[item.Id] = item;
        }
      }
    }

    public virtual void AddRange(IEnumerable<WatchedFolder> items) {
      lock (_lock) {
        foreach (var item in items) {
          Add(item);
        }
      }
    }

    public IEnumerable<WatchedFolder> AsList {
      get {
        lock (_lock) {
          return base.Values.ToList();
        }
      }
      set {
        lock (_lock) {
          base.Clear();
          foreach (var item in value) {
            Add(item);
          }
        }
      }
    }



  }
}
