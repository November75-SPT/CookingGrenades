using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookingGrenades.Utils
{
    public class MathUtils
    {
        // made by Grok AI
        // Generates a normally distributed random number using Box-Muller transform
        public static float GenerateNormalRandomBoxMuller(float mean, float stdDev)
        {
            float u1 = UnityEngine.Random.value;  // First uniform random number (0 to 1)
            float u2 = UnityEngine.Random.value;  // Second uniform random number (0 to 1)
            // Apply Box-Muller transform to get a standard normal variable (mean 0, std dev 1)
            float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
            return mean + stdDev * z;  // Scale and shift to desired mean and std dev
        }
        // Generates a fast approximate normal random number using two uniform variables
        public static float GenerateNormalRandomFast(float mean, float stdDev)
        {
            float u1 = UnityEngine.Random.value;  // First uniform random number (0 to 1)
            float u2 = UnityEngine.Random.value;  // Second uniform random number (0 to 1)
            // Sum two uniform variables and scale to approximate normal distribution
            float z = (u1 + u2 - 1.0f) * 1.732f;  // Mean 0, std dev ~1 (sqrt(12)/2)
            return mean + stdDev * z;  // Scale and shift to desired mean and std dev
        }
    }
}