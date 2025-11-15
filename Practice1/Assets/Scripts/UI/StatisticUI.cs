using TMPro;
using UnityEngine;

namespace UI
{
    public class StatisticUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI missText;
        
        private int _score;
        private int _missing;

        public void UpScore()
        {
            scoreText.text = $"Попаданий: {++_score}";
        }

        public void UpMiss()
        {
            missText.text = $"Промахов: {++_missing}";
        }
    }
}