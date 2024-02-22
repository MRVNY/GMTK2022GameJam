using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FontModels", order = 1)]
public class FontModels : ScriptableObject
{
    public List<Mesh> meshes;
}
