//*************************************************************************************
// File:     ISorter.cs
//*************************************************************************************
// Description: Interface que define metodos que representan un reordenador.
//*************************************************************************************
// Interfaces:  ISorter
//*************************************************************************************
// Author:      http://www.codeproject.com/csharp/csquicksort.asp
//*************************************************************************************

using System.Collections;

namespace ARWNI2S.Infrastructure.Abstractions.Collections.Sorting
{
    /// <summary>
    /// Represents a List Sorter
    /// </summary>
    public interface ISorter
    {
        /// <summary>
        /// Sorts a list
        /// </summary>
        /// <param name="list"><see cref="IList"/> object to sort.</param>
        void Sort(IList list);
    }
}
