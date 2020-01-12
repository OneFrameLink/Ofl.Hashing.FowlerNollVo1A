using System;
using System.Runtime.InteropServices;

namespace Ofl.Hashing.FowlerNollVo1A
{
    public class FowlerNollVo1A32BitHashAlgorithm : IHashAlgorithm
    {
        #region State.

        private const uint Prime = 16777619;

        private const uint Offset = 2166136261;

        private byte[]? _current;

        private ReadOnlyMemory<byte> _currentRom;

        #endregion

        #region IHashAlgorithm implementation.

        public int HashSize => 64;

        public void TransformBlock(ReadOnlySpan<byte> bytes)
        {
            // The current hash.
            uint current;
                
            // If it's null, use the offset, and allocate.
            if (_current == null)
            {
                // Set current to the offset.
                current = Offset;

                // Allocate.
                _current = new byte[Marshal.SizeOf(typeof(uint))];
            }
            else
                // Marshal.
                current = _current == null ? Offset : MemoryMarshal.Read<uint>(_current);

            // Unchecked.
            unchecked
            {
                // Loop.
                foreach (byte b in bytes)
                    // XOR then multiply.
                    current = (current ^ b) * Prime;
            }

            // Write.
            MemoryMarshal.Write(_current, ref current);
            _currentRom = _current;
        }

        public ref readonly ReadOnlyMemory<byte> Hash => ref _currentRom;

        #endregion
    }
}
