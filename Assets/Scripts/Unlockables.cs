using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlockables", menuName = "Mediocrity/Unlockables", order = 0)]
public class Unlockables : ScriptableObject
{
    public List<Hat> hats;
    public List<CharacterSkin> skins;
    public List<GameObject> playablePrefabs; // prefabs of skinned characters
}