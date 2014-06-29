using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Diffables
{
    // Object to implement that does all the change tracking for you.
    // Will need to com back to this later, after I figure out how to do this.
    public class DiffableObject : IDiffable
    {
        private List< Dictionary<string, object> > _deltas;
        private PropertyInfo[] _props;
        private int _currentPosition;

        public DiffableObject()
        {
            _deltas = new List<Dictionary<string, object>>();
            _props = GetType().GetProperties();
            _currentPosition = 0;
        }
        public void RecordState()
        {
            var delta = new Dictionary<string, object>();
            var hasPreviousState = HasPrevious();
            _props.ToList().ForEach(p =>
            {
                var currentValue = GetType().GetProperty( p.Name ).GetValue( this, null );
                if (hasPreviousState)
                {
                    // compare, then save if different
                    var previousValue = _deltas[_currentPosition - 1][p.Name];
                    var valuesAreDifferent = !Equals( previousValue, currentValue );
                    if (previousValue != null && valuesAreDifferent)
                    {
                        delta.Add( p.Name, currentValue );
                    }
                }
                else
                {
                    // save values without comparing
                    delta.Add( p.Name, currentValue );
                }

                
            });
            _deltas.Add(delta);
            ++_currentPosition;
        }

        public void RollBack()
        {
            LoadVersion(--_currentPosition);
        }

        public void RollForward()
        {
            LoadVersion(++_currentPosition);
        }

        public void LoadVersion(int position)
        {
            var delta = _deltas[position];
            _props.ToList().ForEach(p =>
            {
                // check if the delta has a value for each property,
                // set value if necessary
                if (delta[p.Name] != null)
                {
                    GetType().GetProperty(p.Name).SetValue(this, delta[p.Name]);
                }

            });
            _currentPosition = position;
        }

        public void RemoveVersion(int position)
        {
            _deltas.RemoveAt(position);
        }

        public int GetPosition()
        {
            return _currentPosition;
        }

        public Dictionary<string, object> GetDelta(int position)
        {
            return _deltas[position - 1];
        }

        public int GetChangeCount()
        {
            return _deltas.Count;
        }

        public bool HasNext()
        {
            return _currentPosition < _deltas.Count;
        }

        public bool HasPrevious()
        {
            return _deltas.Count > 0;
        }

        public void Flush()
        {
            _deltas.Clear();
        }
    }
}
