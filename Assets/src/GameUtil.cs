using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
public struct FloatRange
{
    private float m_min;
    private float m_max;
    public FloatRange(float min, float max)
    {
        m_min = min;
        m_max = max;
    }
    public float Min => m_min;
    public float Max => m_max;
}
public static class GameUtil 
{
    static Regex s_regexNoAlphaNumeric = new Regex(@"^[^a-zA-Z0-9]*$");
    // Check if text has no pronouncing needed for npc anim
    public static bool TextHasNoAlphaNumeric(string toCheck)
    {
        return s_regexNoAlphaNumeric.IsMatch(toCheck);
    }
    // Coountdown in seconds
    public static IEnumerator Countdown(float secondsLeft, Action onFinish = null)
    {
        float t = secondsLeft;
        while (t > 0f)
        {
            yield return new WaitForSeconds(1f);
            t--;
        }
        onFinish?.Invoke();
    }
    public static string RandomNumberID(int length = 8)
    {
        string ret = "";
        for (int i = 0; i < length; i++)
        {
            ret += UnityEngine.Random.Range(0, 10).ToString();
        }
        return ret;
    }
    // Get min and max of a angle with range thresholds
    public static FloatRange SignedAngleThresholdRange(float threshold, float margin)
    {
        float a = threshold - margin;
        float b = threshold + margin;
        float min;
        float max;
        if (a < -180f)
        {
            max = a + 360f;
            min = b;
        }
        else if (b > 180f)
        {
            min = b - 360f;
            max = a;
        }
        else
        {
            min = a;
            max = b;
        }
        return new FloatRange(min, max);
    }
    // rotate a transform by a given signed angle
    public static void SignedRotationDegree(Transform target, float signedAngle)
    {
        Vector3 newRotation = new Vector3(0f, 0f, signedAngle);
        newRotation.z = signedAngle;
        Quaternion rotateTo = Quaternion.Euler(newRotation);
        // rotate to new direction
        target.rotation = Quaternion.RotateTowards(target.rotation, rotateTo, 360f);
    }

public static void AimTowards2D(Transform toAim, Vector2 directionNormalized)
    {
        float angle = Mathf.Atan2(directionNormalized.y, directionNormalized.x) * Mathf.Rad2Deg;
        toAim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    // Spawn a gameobject a prefab reference (preferrably)
    public static T SpawnObject<T>(T spawnRef, Vector3 position, Transform parent = null) where T : MonoBehaviour
    {
        T ret = UnityEngine.Object.Instantiate(spawnRef);
        ret.transform.SetParent(parent, false);
        ret.transform.position = position;
        return ret;
    }
    public static Vector3 RandomPositionInBounds(Bounds bounds) 
    {
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
    public static Vector2 RandomPositionInBounds2D(Bounds bounds)
    {
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
}
