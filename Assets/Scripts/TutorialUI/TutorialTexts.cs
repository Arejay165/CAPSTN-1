using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Tutorial Script", menuName = "TutsScript")]
public class TutorialTexts : ScriptableObject
{
    public List<string> instructions;
    public List<bool> hasArrow;
    public bool canSpawnCustomers;
    public bool canSpawnButtons;
}
