using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffables
{
    public class DiffService
    {
        private readonly List<IDiffable> _diffables;

        public DiffService()
        {
            _diffables = new List<IDiffable>();
        }

        public void Register(IDiffable obj)
        {
            _diffables.Add(obj);
        }

        public void Register(IEnumerable<IDiffable> objs)
        {
            foreach (var o in objs)
            {
                _diffables.Add(o);
            }
        }

        public void RecordSnapshot (  )
        {
            _diffables.ForEach( d => d.RecordState() );
        }

        public void RollBack ()
        {
            _diffables.ForEach( d => d.RollBack() );
        }

        public void RollForward ()
        {
            _diffables.ForEach( d => d.RollForward() );
        }

        public void Flush ()
        {
            _diffables.ForEach( d => d.Flush() );
        }

        public void LoadVersion(int version)
        {
            _diffables.ForEach( d => d.LoadVersion(version) );
        }


        public int ItemsCount()
        {
            return _diffables.Count;
    }
    }
}
