using Orcabot.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Orcabot.Helpers;

namespace Orcabot.Types
{
    public class Galaxy
    {
        #region Fields & Properties

        private readonly Dictionary<string, StarSystem> systems = new Dictionary<string, StarSystem>();
        private readonly Dictionary<Vector3, SystemVoxel> voxels = new Dictionary<Vector3, SystemVoxel>();
        private readonly Dictionary<TraderType, HashSet<StarSystem>> materialTraders = new Dictionary<TraderType, HashSet<StarSystem>>();

        /// <summary>
        /// Count of all loaded systems
        /// </summary>
        public int SystemCount => systems.Count;

        /// <summary>
        /// Amount of voxels
        /// </summary>
        public int VoxelCount => voxels.Count;

        /// <summary>
        /// Amount of material trader systems
        /// </summary>
        public int Tradercount
        {
            get
            {
                int count = 0;
                foreach (HashSet<StarSystem> systems in materialTraders.Values)
                {
                    count += systems.Count;
                }
                return count;
            }
        }

        /// <summary>
        /// All systems stored
        /// </summary>
        public ICollection<StarSystem> AllSystems => systems.Values;

        #endregion
        #region Constructor

        /// <summary>
        /// Constructs a new <see cref="Galaxy"/> object
        /// </summary>
        /// <param name="systemEnumerable">Enumerable of systems contained in the galaxy</param>
        public Galaxy(IEnumerable<StarSystem> systemEnumerable)
        {
            foreach (StarSystem system in systemEnumerable)
            {
                if (!voxels.TryGetValue(system.Coordinate, out SystemVoxel voxel))
                {
                    voxel = new SystemVoxel((VoxelPos)system.Coordinate);
                    voxels.Add(system.Coordinate, voxel);
                }
                voxel.AddSystem(system);

                systems.Add(system.Name, system);

                if (system.HasMaterialTrader)
                {
                    addMaterialTrader(system);
                }
            }
        }

        private void addMaterialTrader(StarSystem system)
        {
            if (system.MaterialTraderType == TraderType.NoTrader)
            {
                throw new ArgumentException(nameof(system));
            }

            if (!materialTraders.TryGetValue(system.MaterialTraderType, out HashSet<StarSystem> systemCollection))
            {
                systemCollection = new HashSet<StarSystem>();
                materialTraders.Add(system.MaterialTraderType, systemCollection);
            }
            systemCollection.Add(system);
        }

        #endregion
        #region Accessing Systems

        /// <summary>
        /// Sorts <see cref="StarSystem"/>s relative to a the reference <paramref name="position"/>, with a result <paramref name="radius"/>
        /// </summary>
        /// <param name="position">Reference position to sort systems towards</param>
        /// <param name="radius">Radius to choose voxels from</param>
        /// <param name="filter">If not null, only sorts and returns systems matching the filter settings</param>
        /// <returns>Sorted list of <see cref="StarSystem"/>s wrapped in <see cref="DistanceWrapper{StarSystem}"/> structs</returns>
        public IReadOnlyList<DistanceWrapper<StarSystem>> GetSortedSystemsNear(Vector3 position, int radius = 100, SystemSearchFilter? filter = null)
        {
            IReadOnlyList<StarSystem> systemsNear = GetSystemsNear(position, radius, filter);
            DistanceSortedList<StarSystem> sortedSystems = systemsNear.GetDistanceSortedList(position);
            return sortedSystems.Sorted;
        }

        /// <summary>
        /// Retrieves <see cref="StarSystem"/>s relative to a the reference <paramref name="position"/>, with a result <paramref name="radius"/>
        /// </summary>
        /// <param name="position">Reference position to sort systems towards</param>
        /// <param name="radius">Radius to choose voxels from</param>
        /// <param name="filter">If not null, only sorts and returns systems matching the filter settings</param>
        /// <returns>List of <see cref="StarSystem"/>s</returns>
        public List<StarSystem> GetSystemsNear(Vector3 position, int radius = 100, SystemSearchFilter? filter = null)
        {
            IReadOnlyList<DistanceWrapper<SystemVoxel>> orderedVoxels = GetVoxelsSorted(position);

            int maxDistanceSquared = radius * radius;

            List<StarSystem> systemsNear = new List<StarSystem>();
            foreach (DistanceWrapper<SystemVoxel> voxelWrapper in orderedVoxels)
            {
                if (voxelWrapper.DistanceSquared < maxDistanceSquared)
                {
                    systemsNear.AddRange(voxelWrapper.Target.Systems);
                }
            }
            if (filter != null)
            {
                systemsNear.Filter(filter.Value, out List<StarSystem> filtered);
                return filtered;
            }
            return systemsNear;
        }

        /// <summary>
        /// Attempts to retrieve a system by name
        /// </summary>
        /// <param name="systemName">System name to filter for</param>
        /// <param name="system">Result, if one is found</param>
        /// <returns>True, if a matching system is found</returns>
        public bool TryGetSystem(string systemName, out StarSystem system)
        {
            return systems.TryGetValue(systemName, out system);
        }

        private IReadOnlyList<DistanceWrapper<SystemVoxel>> GetVoxelsSorted(Vector3 position)
        {
            DistanceSortedList<SystemVoxel> result = voxels.Values.GetDistanceSortedList(position);
            return result.Sorted;
        }

        public bool TryGetMaterialTradersOrderedByDistance(TraderType type, Vector3 position, out IReadOnlyList<DistanceWrapper<StarSystem>> result)
        {
            if (materialTraders.TryGetValue(type, out HashSet<StarSystem> systems))
            {
                DistanceSortedList<StarSystem> list = systems.GetDistanceSortedList(position);
                result = list.Sorted;
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// Sorts all stored <see cref="StarSystem"/>s to a reference <paramref name="position"/>
        /// </summary>
        /// <param name="position">Reference position to sort systems towards</param>
        /// <param name="filter">If not null, only sorts and returns systems matching the filter settings</param>
        /// <returns>Sorted list of <see cref="StarSystem"/>s wrapped in <see cref="DistanceWrapper{StarSystem}"/> structs</returns>
        public IReadOnlyList<DistanceWrapper<StarSystem>> SortAllSystems(Vector3 position, SystemSearchFilter? filter)
        {
            if (filter.HasValue)
            {
                if (FilterAllSystems(filter.Value, out List<StarSystem> filteredSystems))
                {
                    DistanceSortedList<StarSystem> sorted = filteredSystems.GetDistanceSortedList(position);
                    return sorted.Sorted;
                }
                else
                {
                    return new List<DistanceWrapper<StarSystem>>(0);
                }
            }
            else
            {
                DistanceSortedList<StarSystem> sorted = systems.Values.GetDistanceSortedList(position);
                return sorted.Sorted;
            }
        }

        /// <summary>
        /// Filters all <see cref="StarSystem"/>s
        /// </summary>
        /// <param name="filter">Filter to apply</param>
        /// <param name="filteredSystems">List of <see cref="StarSystem"/>s matching the filter</param>
        /// <returns>True, if more than one result was returned</returns>
        public bool FilterAllSystems(SystemSearchFilter filter, out List<StarSystem> filteredSystems)
        {
            return systems.Values.Filter(filter, out filteredSystems);
        }

        #endregion

        private class SystemVoxel : IDistanceProvider
        {
            public readonly VoxelPos Center;

            private readonly HashSet<StarSystem> systems;
            private readonly Dictionary<TraderType, HashSet<StarSystem>> MaterialTraders = new Dictionary<TraderType, HashSet<StarSystem>>();

            internal ICollection<StarSystem> Systems { get { return systems; } }

            internal SystemVoxel(VoxelPos center)
            {
                Center = center;
                systems = new HashSet<StarSystem>();
            }

            internal SystemVoxel(VoxelPos center, IEnumerable<StarSystem> systems)
            {
                Center = center;
                this.systems = new HashSet<StarSystem>(systems);
                foreach (StarSystem system in systems)
                {
                    if (system.HasMaterialTrader)
                    {
                        addMaterialTrader(system);
                    }
                }
            }

            private void addMaterialTrader(StarSystem system)
            {
                if (system.MaterialTraderType == TraderType.NoTrader)
                {
                    throw new ArgumentException(nameof(system));
                }

                if (!MaterialTraders.TryGetValue(system.MaterialTraderType, out HashSet<StarSystem> systemCollection))
                {
                    systemCollection = new HashSet<StarSystem>();
                    MaterialTraders.Add(system.MaterialTraderType, systemCollection);
                }
                systemCollection.Add(system);
            }

            internal void AddSystem(StarSystem system)
            {
                systems.Add(system);
                if (system.HasMaterialTrader)
                {
                    addMaterialTrader(system);
                }
            }

            public float GetDistanceSquaredTo(Vector3 position)
            {
                return Vector3.DistanceSquared((Vector3)Center, position);
            }

        }
        private const int LIGHTSECONDSPERLIGHTYEAR = 31557600;

        public static float LightYearsToLightSeconds(float lightyears)
        {
            return lightyears * LIGHTSECONDSPERLIGHTYEAR;
        }

        public static float LightSecondsToLightYears(float lightseconds)
        {
            return lightseconds / LIGHTSECONDSPERLIGHTYEAR;
        }
    }

    public struct SystemSearchFilter
    {
        public TraderType? TraderType;
        public string PermitName;
        public bool? PermitLocked;
        public SystemSecurity? Security;
    }

    public struct StationSearchFilter
    {
        public StationFacility? Facility;
        public StationType? Type;
        public PadSize? MinPadSize;
    }
}
