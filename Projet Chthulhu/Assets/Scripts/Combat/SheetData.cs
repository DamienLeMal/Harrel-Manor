using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetData : ScriptableObject
{
    /// <summary>
    /// Read a set of data and return a multi-dimensional array of int
    /// </summary>
    /// <param name="data">A string data from a sreadsheet</param>
    /// <returns>Multi-dimensional array of int</returns>
    public int[,] ReadSheetData (string data) {
        
        string[] correctData = data.Split(',');
        int size = correctData.Length;
        int[] firstArray = new int[size];
        for (int i = 0; i < size; i++) {
            firstArray[i] = int.Parse(correctData[i]);
        }

        int[,] finalArray = new int[(int)Mathf.Sqrt(size),(int)Mathf.Sqrt(size)];
        int count = 0;
        int rowCount = 0;
        foreach (int i in firstArray) {
            finalArray[count,rowCount] = i;
            count++;
            if (count == (int)Mathf.Sqrt(size)) {
                count = 0;
                rowCount++;
            }
        }
        return finalArray;
    }
}
