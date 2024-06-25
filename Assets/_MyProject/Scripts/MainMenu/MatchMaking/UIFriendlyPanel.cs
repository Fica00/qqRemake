using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendlyPanel : MonoBehaviour
{
   [SerializeField] private GameObject holder;
   [SerializeField] private TMP_InputField roomName;
   [SerializeField] private Button continueButton;
   [SerializeField] private Button closeButton;
   [SerializeField] private UIPVPPanel pvpPanel;

   public void Setup()
   {
      ManageInteractables(true);
      holder.SetActive(true);
   }
   
   private void OnEnable()
   {
      continueButton.onClick.AddListener(CreateFriendlyMatch);
      closeButton.onClick.AddListener(Close);
   }

   private void OnDisable()
   {
      continueButton.onClick.RemoveListener(CreateFriendlyMatch);
      closeButton.onClick.RemoveListener(Close);
   }

   private void CreateFriendlyMatch()
   {
      string _roomName = roomName.text;
      if (!IsRoomNameValid(_roomName))
      {
         return;
      }
      ManageInteractables(false);
      SocketServerCommunication.Instance.JoinFriendlyMatch(_roomName);
      Close();
      pvpPanel.Setup();
   }

   private bool IsRoomNameValid(string _roomName)
   {
      _roomName = _roomName.Trim();
      if (string.IsNullOrEmpty(_roomName))
      {
         DialogsManager.Instance.OkDialog.Setup("Please enter room name");
         return false;
      }

      if (_roomName.Length < 4)
      {
         DialogsManager.Instance.OkDialog.Setup("Room name must contain at least 4 characters");
         return false;
      }

      if (_roomName.Length>10)
      {
         DialogsManager.Instance.OkDialog.Setup("Room name must contain maximum 10 characters");
         return false;
      }

      return true;
   }

   private void Close()
   {
      holder.SetActive(false);
   }

   private void ManageInteractables(bool _status)
   {
      continueButton.interactable = _status;
      closeButton.interactable = _status;
   }
}
