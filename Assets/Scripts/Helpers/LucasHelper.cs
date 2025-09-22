using UnityEngine;

/// <summary>
/// A class that helps with various random tasks
/// Might get split off in the future but not for this project
/// </summary>
public static class LucasHelper
{
    /// <summary>
    /// Exclusive. Evenly splits a value into regions. Starts at 0.
    /// </summary>
    /// <param name="minValue">Minimum value of a float</param>
    /// <param name="maxValue">Maximum value of a float</param>
    /// <param name="value">The actual float value</param>
    /// <param name="returnValues">The maximum amount of values you can recieve. Exclusive.</param>
    /// <returns></returns>
    public static int Distribute(float minValue, float maxValue, float value, int returnValues)
    {
        //Fastest Error Collection in the West
        if(minValue > maxValue)
        {
            throw new System.Exception("Min Value greater than Max Value");
        }

        if (value > maxValue)
        {
            throw new System.Exception("Value greater than Max Value");
        }

        if(value < minValue)
        {
            throw new System.Exception("Value less than MinValue");
        }

        int finalValue = 0;

        float totalValue = minValue - maxValue;
        float stepValue = Mathf.Abs(totalValue / (float)returnValues);

        //Actually do the math
        for (int i = 0; i < returnValues; i++)
        {
            if(value < (i * stepValue) + minValue)
            {
                finalValue = i;
                break;
            }
        }
        return finalValue;
    }
}
