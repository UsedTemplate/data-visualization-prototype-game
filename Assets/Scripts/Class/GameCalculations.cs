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

    public static int GetStageFromAge(float age, float min = 20f, float max = 100f, int stages = 5)
    {
        age = Mathf.Clamp(age, min, max);
        float stageSize = (max - min) / stages;

        int stage = Mathf.FloorToInt((age - min) / stageSize);
        if (stage >= stages) stage = stages - 1;

        return stage + 1;
    }
}
