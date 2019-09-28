using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Orcabot.Types
{
    public class DistanceSortedList<T> where T : IDistanceProvider
    {
        private readonly object listLock = new object();

        private List<DistanceWrapper<T>> list;
        public IReadOnlyList<DistanceWrapper<T>> List
        {
            get
            {
                if (!IsSorted)
                {
                    Sort();
                }
                return list.AsReadOnly();
            }
        }

        public Vector3 Position { get; private set; }
        public bool IsSorted { get; private set; }

        public DistanceSortedList(ICollection<T> source, Vector3 position)
        {
            Position = position;
            lock (listLock)
            {
                list = new List<DistanceWrapper<T>>(source.Count);
                foreach (T item in source)
                {
                    list.Add(new DistanceWrapper<T>(item, position));
                }
                IsSorted = false;
            }
        }

        public DistanceSortedList(IReadOnlyCollection<T> source, Vector3 position)
        {
            Position = position;
            lock (listLock)
            {
                list = new List<DistanceWrapper<T>>(source.Count);
                foreach (T item in source)
                {
                    list.Add(new DistanceWrapper<T>(item, position));
                }
                IsSorted = false;
            }
        }

        public void AddRange(ICollection<T> source)
        {
            lock (listLock)
            {
                list.Capacity += source.Count;
                foreach (T item in source)
                {
                    list.Add(new DistanceWrapper<T>(item, Position));
                }
                IsSorted = false;
            }
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
            lock (listLock)
            {
                foreach (DistanceWrapper<T> item in list)
                {
                    item.SetPosition(position);
                }
                IsSorted = false;
            }
        }

        public void Sort()
        {
            lock (listLock)
            {
                list.Sort(new DistanceWrapperComparer<T>());
                IsSorted = true;
            }
        }

        public IReadOnlyList<T> AsDirectList()
        {
            if (!IsSorted)
            {
                Sort();
            }
            return new List<T>(list.Select(wrapper => { return wrapper.Target; })).AsReadOnly();
        }
    }

    public static class DistanceSortedListExtensions
    {
        //public static DistanceSortedList<T> GetDistanceSortedList<T>(this ICollection<T> source, Vector3 position) where T : IDistanceProvider
        //{
        //    DistanceSortedList<T> result = new DistanceSortedList<T>(source, position);
        //    result.Sort();
        //    return result;
        //}

        public static DistanceSortedList<T> GetDistanceSortedList<T>(this IReadOnlyCollection<T> source, Vector3 position) where T : IDistanceProvider
        {
            DistanceSortedList<T> result = new DistanceSortedList<T>(source, position);
            result.Sort();
            return result;
        }
    }

    public struct DistanceWrapper<T> where T : IDistanceProvider
    {
        public float DistanceSquared { get; private set; }
        public float Distance { get { return MathF.Sqrt(DistanceSquared); } }
        public readonly T Target;

        public DistanceWrapper(T target, Vector3 position)
        {
            DistanceSquared = target.GetDistanceSquared(position);
            Target = target;
        }

        public void SetPosition(Vector3 position)
        {
            DistanceSquared = Target.GetDistanceSquared(position);
        }
    }

    public class DistanceWrapperComparer<T> : IComparer<DistanceWrapper<T>> where T : IDistanceProvider
    {
        public int Compare(DistanceWrapper<T> x, DistanceWrapper<T> y)
        {
            return (int)(x.DistanceSquared - y.DistanceSquared);
        }
    }

    public interface IDistanceProvider
    {
        float GetDistanceSquared(Vector3 position);
    }
}
