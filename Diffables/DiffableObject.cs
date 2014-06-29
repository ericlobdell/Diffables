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
        private int _currentDelta;

        public DiffableObject ()
        {
            _deltas = new List<Dictionary<string, object>>();
            _props = GetType().GetProperties();
            _currentDelta = 0;
        }
        public void RecordState ()
        {
            var delta = new Dictionary<string, object>();
            var hasPreviousState = HasPrevious();
            _props.ToList().ForEach( p =>
            {
                var currentValue = GetType().GetProperty( p.Name ).GetValue( this, null );
                if ( hasPreviousState )
                {
                    // compare, then save if different
                    var previousValue = LastValueOf( p.Name );

                    if ( previousValue != null && !Equals( previousValue, currentValue ) )
                        delta.Add( p.Name, currentValue );
                }
                else
                {
                    // save values without comparing
                    delta.Add( p.Name, currentValue );
                }


            } );

            if ( delta.Count > 0 )
            {
                _deltas.Add( delta );
                _currentDelta++;
            }

        }

        private object LastValueOf ( string property )
        {
            object previousValue = null;
            for ( var i = _deltas.Count - 1; i >= 0; i-- )
            {
                var d = _deltas [ i ];
                if ( d.ContainsKey( property ) )
                {
                    previousValue = d [ property ];
                    break;
                }
            }
            return previousValue;
        }

        public void RollBack ()
        {
            if ( HasPrevious() )
                LoadVersion( _currentDelta - 1 );
        }

        public void RollForward ()
        {
            if ( HasNext() )
                LoadVersion( _currentDelta + 1 );
        }

        public void LoadVersion ( int position )
        {
            var delta = _deltas [ position - 1 ];
            _props.ToList().ForEach( p =>
            {
                // check if the delta has a value for each property,
                // set value if necessary
                if ( delta.ContainsKey( p.Name ) )
                {
                    GetType().GetProperty( p.Name ).SetValue( this, delta [ p.Name ] );
                }

            } );
            _currentDelta = position;
        }

        // This feels like it might cause problems,
        // consider using if changes are being tracked with unique id
        //public void RemoveVersion(int position)
        //{
        //    _deltas.RemoveAt(position);
        //}

        public int GetPosition ()
        {
            return _currentDelta;
        }

        public Dictionary<string, object> GetDelta ( int position )
        {
            return _deltas [ position - 1 ];
        }

        public int GetChangeCount ()
        {
            return _deltas.Count;
        }

        public bool HasNext ()
        {
            return _currentDelta < _deltas.Count;
        }

        public bool HasPrevious ()
        {
            return _deltas.Count > 0;
        }

        public void Flush ()
        {
            _deltas.Clear();
        }
    }
}
