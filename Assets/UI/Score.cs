using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Score : MonoBehaviour
    {
        public long score;
        private TextMeshProUGUI scoreText;

        private void Start()
        {
            scoreText = GetComponent<TextMeshProUGUI>();
            score = 0;
        }

        private void Update()
        {
            scoreText.text = $"Score: {score}";
        }
    }
}