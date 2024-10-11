//*************************************************************************************
// File:     DefaultSwap.cs
//*************************************************************************************
// Description: Encapsula un intercambiador simple.
//*************************************************************************************
// Classes:      DefaultSwap : ISwap
//*************************************************************************************
// Author:      http://www.codeproject.com/csharp/csquicksort.asp
//*************************************************************************************

using System.Collections;

namespace ARWNI2S.Infrastructure.Collections.Sorting
{
    /// <summary>
    /// Representa un intercambiador simple.
    /// </summary>
    public class DefaultSwap : ISwap
    {
        /// <summary>
        /// Swap operation
        /// </summary>
        /// <param name="array">array to swap</param>
        /// <param name="left">left index</param>
        /// <param name="right">right index</param>
        public void Swap(IList array, int left, int right)
        {
            object swap = array[left];
            array[left] = array[right];
            array[right] = swap;
        }

        /// <summary>
        /// Set operation
        /// </summary>
        /// <param name="array">array to set</param>
        /// <param name="left">left index</param>
        /// <param name="right">right index</param>
        public void Set(IList array, int left, int right)
        {
            array[left] = array[right];
        }

        /// <summary>
        /// Set operation
        /// </summary>
        /// <param name="array">array to set</param>
        /// <param name="left">left index</param>
        /// <param name="obj">object to set</param>
        public void Set(IList array, int left, object obj)
        {
            array[left] = obj;
        }
    }
}
