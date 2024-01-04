﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simon.Models
{
    public class Game(int? difficulty)
    {
        // TODO:  Figure out how to play sounds in a MAUI app
        private readonly Random _random = new();
        private readonly List<string> _buttonColors = ["Red", "Blue", "Green", "Yellow"];
        public List<string> Pattern { get; set; } = [];
        public List<string> UserPattern { get; set; } = [];
        public int Difficulty { get; set; } = difficulty ?? 1;
        public int Level { get; set; } = 0;
        public bool FirstRun { get; set; } = true;
        public bool Winner { get; set; } = false;
        public bool GameOver { get; set; } = false;

        public void NextSequence()
        {
            string randomColor = _buttonColors[_random.Next(_buttonColors.Count)];
            Pattern.Add(randomColor);
            Level++;
        }

        public bool PatternsAreEqualSize()
        {
            if (Pattern.Count == UserPattern.Count) return true;
            else return false;
        }

        public bool CheckAnswer(int currentValue)
        {
            if (!string.IsNullOrEmpty(UserPattern.ElementAtOrDefault(currentValue)) && !string.IsNullOrEmpty(Pattern.ElementAtOrDefault(currentValue)))
            {
                string userValue = UserPattern.ElementAt(currentValue);
                string gameValue = Pattern.ElementAt(currentValue);
                if (gameValue == userValue) return true;
                else return false;
            }
            else return false;
        }

        public void ResetUserPattern()
        {
            UserPattern.Clear();
        }

        public bool IsWinner()
        {
            int winner = 8;
            switch (Difficulty)
            {
                case 2:
                    winner = 16;
                    break;
                case 3:
                    winner = 24;
                    break;
                case 4:
                    winner = 32;
                    break;
                default:
                    break;
            }
            if (Pattern.Count == winner)
            {
                Winner = true;
                return true;
            }
            else return false;
        }

        public int GetDelay()
        {
            switch (Difficulty)
            {
                case 1:
                    if (Pattern.Count > 2) return 600;
                    if (Pattern.Count > 4) return 300;
                    if (Pattern.Count > 6) return 0;
                    return 800;
                case 2:
                    if (Pattern.Count > 4) return 600;
                    if (Pattern.Count > 8) return 300;
                    if (Pattern.Count > 12) return 0;
                    return 800;
                case 3:
                    if (Pattern.Count > 6) return 600;
                    if (Pattern.Count > 12) return 300;
                    if (Pattern.Count > 18) return 0;
                    return 800;
                case 4:
                    if (Pattern.Count > 8) return 600;
                    if (Pattern.Count > 16) return 300;
                    if (Pattern.Count > 24) return 0;
                    return 800;
                default:
                    return 800;
            }
        }

        public void ResetGame(int difficulty)
        {
            Difficulty = difficulty;
            Pattern.Clear();
            UserPattern.Clear();
            Level = 0;
            FirstRun = true;
            Winner = false;
            GameOver = false;
        }

        // TODO:  Allow saving current and loading saved game?
    }
}