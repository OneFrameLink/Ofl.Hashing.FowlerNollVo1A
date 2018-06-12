using System;
using System.Numerics;
using Ofl.Numerics;

namespace Ofl.Hashing.FowlerNollVo1A
{
    public abstract class FowlerNollVo1ABigIntegerHashAlgorithm : IHashAlgorithm
    {
        #region Constructor

        protected FowlerNollVo1ABigIntegerHashAlgorithm(FowlerNollVo1ABigIntegerHashAlgorithmParameters parameters)
        {
            // Validate parameters.
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        #endregion

        #region Instance state.

        private readonly FowlerNollVo1ABigIntegerHashAlgorithmParameters _parameters;

        private BigInteger? _current;

        private ReadOnlyMemory<byte> _currentRom;

        #endregion

        #region IHashAlgorithm implementation.

        public int HashSize => _parameters.Bits;

        public void TransformBlock(ReadOnlySpan<byte> bytes)
        {
            // Set current if not set.
            _current = _current ?? _parameters.Offset;

            // The bytes.
            int size = HashSize / 8;

            // Cycle through the bytes.
            foreach (byte b in bytes)
                // Update hash.
                _current = ((_current ^ b).Value.Unchecked(size) * _parameters.Prime).Unchecked(size);

            // Set ROM.
            _currentRom = new ReadOnlyMemory<byte>(_current.Value.ToByteArray());
        }

        public ref readonly ReadOnlyMemory<byte> Hash => ref _currentRom;

        #endregion
    }
}
