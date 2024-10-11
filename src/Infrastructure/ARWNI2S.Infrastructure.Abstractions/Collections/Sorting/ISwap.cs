//*************************************************************************************
// File:     ISwap.cs
//*************************************************************************************
// Description: Interface que define metodos que representan un intercambiador.
//*************************************************************************************
// Interfaces:  ISwap
//*************************************************************************************
// Author:      http://www.codeproject.com/csharp/csquicksort.asp
//*************************************************************************************

using System.Collections;

namespace ARWNI2S.Infrastructure.Collections.Sorting
{
    /// <summary>
    /// Interface que define metodos para la representacion de un intercambiador.
    /// </summary>
    public interface ISwap
    {
        /// <summary>
        /// Swap operation
        /// </summary>
        /// <param name="array">array to swap</param>
        /// <param name="left">left index</param>
        /// <param name="right">right index</param>
        void Swap(IList array, int left, int right);
        /// <summary>
        /// Set operation
        /// </summary>
        /// <param name="array">array to set</param>
        /// <param name="left">left index</param>
        /// <param name="right">right index</param>
        void Set(IList array, int left, int right);
        /// <summary>
        /// Set operation
        /// </summary>
        /// <param name="array">array to set</param>
        /// <param name="left">left index</param>
        /// <param name="obj">object to set</param>
        void Set(IList array, int left, object obj);
    }
}
