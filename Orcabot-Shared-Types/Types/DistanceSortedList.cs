using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Orcabot.Types
{
    /// <summary>
    /// A list wrapper providing methods for sorting by distance relative to a position
    /// </summary>
    /// <typeparam name="T">A type implementing the <see cref="IDistanceProvider"/> interface</typeparam>
    public class DistanceSortedList<T> where T : IDistanceProvider
    {
        private readonly object listLock = new object();

        private List<DistanceWrapper<T>> list;

        /// <summary>
        /// List, sorted by the distance, of all the stored systems.
        /// </summary>
        public IReadOnlyList<DistanceWrapper<T>> Sorted
        {
            get
            {
                bool isSorted;
                lock (listLock)
                {
                    isSorted = IsSorted;
                }
                if (!isSorted)
                {
                    Sort();
                }
                return list.AsReadOnly();
            }
        }

        /// <summary>
        /// Position the distances are calculated relative to
        /// </summary>
        public Vector3 Position { get; private set; }
        private bool IsSorted;

        /// <summary>
        /// Constructs a new DistanceSortedList object
        /// </summary>
        /// <param name="source">Collection of items as base list contents</param>
        /// <param name="position">Relative position to calculate distances to</param>
        public DistanceSortedList(IReadOnlyCollection<T> source = null, Vector3 position = default)
        {
            Position = position;
            if (source == null)
            {
                list = new List<DistanceWrapper<T>>();
            }
            else
            {
                list = new List<DistanceWrapper<T>>(source.Count);
                foreach (T item in source)
                {
                    list.Add(new DistanceWrapper<T>(item, position));
                }
            }
            IsSorted = false;
        }

        /// <summary>
        /// Add a range of new items
        /// </summary>
        /// <param name="source">Items to add</param>
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

        /// <summary>
        /// Update the relative position distances are calculated to
        /// </summary>
        /// <param name="position">new relative position</param>
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

        private void Sort()
        {
            lock (listLock)
            {
                list.Sort(new DistanceWrapperComparer<T>());
                IsSorted = true;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="List{T}"/>, which is not wrapped with DistanceWrappers, but still sorted by distance.
        /// </summary>
        public List<T> AsDirectList()
        {
            bool isSorted;
            lock (listLock)
            {
                isSorted = IsSorted;
            }
            if (!isSorted)
            {
                Sort();
            }
            return new List<T>(list.Select(wrapper => { return wrapper.Target; }));
        }
    }

    public static class DistanceSortedListExtensions
    {
        /// <summary>
        /// Constructs a new <see cref="DistanceSortedList{T}"/> object
        /// </summary>
        /// <typeparam name="T">A type implementing the <see cref="IDistanceProvider"/> interface</typeparam>
        /// <param name="source">Collection of items as base list contents</param>
        /// <param name="position">Relative position to calculate distances to</param>
        public static DistanceSortedList<T> GetDistanceSortedList<T>(this IReadOnlyCollection<T> source, Vector3 position) where T : IDistanceProvider
        {
            DistanceSortedList<T> result = new DistanceSortedList<T>(source, position);
            return result;
        }
    }

    /// <summary>
    /// Wraps a type implementing <see cref="IDistanceProvider"/> with Distance information
    /// </summary>
    /// <typeparam name="T">A type implementing the <see cref="IDistanceProvider"/> interface</typeparam>
    public struct DistanceWrapper<T> where T : IDistanceProvider
    {
        /// <summary>
        /// The squared distance. Due to being easier to calculate than <see cref="Distance"/>, used for sorting
        /// </summary>
        public float DistanceSquared { get; private set; }
        /// <summary>
        /// Calculates the Distance as the square root of <see cref="DistanceSquared"/>
        /// </summary>
        public float Distance { get { return MathF.Sqrt(DistanceSquared); } }
        /// <summary>
        /// The wrapped Object/Struct
        /// </summary>
        public readonly T Target;

        /// <summary>
        /// Constructs a new <see cref="DistanceWrapper{T}"/> struct
        /// </summary>
        /// <param name="target">Object/Struct to be wrapped</param>
        /// <param name="position">Position to calculate the distance relative to</param>
        public DistanceWrapper(T target, Vector3 position)
        {
            DistanceSquared = target.GetDistanceSquaredTo(position);
            Target = target;
        }

        /// <summary>
        /// Sets a new <paramref name="position"/> and as such updates the Distance
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            DistanceSquared = Target.GetDistanceSquaredTo(position);
        }
    }

    /// <summary>
    /// Compares <see cref="DistanceWrapper{T}"/> structs
    /// </summary>
    /// <typeparam name="T">A type implementing the <see cref="IDistanceProvider"/> interface</typeparam>
    public class DistanceWrapperComparer<T> : IComparer<DistanceWrapper<T>> where T : IDistanceProvider
    {
        public int Compare(DistanceWrapper<T> x, DistanceWrapper<T> y)
        {
            return (int)(x.DistanceSquared - y.DistanceSquared);
        }
    }

    /// <summary>
    /// Exposes a method to retrieve the squared distance relative to a <see cref="Vector3"/> position
    /// </summary>
    public interface IDistanceProvider
    {
        /// <summary>
        /// Calculates the squared distance relative to <paramref name="position"/>
        /// </summary>
        float GetDistanceSquaredTo(Vector3 position);
    }
}
