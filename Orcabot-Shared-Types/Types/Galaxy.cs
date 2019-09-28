using Orcabot.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Orcabot.Types
{
    public class Galaxy
    {
        private readonly Dictionary<string, StarSystem> systems = new Dictionary<string, StarSystem>();
        private readonly Dictionary<Vector3, SystemVoxel> voxels = new Dictionary<Vector3, SystemVoxel>();
        private readonly Dictionary<TraderType, HashSet<StarSystem>> materialTraders = new Dictionary<TraderType, HashSet<StarSystem>>();

        public int SystemCount => systems.Count;
        public int VoxelCount => voxels.Count;
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


        public IReadOnlyList<DistanceWrapper<StarSystem>> GetSortedSystemsNear(Vector3 position, int maxDistance)
        {
            IReadOnlyList<StarSystem> systemsNear = GetSystemsNear(position, maxDistance);
            DistanceSortedList<StarSystem> sortedSystems = systemsNear.GetDistanceSortedList(position);
            return sortedSystems.List;
        }

        public IReadOnlyList<StarSystem> GetSystemsNear(Vector3 position, int maxDistance)
        {
            IReadOnlyList<DistanceWrapper<SystemVoxel>> orderedVoxels = GetVoxelsSorted(position);

            int maxDistanceSquared = maxDistance * maxDistance;

            List<StarSystem> systemsNear = new List<StarSystem>();
            foreach (DistanceWrapper<SystemVoxel> voxelWrapper in orderedVoxels)
            {
                if (voxelWrapper.DistanceSquared < maxDistanceSquared)
                {
                    systemsNear.AddRange(voxelWrapper.Target.Systems);
                }
            }
            return systemsNear.AsReadOnly();
        }

        public bool TryGetSystem(string systemName, out StarSystem system)
        {
            return systems.TryGetValue(systemName, out system);
        }

        private IReadOnlyList<DistanceWrapper<SystemVoxel>> GetVoxelsSorted(Vector3 position)
        {
            DistanceSortedList<SystemVoxel> result = voxels.Values.GetDistanceSortedList(position);
            return result.List;
        }

        public bool TryGetMaterialTradersOrderedByDistance(TraderType type, Vector3 position, out IReadOnlyList<DistanceWrapper<StarSystem>> result)
        {
            if (materialTraders.TryGetValue(type, out HashSet<StarSystem> systems))
            {
                DistanceSortedList<StarSystem> list = systems.GetDistanceSortedList(position);
                result = list.List;
                return true;
            }
            result = null;
            return false;
        }

        private class SystemVoxel : IDistanceProvider
        {
            private const int LIGHTSECONDSPERLIGHTYEAR = 31557600;
            public readonly VoxelPos Center;

            private readonly HashSet<StarSystem> systems;
            private readonly Dictionary<TraderType, HashSet<StarSystem>> MaterialTraders = new Dictionary<TraderType, HashSet<StarSystem>>();

            public ICollection<StarSystem> Systems { get { return systems; } }

            public SystemVoxel(VoxelPos center)
            {
                Center = center;
                systems = new HashSet<StarSystem>();
            }

            public SystemVoxel(VoxelPos center, IEnumerable<StarSystem> systems)
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

            public void AddSystem(StarSystem system)
            {
                systems.Add(system);
                if (system.HasMaterialTrader)
                {
                    addMaterialTrader(system);
                }
            }

            public bool TryGetMaterialTradersOrderedByDistance(TraderType type, Vector3 position, out IReadOnlyList<DistanceWrapper<StarSystem>> result)
            {
                if (MaterialTraders.TryGetValue(type, out HashSet<StarSystem> systems))
                {
                    DistanceSortedList<StarSystem> list = systems.GetDistanceSortedList(position);
                    result = list.List;
                    return true;
                }
                result = null;
                return false;
            }

            public bool HasMaterialTrader(TraderType type)
            {
                return MaterialTraders.ContainsKey(type);
            }

            public float GetDistanceSquared(Vector3 position)
            {
                return Vector3.DistanceSquared((Vector3)Center, position);
            }

            public static float LightYearsToLightSeconds(float lightyears)
            {
                return lightyears * LIGHTSECONDSPERLIGHTYEAR;
            }

            public static float LightSecondsToLightYears(float lightseconds)
            {
                return lightseconds / LIGHTSECONDSPERLIGHTYEAR;
            }
        }
    }
}
