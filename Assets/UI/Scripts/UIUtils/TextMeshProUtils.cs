using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.UIUtils
{
    public static class TextMeshProUtils
    {
        public static async UniTask SetTextGradually(TextMeshProUGUI tmp, string text, float secondsOnOneSymbol = 0.05f)
        {
            tmp.text = String.Empty;
            foreach (var symbol in text)
            {
                tmp.text += symbol;
                await UniTask.Delay(TimeSpan.FromSeconds(secondsOnOneSymbol));
            }
        }

        public static async UniTask SetNumberTextUsingRandomNumbers(TextMeshProUGUI tmp, string text, float randomTime)
        {
            tmp.text = String.Empty;
            float currentTime = 0;
            while (randomTime > currentTime)
            {
                tmp.text = Random.Range(10, 99).ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
                currentTime += 0.05f;
            }
            tmp.text = text;
        }

        public static async UniTask DecipherMessageAnimation(TextMeshProUGUI tmp, string text, float secondsOnOneSymbol = 0.05f)
        {
            string cipher = String.Empty;
            foreach (var symbol in text)
            {
                cipher += Random.Range(0, 2);
            }

            tmp.text = cipher;
            for (int i = 0; i < cipher.Length; i++)
            {
                var chars = cipher.ToCharArray();
                chars[i] = text[i];
                for (int j = i+1; j < cipher.Length; j++)
                {
                    chars[j] = Char.Parse(Random.Range(0, 2).ToString());
                }
                cipher = chars.ArrayToString();
                tmp.text = cipher;
                await UniTask.Delay(TimeSpan.FromSeconds(secondsOnOneSymbol));
            }
        }
    }
}