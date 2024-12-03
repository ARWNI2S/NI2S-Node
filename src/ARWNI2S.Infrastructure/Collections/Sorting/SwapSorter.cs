//*************************************************************************************
// File:     SwapSorter.cs
//*************************************************************************************
// Description: Encapsula una estructura para la reordenacion por intercambio de un
//              objeto derivado de System.Collections.IList
//*************************************************************************************
// Classes:      SwapSorter : ISorter
//*************************************************************************************
// Author:      http://www.codeproject.com/csharp/csquicksort.asp
//*************************************************************************************

using ARWNI2S.Collections.Comparers;
using System.Collections;

namespace ARWNI2S.Collections.Sorting
{
    /// <summary>
    /// Representa una estructura para la reordenacion por intercambio de un objeto derivado de System.Collections.IList
    /// </summary>
    public abstract class SwapSorter : ISorter
    {
        private IComparer _comparer;
        private ISwap _swapper;

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public SwapSorter()
        {
            _comparer = new ComparableComparer();
            _swapper = new DefaultSwap();
        }
        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="comparer">El comparador a emplear.</param>
        /// <param name="swapper">El intercambiador a emplear.</param>
        public SwapSorter(IComparer comparer, ISwap swapper)
        {
            ArgumentNullException.ThrowIfNull(comparer);
            ArgumentNullException.ThrowIfNull(swapper);

            _comparer = comparer;
            _swapper = swapper;
        }
        /// <summary>
        /// Devuelve o establece el comparador.
        /// </summary>
        public IComparer Comparer
        {
            get
            {
                return _comparer;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _comparer = value;
            }
        }
        /// <summary>
        /// Devuelve o establece el intercambiador.
        /// </summary>
        public ISwap Swapper
        {
            get
            {
                return _swapper;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _swapper = value;
            }
        }

        /// <summary>
        /// Ordena los valores contenidos en <paramref name="list"/> segun los criterios proporcionados por comparador e intercambiador.
        /// </summary>
        /// <param name="list">Un objeto derivado de System.Collections.IList que contiene los valores a ordenar.</param>
        public abstract void Sort(IList list);
    }
}
