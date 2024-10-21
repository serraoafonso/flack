using UnityEngine;
using System;
using System.Collections.Generic;

public static class Extensions
{
    private static readonly System.Random rng = new System.Random(); // Use System.Random here

    // Extension method to shuffle a List
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list == null || list.Count <= 1)
            return; // Handle null or empty lists

        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--); // Generate a random index
                                   // Swap elements
            T value = list[n];
            list[n] = list[k];
            list[k] = value;
        }
    }
}