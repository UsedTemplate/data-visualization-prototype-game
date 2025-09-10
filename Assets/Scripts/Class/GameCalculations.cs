using UnityEngine;

public static class GameCalculations
{
    public static float MapExponential(float value, float min = 120f, float max = 210f, float exponent = 2f)
    {
        value = Mathf.Clamp(value, min, max);
        float t = (value - min) / (max - min);

        return Mathf.Pow(t, exponent);
    }

    public static Vector3 ScaleToVector3(float scale)
    {
        return new Vector3(scale, scale, scale);
    }
}
