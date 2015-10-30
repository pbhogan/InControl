using UnityEngine;
using System.Collections;
using InControl;
using System.Linq;
using UnityEngine.UI;

namespace JoinScreenExample
{
    public class JoinControllers : MonoBehaviour
    {

        public Button PlayMatchButton;
        public Toggle[] PlayerToggles;
        
        private int assignedPlayers;

        void Update()
        {
            Join();

            foreach (var a in GameManager.Instance.Controllers)
            {
                var control = a.GetControl(InputControlType.Start);

                if (control.IsPressed && PlayMatchButton.IsInteractable())
                    PlayMatch();
            }
        }

        void Join()
        {
            foreach (var d in InputManager.Devices)
            {
                if (d.Action1.WasPressed)
                {
                    switch (assignedPlayers)
                    {
                        case 0:
                            GameManager.Instance.Controllers.Add(d);
                            print(PlayerToggles[assignedPlayers]);
                            PlayerToggles[assignedPlayers].isOn = true;
                            assignedPlayers++;
                            PlayMatchButton.interactable = true;
                            break;

                        default:
                            var inList = GameManager.Instance.Controllers.Any(c => c == d);
                            if (!inList)
                            {
                                GameManager.Instance.Controllers.Add(d);
                                PlayerToggles[assignedPlayers].isOn = true;
                                assignedPlayers++;
                            }
                            break;
                    }
                }
            }
        }

        public void PlayMatch()
        {
            Application.LoadLevel(2);
        }

    }
}
