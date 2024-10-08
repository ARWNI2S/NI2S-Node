//*************************************************************************************
// File:     ComparableComparer.cs
//*************************************************************************************
// Description: Encapsula un comparador comparable.
//*************************************************************************************
// Classes:      ComparableComparer : IComparer
//*************************************************************************************
// Author:      http://www.codeproject.com/csharp/csquicksort.asp
//*************************************************************************************

using System.Collections;

namespace ARWNI2S.Infrastructure.Collections.Comparers
{
    /// <summary>
    /// Clase que representa un comparador comparable.
    /// </summary>
    internal class ComparableComparer : IComparer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IComparable x, object y)
        {
            return x.CompareTo(y);
        }

        #region IComparer Members
        /// <summary>
        /// Compara dos objetos y devuelve un valor que indica si uno de ellos es menor, igual o mayor que el otro.
        /// </summary>
        /// <param name="x">Primer objeto que se va a comparar.</param>
        /// <param name="y">Segundo objeto que se va a comparar.</param>
        /// <returns>Entero con signo que indica los valores relativos de x e y, como se muestra en la tabla siguiente.
        /// Valor Significado Menor que cero x es menor que y. Zero x es igual que y. Mayor que cero x es mayor que y.</returns>
        /// <exception cref="ArgumentException">Ni x ni y implementan la interfaz System.IComparable.O bien x y y son de tipos
        /// diferentes y ninguno puede controlar comparaciones con el otro.</exception>
        int IComparer.Compare(object x, object y)
        {
            return Compare((IComparable)x, y);
        }
        #endregion
    }
}