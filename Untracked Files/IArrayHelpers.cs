using System;

//using SDC;
namespace SDC.Schema
{
    public interface IArrayHelpers<T>
    {
        int GetFirstNullArrayIndex(T[] array, int growthIncrement = 3)
        {
            int i = 0;
            if (array is null) array = new T[growthIncrement];
            foreach (var n in array)
            {
                if (n is null) return i;
                i++;
            }
            Array.Resize(ref array, array.Length + growthIncrement);
            return i;
        }

        T[] ArrayAddItemReturnArray(T[] array, T itemToAdd, int growthIncrement = 3)
        {
            int i = GetFirstNullArrayIndex(array, growthIncrement);
            array[i] = itemToAdd;
            return array;

        }
        T ArrayAddReturnItem(T[] array, T itemToAdd, int growthIncrement = 3)
        {
            ArrayAddItemReturnArray(array, itemToAdd, growthIncrement);
            return itemToAdd;

        }


    }

}
