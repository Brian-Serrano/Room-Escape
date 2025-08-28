using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class RoomEscapeUtils
{
    // Floor Offsets (0, 3, 7.5), (0, 3, 2.5), (0, 3, -2.5), (0, 3, -7.5) id = 1, 2, 3, 4
    // Spinning Obstacle Offsets (0, 6, 0), (0, 6, 5), (0, 6, -5) id = 5, 6, 7
    // Up Down Obstacle Offsets (0, 4.5, 1.5), (0, 4.5, -1.5), (0, 4.5, 3.5), (0, 4.5, 6.5), (0, 4.5, -3.5), (0, 4.5, -6.5) id = 8, 9, 10
    // Forward Backward Obstacle Offsets (0, 4.5, 0), (0, 4.5, 5), (0, 4.5, -5) id = 11, 12, 13
    // Door Obstacle Offsets (0, 8, 7.5), (0, 8, 2.5), (0, 8, -2.5), (0, 8, -7.5) id = 14, 15, 16, 17
    // 1 Spike (0, 6, 7.5), (0, 6, 2.5), (0, 6, -2.5), (0, 6, -7.5) id = 18, 19, 20, 21
    // 2 Spike (-2.5 and 2.5, 6, 7.5), (-2.5 and 2.5, 6, 2.5), (-2.5 and 2.5, 6, -2.5), (-2.5 and 2.5, 6, -7.5) id = 22, 23, 24, 25
    // 3 Spike (-3.5, 0 and 3.5, 6, 7.5), (-3.5, 0 and 3.5, 6, 2.5), (-3.5, 0 and 3.5, 6, -2.5), (-3.5, 0 and 3.5, 6, -7.5) id = 26, 27, 28, 29
    // Acid (0, 2, 7.5), (0, 2, 2.5), (0, 2, -2.5), (0, 2, -7.5) id = 30, 31, 32, 33

    // Floor index = 0
    // Spinning Obstacle index = 1
    // UD Obstacle index = 2
    // FW Obstacle index = 3
    // Door Obstacle index = 4
    // Spike index = 5
    // Acid index = 6
    // Room index = 7

    public static string Truncate(this string value, int length)
    {
        if (value.Length > length)
        {
            return value[..length];
        }

        return value;
    }

    public static List<List<float>> GetObstacleSpawnPoints()
    {
        // [0] Game Object, [1] X Position, [2] Y Position, [3] Z Position
        return new List<List<float>>()
        {
            new List<float>() {0f, 0f, 3f, 7.5f},
            new List<float>() {0f, 0f, 3f, 2.5f},
            new List<float>() {0f, 0f, 3f, -2.5f},
            new List<float>() {0f, 0f, 3f, -7.5f},
            new List<float>() {1f, 0f, 6f, 0f},
            new List<float>() {1f, 0f, 6f, 5f},
            new List<float>() {1f, 0f, 6f, -5f},
            new List<float>() {2f, 0f, 4.5f, 1.5f, 2f, 0f, 4.5f, -1.5f},
            new List<float>() {2f, 0f, 4.5f, 3.5f, 2f, 0f, 4.5f, 6.5f},
            new List<float>() {2f, 0f, 4.5f, -3.5f, 2f, 0f, 4.5f, -6.5f},
            new List<float>() {3f, 0f, 4.5f, 0f},
            new List<float>() {3f, 0f, 4.5f, 5f},
            new List<float>() {3f, 0f, 4.5f, -5f},
            new List<float>() {4f, 0f, 8f, 7.5f},
            new List<float>() {4f, 0f, 8f, 2.5f},
            new List<float>() {4f, 0f, 8f, -2.5f},
            new List<float>() {4f, 0f, 8f, -7.5f},
            new List<float>() {5f, 0f, 6.5f, 7.5f},
            new List<float>() {5f, 0f, 6.5f, 2.5f},
            new List<float>() {5f, 0f, 6.5f, -2.5f},
            new List<float>() {5f, 0f, 6.5f, -7.5f},
            new List<float>() {5f, -2.5f, 6.5f, 7.5f, 5f, 2.5f, 6.5f, 7.5f},
            new List<float>() {5f, -2.5f, 6.5f, 2.5f, 5f, 2.5f, 6.5f, 2.5f},
            new List<float>() {5f, -2.5f, 6.5f, -2.5f, 5f, 2.5f, 6.5f, -2.5f},
            new List<float>() {5f, -2.5f, 6.5f, -7.5f, 5f, 2.5f, 6.5f, -7.5f},
            new List<float>() {5f, -3.5f, 6.5f, 7.5f, 5f, 0f, 6.5f, 7.5f, 5f, 3.5f, 6.5f, 7.5f},
            new List<float>() {5f, -3.5f, 6.5f, 2.5f, 5f, 0f, 6.5f, 2.5f, 5f, 3.5f, 6.5f, 2.5f},
            new List<float>() {5f, -3.5f, 6.5f, -2.5f, 5f, 0f, 6.5f, -2.5f, 5f, 3.5f, 6.5f, -2.5f},
            new List<float>() {5f, -3.5f, 6.5f, -7.5f, 5f, 0f, 6.5f, -7.5f, 5f, 3.5f, 6.5f, -7.5f},
            new List<float>() {6f, 0f, 2f, 7.5f},
            new List<float>() {6f, 0f, 2f, 2.5f},
            new List<float>() {6f, 0f, 2f, -2.5f},
            new List<float>() {6f, 0f, 2f, -7.5f}
        };
    }

    public static List<List<int>> CreateObstacleSpawns(List<int> nums1, List<int> nums2)
    {
        return new List<List<int>>()
        {
            new List<int>() {4, 32, 31, 1, nums1[Random.Range(0, nums1.Count)], nums2[Random.Range(0, nums2.Count)] + 3, nums2[Random.Range(0, nums2.Count)]},
            new List<int>() {33, 32, 2, 1, nums1[Random.Range(0, nums1.Count)] + 2, nums2[Random.Range(0, nums2.Count)] + 1, nums2[Random.Range(0, nums2.Count)]},
            new List<int>() {4, 3, 31, 30, nums1[Random.Range(0, nums1.Count)] + 1, nums2[Random.Range(0, nums2.Count)] + 3, nums2[Random.Range(0, nums2.Count)] + 2},
            new List<int>() {33, 32, 31, 30, nums1[Random.Range(0, nums1.Count)] + 1, nums1[Random.Range(0, nums1.Count)] + 2},
            new List<int>() {4, 3, 2, 1, nums2[Random.Range(0, nums2.Count)] + 3, nums2[Random.Range(0, nums2.Count)] + 2, nums2[Random.Range(0, nums2.Count)] + 1, nums2[Random.Range(0, nums2.Count)]}
        };
    }
}
