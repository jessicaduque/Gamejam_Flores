using UnityEngine;
[CreateAssetMenu(fileName = "New Organ", menuName = "Scriptable Objects/Create Organ")]
public class OrgaoSO : ScriptableObject
{
    public string organName;
    public Sprite organNormalSprite;
    public GameObject organNormalPrefab;
    public GameObject organRottenPrefab;
    public GameObject organSignPrefab;
}