using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/GuideDatabase")]
public class GuideDatabase : ScriptableObject
{
    [SerializeField] private List<GuideQuest> _guides;

    public List<GuideQuest> Guides => _guides;
}