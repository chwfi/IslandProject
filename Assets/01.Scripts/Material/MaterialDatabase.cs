using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Material/MaterialDatabase")]
public class MaterialDatabase : ScriptableObject
{
    [SerializeField] private List<InGameMaterial> _cropsMaterials;
    [SerializeField] private List<InGameMaterial> _stuffMaterials;
    public IReadOnlyList<InGameMaterial> CropsMaterials => _cropsMaterials;
    public IReadOnlyList<InGameMaterial> StuffMaterials => _stuffMaterials;
}
