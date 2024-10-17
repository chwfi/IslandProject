using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Material/MaterialDatabase")]
public class MaterialDatabase : ScriptableObject
{
    [SerializeField] private List<InGameMaterial> _materials;
    public IReadOnlyList<InGameMaterial> Materials => _materials;
}
