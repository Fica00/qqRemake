using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAvatar", menuName = "ScriptableObject/Avatar")]
public class AvatarSo : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    
    private static List<AvatarSo> allAvatars;

    public static void Init()
    {
        allAvatars = Resources.LoadAll<AvatarSo>("Avatars").ToList();
    }

    public static AvatarSo Get(int _id)
    {
        return allAvatars.Find(_avatar => _avatar.Id == _id);
    }    
    
    public static List<AvatarSo> Get()
    {
        return allAvatars;
    }

    public static AvatarSo GetRandom()
    {
        return allAvatars[Random.Range(0, allAvatars.Count)];
    }
}
