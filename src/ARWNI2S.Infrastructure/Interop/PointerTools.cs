using System.Runtime.InteropServices;

namespace ARWNI2S.Interop
{

    internal class PointerTools
    {

        #region Methods

        public static object[] PtrToStructureArray(nint pointer, Type structureType, int len)
        {

            object[] array = new object[len];

            for (int i = 0; i < len; i++)
            {
                array[i] = Marshal.PtrToStructure(pointer, structureType);
                pointer = pointer.ToInt32() + Marshal.SizeOf(array[i]);
            }

            return array;

        }

        #endregion

    }
}
