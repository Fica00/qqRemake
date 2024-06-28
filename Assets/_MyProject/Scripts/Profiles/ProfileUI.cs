using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] private Button close;
    [SerializeField] private List<StatisticDisplay> statisticDisplays;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Image avatarDisplay;
    [SerializeField] private AvatarDisplay avatarPrefab;
    [SerializeField] private Transform avatarHolder;

    private List<GameObject> shownObjects = new();

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        nameInput.text = DataManager.Instance.PlayerData.Name;
        PlayerData.UpdatedSelectedAvatar += ShowSelectedAvatar;
        AvatarDisplay.OnClicked += SelectAvatar;
        
        ShowSelectedAvatar();
        ShowStatistics();
        ShowOwnedAvatars();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedSelectedAvatar -= ShowSelectedAvatar;
        AvatarDisplay.OnClicked -= SelectAvatar;
        close.onClick.RemoveListener(Close);

        foreach (var _shownObject in shownObjects)
        {
            Destroy(_shownObject);
        }
        
        shownObjects.Clear();
    }

    private void Close()
    {
        if (!TryUpdateName())
        {
            return;
        }
        
        SceneManager.Instance.LoadSettingsPage();
    }

    private void ShowStatistics()
    {
        statisticDisplays[0].Setup("Games played",DataManager.Instance.PlayerData.Statistics.GamesPlayed);
        statisticDisplays[1].Setup("Games won",DataManager.Instance.PlayerData.Statistics.AmountOfGamesWon);
        statisticDisplays[2].Setup("Qoomons owned",DataManager.Instance.PlayerData.OwnedQoomons.Count);
    }
    
    private bool TryUpdateName()
    {
        string _name = nameInput.text;
        if (string.IsNullOrEmpty(_name))
        {
            DialogsManager.Instance.OkDialog.Setup("Please enter name");
            return false;
        }

        if (_name.Length < 3 || _name.Length > 10)
        {
            DialogsManager.Instance.OkDialog.Setup("Name must contain more than 3 characters and less than 10");
            return false;
        }

        if (DataManager.Instance.PlayerData.Name == _name)
        {
            return true;
        }

        DataManager.Instance.PlayerData.Name = _name;
        return true;
    }

    private void ShowSelectedAvatar()
    {
        AvatarSo _avatar = AvatarSo.Get(DataManager.Instance.PlayerData.SelectedAvatar);
        avatarDisplay.sprite = _avatar.Sprite;
    }

    private void ShowOwnedAvatars()
    {
        foreach (var _ownedAvatar in DataManager.Instance.PlayerData.OwnedAvatars)
        {
            AvatarDisplay _avatarDisplay = Instantiate(avatarPrefab, avatarHolder);
            AvatarSo _avatarSo = AvatarSo.Get(_ownedAvatar);
            _avatarDisplay.Setup(_avatarSo,false);
            shownObjects.Add(_avatarDisplay.gameObject);
        }
        
        foreach (var _avatar in AvatarSo.Get().ToList().OrderBy(_avatar => _avatar.Id).ToList())
        {
            if (DataManager.Instance.PlayerData.OwnedAvatars.Contains(_avatar.Id))
            {
                continue;
            }
            AvatarDisplay _avatarDisplay = Instantiate(avatarPrefab, avatarHolder);
            _avatarDisplay.Setup(_avatar);
            _avatarDisplay.GreyOut();
            shownObjects.Add(_avatarDisplay.gameObject);
        }
    }
    
    private void SelectAvatar(AvatarSo _avatar)
    {
        if (DataManager.Instance.PlayerData.SelectedAvatar == _avatar.Id)
        {
            DialogsManager.Instance.OkDialog.Setup("This avatar is already selected");
            return;
        }
        
        DataManager.Instance.PlayerData.SelectedAvatar = _avatar.Id;
    }
}
