using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TootTallyCore.Graphics;
using TootTallyCore.Graphics.Animations;
using TootTallyCore.Utils.TootTallyNotifs;
using UnityEngine;
using UnityEngine.UI;
using static TootTallyMultiplayer.MultiplayerSystem;

namespace TootTallyMultiplayer.MultiplayerCore.InputPrompts
{
    public class LobbySettingsInputPrompt
    {
        public GameObject gameObject;
        private GameObject _container, _topContainer, _bottomContainer, _lobbySettingsContainer, _otherSettingsContainer;
        private CustomButton _cancelButton, _confirmButton;
        private Toggle _freemodButton, _teamsButton;

        private TootTallyAnimation _lobbySettingsAnimation;
        public LobbySettingsInputPrompt(Transform canvasTransform, Action<string, string, string, string, bool, bool> OnConfirm)
        {
            gameObject = MultiplayerGameObjectFactory.GetBorderedVerticalBox(new Vector2(890, 490), 5, canvasTransform);
            _container = gameObject.transform.GetChild(0).gameObject;
            _container.GetComponent<Image>().color = new Color(.1f, .1f, .1f, .85f);
            var containerLayout = _container.GetComponent<VerticalLayoutGroup>();
            containerLayout.spacing = 10f;
            containerLayout.padding = new RectOffset(5, 5, 25, 5);

            //Center to screen
            var promptRect = gameObject.GetComponent<RectTransform>();
            promptRect.anchorMin = promptRect.anchorMax = promptRect.pivot = Vector2.one / 2f;

            _topContainer = MultiplayerGameObjectFactory.GetHorizontalBox(new Vector2(0, 360), _container.transform);

            //Lobby Settings
            _lobbySettingsContainer = MultiplayerGameObjectFactory.GetVerticalBox(new Vector2(545, 0), _topContainer.transform);
            var lobbyLayout = _lobbySettingsContainer.GetComponent<VerticalLayoutGroup>();
            lobbyLayout.childAlignment = TextAnchor.UpperLeft;
            lobbyLayout.spacing = 10f;

            var lobbySettingsText = GameObjectFactory.CreateSingleText(_lobbySettingsContainer.transform, "LobbySettingsText", "Lobby Settings");
            lobbySettingsText.fontSize = 42;
            lobbySettingsText.margin = new Vector4(5,0);
            lobbySettingsText.rectTransform.sizeDelta = new Vector2(545, 70);
            lobbySettingsText.alignment = TMPro.TextAlignmentOptions.MidlineLeft;

            var nameHBox = MultiplayerGameObjectFactory.GetHorizontalBox(new Vector2(0, 55), _lobbySettingsContainer.transform);
            var hlayout = nameHBox.GetComponent<HorizontalLayoutGroup>();
            hlayout.spacing = 8f;
            hlayout.childAlignment = TextAnchor.MiddleLeft;

            var descHBox = GameObject.Instantiate(nameHBox, _lobbySettingsContainer.transform);
            var passwordHBox = GameObject.Instantiate(nameHBox, _lobbySettingsContainer.transform);
            var maxCountHBox = GameObject.Instantiate(nameHBox, _lobbySettingsContainer.transform);

            var nameLabel = GameObjectFactory.CreateSingleText(nameHBox.transform, "NameLabel", "Name:");
            nameLabel.rectTransform.sizeDelta = new Vector2(140, 55);
            nameLabel.alignment = TextAlignmentOptions.BottomLeft;
            var nameInput = MultiplayerGameObjectFactory.CreateInputField(nameHBox.transform, "NameInputField", new Vector2(350, 30), 24, Plugin.Instance.SavedLobbyTitle.Value, false);

            var descLabel = GameObject.Instantiate(nameLabel, descHBox.transform);
            descLabel.name = "DescLabel"; descLabel.text = "Description:";
            var descInput = MultiplayerGameObjectFactory.CreateInputField(descHBox.transform, "DescInputField", new Vector2(350, 30), 24, Plugin.Instance.SavedLobbyDesc.Value, false);

            var passLabel = GameObject.Instantiate(nameLabel, passwordHBox.transform);
            passLabel.name = "PasswordLabel"; passLabel.text = "Password:";
            var passwordInput = MultiplayerGameObjectFactory.CreateInputField(passwordHBox.transform, "PassInputField", new Vector2(350, 30), 24, "", true);

            var maxPlayerLabel = GameObject.Instantiate(nameLabel, maxCountHBox.transform);
            maxPlayerLabel.name = "MaxPlayerLabel"; maxPlayerLabel.text = "Max Player:";
            var maxPlayerInput = MultiplayerGameObjectFactory.CreateInputField(maxCountHBox.transform, "MaxPlayerInputField", new Vector2(350, 30), 24, Plugin.Instance.SavedLobbyMaxPlayer.Value.ToString(), false);

            //Other Settings
            _otherSettingsContainer = MultiplayerGameObjectFactory.GetVerticalBox(new Vector2(245, 0), _topContainer.transform);
            var otherLayout = _otherSettingsContainer.GetComponent<VerticalLayoutGroup>();
            otherLayout.childAlignment = TextAnchor.UpperLeft;
            otherLayout.childForceExpandWidth = otherLayout.childControlWidth = false;

            var otherSettingsText = GameObjectFactory.CreateSingleText(_otherSettingsContainer.transform, "OtherSettingsText", "Other");
            otherSettingsText.fontSize = 42;
            otherSettingsText.rectTransform.sizeDelta = new Vector2(245, 55);
            otherSettingsText.alignment = TMPro.TextAlignmentOptions.Midline;

            var ButtonsLayout = _otherSettingsContainer.GetComponent<VerticalLayoutGroup>();
            ButtonsLayout.spacing = 8f;
            ButtonsLayout.childControlHeight = ButtonsLayout.childForceExpandHeight = false;

            _freemodButton = MultiplayerGameObjectFactory.CreateToggle(_otherSettingsContainer.transform, "FreemodToggle", new Vector2(60, 60), "freemod");
            _teamsButton = MultiplayerGameObjectFactory.CreateToggle(_otherSettingsContainer.transform, "TeamsToggle", new Vector2(60, 60), "teams");

            //Buttons Container
            _bottomContainer = MultiplayerGameObjectFactory.GetHorizontalBox(new Vector2(0, 90), _container.transform);
            var buttonsLayout = _bottomContainer.GetComponent<HorizontalLayoutGroup>();
            buttonsLayout.spacing = 40f;
            buttonsLayout.childControlHeight = buttonsLayout.childForceExpandHeight = false;

            _confirmButton = GameObjectFactory.CreateCustomButton(_bottomContainer.transform, Vector2.zero, new Vector2(170, 65), "Confirm", "ConfirmButton", delegate { OnConfirm?.Invoke(nameInput.text, descInput.text, passwordInput.text, maxPlayerInput.text, _freemodButton.isOn, _teamsButton.isOn); });
            _cancelButton = GameObjectFactory.CreateCustomButton(_bottomContainer.transform, Vector2.zero, new Vector2(170, 65), "Cancel", "CancelButton", Hide);
        } 

        public void Show()
        {
            _lobbySettingsAnimation?.Dispose();
            gameObject.SetActive(true);
            _lobbySettingsAnimation = TootTallyAnimationManager.AddNewScaleAnimation(gameObject, Vector3.one, .8f, new SecondDegreeDynamicsAnimation(1.8f, .95f, 1), delegate { _lobbySettingsAnimation = null; });
        }

        public void Hide() => Hide(true);

        public void Hide(bool animate = true)
        {
            _lobbySettingsAnimation?.Dispose();
            if (animate)
            {
                _lobbySettingsAnimation = TootTallyAnimationManager.AddNewScaleAnimation(gameObject, Vector2.zero, .8f, new SecondDegreeDynamicsAnimation(2.2f, 1f, 1), delegate
                {
                    _lobbySettingsAnimation = null;
                    gameObject.SetActive(false);
                });
            }
            else
                gameObject.SetActive(false);
            
        }
    }
}
