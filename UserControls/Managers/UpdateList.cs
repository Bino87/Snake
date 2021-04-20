using System.Collections;
using System.Collections.Generic;
using UserControls.Models;

namespace UserControls.Managers
{
    public class UpdateList<T> : IList<T>
    {
        private readonly SimulationGuiViewModel _simulationGuiViewModel;
        private readonly List<T> _list;
        public UpdateList(SimulationGuiViewModel simulationGuiViewModel)
        {
            _simulationGuiViewModel = simulationGuiViewModel;
            _list = new List<T>();
        }

        public void Add(T item)
        {
            if (_simulationGuiViewModel.RunInBackground)
                return;
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public void Sort()
        {
            _list.Sort();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
    }
}