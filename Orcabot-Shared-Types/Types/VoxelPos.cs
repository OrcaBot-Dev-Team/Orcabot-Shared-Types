using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Orcabot.Types
{
    public struct VoxelPos
    {
        public const int VOXELSIZE = 25;

        public int X;
        public int Y;
        public int Z;

        internal VoxelPos(Vector3 position)
        {
            position /= VOXELSIZE;
            X = (int)position.X;
            Y = (int)position.Y;
            Z = (int)position.Z;
        }

        internal VoxelPos(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static explicit operator Vector3(VoxelPos value)
        {
            Vector3 result = new Vector3(value.X, value.Y, value.Z);
            result *= VOXELSIZE;
            return result;
        }

        public static explicit operator VoxelPos(Vector3 value)
        {
            return new VoxelPos(value);
        }
    }
}
