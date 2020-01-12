using System;
using System.Runtime.InteropServices;

namespace Ofl.Hashing.FowlerNollVo1A
{
    public class FowlerNollVo1A64BitHashAlgorithm : IHashAlgorithm
    {
        #region State.

        private const ulong Prime = 1099511628211;

        private const ulong Offset = 14695981039346656037;

        private byte[]? _current;

        private ReadOnlyMemory<byte> _currentRom;

        #endregion

        #region IHashAlgorithm implementation.

        public int HashSize => 64;

        public void TransformBlock(ReadOnlySpan<byte> bytes)
        {
            // The current hash.
            ulong current;
                
            // If the current value is not set, allocate.
            if (_current == null)
            {
                // Set to the offset.
                current = Offset;

                // Allocate.
                _current = new byte[Marshal.SizeOf(typeof(ulong))];
            }
            else
                // Set to the current.
                current = MemoryMarshal.Read<ulong>(_current);
            
            // Unchecked.
            unchecked
            {
                // Loop.
                foreach (byte b in bytes)
                    // XOR then multiply.
                    current = (current ^ b) * Prime;
            }

            // Set the value.
            MemoryMarshal.Write(_current, ref current);
            _currentRom = _current;
        }

        public ref readonly ReadOnlyMemory<byte> Hash => ref _currentRom;

        #endregion
    }
}
