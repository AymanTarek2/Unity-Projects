using System.Collections.Generic;
using UnityEngine;

public class FaceInfo
{
    private List<float> angles;
    private List<float> lengths;
    private Vector3 position;

    public List<float> Angles { get { return angles; } }
    public List<float> Lengths { get { return lengths; } }
    public Vector3 Position { get { return position; } }

    public FaceInfo(List<float> angles, List<float> lengths, Vector3 position)
    {
        this.angles = angles;
        this.lengths = lengths;
        this.position = position;
    }
}
