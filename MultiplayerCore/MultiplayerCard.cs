using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TootTallyMultiplayer.APIService.MultSerializableClasses;

namespace TootTallyMultiplayer
{
    public class MultiplayerCard : MonoBehaviour
    {
        public MultiplayerUserInfo user;
        public MultiplayerTeamState team;
        public TMP_Text textName, textState, textRank;
        public GameObject teamChanger;
        public Image image;
        public Image containerImage;
        public Transform container;
        private Color _defaultColor, _defaultContainerColor;

        public void Init()
        {
            container = transform.GetChild(1);
            textName = container.Find("Name").GetComponent<TMP_Text>();
            textState = container.Find("State").GetComponent<TMP_Text>();
            textRank = container.Find("Rank").GetComponent<TMP_Text>();

            teamChanger = transform.GetChild(0).gameObject;
            team = MultiplayerTeamState.Red;
            var mainColor = new Color(1, 0, 0);
            UpdateTeamColor(mainColor, "R");
            teamChanger.GetComponent<Button>().onClick.AddListener(() => 
            {
                if (team == MultiplayerTeamState.Red)
                {
                    UpdateTeamColor(new Color(0, 0, 1), "B");
                    team = MultiplayerTeamState.Blue;
                }
                else
                {
                    UpdateTeamColor(new Color(1, 0, 0), "R");
                    team = MultiplayerTeamState.Red;
                    
                }
            });

            image = gameObject.GetComponent<Image>();
            _defaultColor = image.color;

            containerImage = container.GetComponent<Image>();
            _defaultContainerColor = containerImage.color;
        }

        private void UpdateTeamColor(Color mainColor, string text)
        {
            teamChanger.gameObject.GetComponent<Button>().colors = new ColorBlock
            {
                normalColor = mainColor,
                highlightedColor = new Color(mainColor.r + 0.3f, mainColor.g + 0.3f, mainColor.b + 0.3f),
                pressedColor = new Color(mainColor.r + 0.6f, mainColor.g + 0.6f, mainColor.b + 0.6f),
                colorMultiplier = 1
            };
            teamChanger.gameObject.GetComponentInChildren<Text>().text = text;
        }
        public void ResetImageColor()
        {
            image.color = _defaultColor;
            containerImage.color = _defaultContainerColor;
        }

        public void UpdateUserInfo(MultiplayerUserInfo user, string state = null)
        {
            this.user = user;
            ResetImageColor();
            SetTexts(user.username, state ?? user.state, user.rank);
        }

        public void SetTexts(string username, string state, int rank)
        {
            textName.text = username;
            textState.text = state;
            textRank.text = $"#{rank}";
        }
    }
}
