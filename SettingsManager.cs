using UnityEngine;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    public static int filter;

    public VolumeProfile[] filters;

    public Volume volume;

    public void ChangeFilter(int _index)
    {
        volume.profile = filters[_index];
    }
}