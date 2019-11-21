using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Interface
{
    public class HUD : MonoBehaviour
    {
        public Slider sanity;
        public Slider power;
        public TextMeshProUGUI remainingEnemies;
        public TextMeshProUGUI currentWave;
        public Button restart;
        public Button quitGame;

        public Image wave1;
        public Image wave2;
        public Image wave3;
        public Image wave4;
        public Image wave5;

        public Transform horizontalLayout;

        private void Awake()
        {
            restart.onClick.AddListener(Restart);
            quitGame.onClick.AddListener(QuitGame);
            Game.Hud = this;
            Game.Hud.power.value = 0;
        }

        public void WaveCount()
        {
            foreach (Transform child in horizontalLayout)
            {
                Destroy(child.gameObject);
            }
            var rest = Game.Controller.currentWave % 5;
            int multiplesOfFive = Game.Controller.currentWave / 5;
            for (int i = 0; i < multiplesOfFive; i++)
            {
                Instantiate(wave5,horizontalLayout);
            }



            switch (rest)
            {
                case 0:
                    return;
                case 1:
                    Instantiate(wave1,horizontalLayout);
                    return;
                case 2:
                    Instantiate(wave2,horizontalLayout);
                    return;
                case 3:
                    Instantiate(wave3,horizontalLayout);
                    return;
                case 4:
                    Instantiate(wave4,horizontalLayout);
                    return;
            }
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}