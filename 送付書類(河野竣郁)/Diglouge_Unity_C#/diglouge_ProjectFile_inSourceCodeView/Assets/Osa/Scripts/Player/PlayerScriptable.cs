using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Battle/Player")]

public class PlayerScriptable : ScriptableObject
{
    public int baseHP = 100;
    public int baseDefencePower = 1;

    public Battle.Player playerPrefab;
    public Transform spawnPoint;
}