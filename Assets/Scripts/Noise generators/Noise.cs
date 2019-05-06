using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public int seed;

    public Noise(int seed = -1)
    {
        SetSeed(seed);
    }

    /// <summary>
    /// Set seed to pseudo random generator. Generate random seed if it not set or equals -1.
    /// </summary>
    /// <param name="seed"></param>
    public void SetSeed(int seed = -1)
    {
        this.seed = (seed == -1) ? (int)System.DateTime.Now.Ticks : seed;
        Random.InitState(this.seed);
    }

    /// <summary>
    /// Generate 1 dimension perlin noise value.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="octaves">How many noise layers will be combined</param>
    /// <param name="frequency">Same as wavelength (x scale factor)</param>
    /// <param name="heightMultiplier">y scale factor</param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public float PerlinNoise1D(float x, int octaves = 1, float frequency = 1, float amplitude = 1f, int offset = 0)
    {
        return PerlinNoise(x, 0, octaves, frequency, amplitude, offset);
    }

    /// <summary>
    ///  Generate 2 dimension perlin noise value.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="octaves"></param>
    /// <param name="frequency"></param>
    /// <param name="heightMultiplier"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public float PerlinNoise2D(float x, float y = 0, int octaves = 1, float frequency = 1, float amplitude = 1f, int offset = 0)
    {
        return PerlinNoise(x, y, octaves, frequency, amplitude, offset);
    }

    private float PerlinNoise(float x, float y, int octaves, float frequency, float amplitude, int offset)
    {
        float result = 0;
        float maxValue = 0; // Used for normalizing result to 0.0 - 1.0

        if (octaves > 2)
        {
            for (int i = 0; i < octaves; i++)
            {
                maxValue += amplitude;
                result += Mathf.PerlinNoise(x * frequency + 0.001f + offset,
                                            y * frequency + seed + 0.001f) * amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }
            return result / maxValue;
        }
        else
        {
            return Mathf.PerlinNoise(x * frequency + 0.001f + offset,
                                     y * frequency + seed + 0.001f) * amplitude;
        }
    }
}
