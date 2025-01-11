﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Security.Cryptography;

namespace ARWNI2S.Engine.Security
{
    /// <summary>
    ///  Represents the class implementation of cryptographic random number generator derive
    /// </summary>
    public sealed class SecureRandomNumberGenerator : RandomNumberGenerator
    {
        #region Field

        private bool _disposed = false;
        private readonly RandomNumberGenerator _rng;

        #endregion

        #region Ctor

        public SecureRandomNumberGenerator() => _rng = Create();

        #endregion

        #region Methods

        public int Next()
        {
            var data = new byte[sizeof(int)];
            _rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0) & int.MaxValue - 1;
        }

        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue);
            return (int)Math.Floor(minValue + ((double)maxValue - minValue) * NextDouble());
        }

        public double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            _rng.GetBytes(data);
            var randUint = BitConverter.ToUInt32(data, 0);
            return randUint / (uint.MaxValue + 1.0);
        }

        public override void GetBytes(byte[] data)
        {
            _rng.GetBytes(data);
        }

        public override void GetNonZeroBytes(byte[] data)
        {
            _rng.GetNonZeroBytes(data);
        }

        /// <summary>
        /// Dispose secure random
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _rng?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
