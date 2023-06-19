using System;
using System.Collections.Generic;

class WordGuessGame
{
    static List<string> wordList = new List<string>() { "banana", "apple", "orange", "grape", "pineapple" };
    static List<string> usedWords = new List<string>();
    static int score = 0;
    static int maxScore = 0;
    static int attempts = 0;

    static void Main()
    {
        bool playAgain = true;

        Console.WriteLine("Welcome to Word Guess Game!");

        while (playAgain)
        {
            Console.WriteLine("Select the difficulty level:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.Write("Enter the level number: ");
            int level = Convert.ToInt32(Console.ReadLine());

            switch (level)
            {
                case 1:
                    attempts = 10;
                    break;
                case 2:
                    attempts = 7;
                    break;
                case 3:
                    attempts = 5;
                    break;
                default:
                    attempts = 7;
                    break;
            }

            string selectedWord = SelectWord();
            char[] hiddenWord = InitializeHiddenWord(selectedWord);
            List<char> guessedLetters = new List<char>();
            List<char> wrongLetters = new List<char>();

            Console.WriteLine("\nNew game started!");
            Console.WriteLine("Guess the word by entering one letter at a time.");
            Console.WriteLine("You have " + attempts + " attempts.");
            Console.WriteLine("Score: " + score);
            Console.WriteLine("Max Score: " + maxScore);

            while (attempts > 0)
            {
                Console.WriteLine("\nWord: " + new string(hiddenWord));
                Console.WriteLine("Attempts left: " + attempts);
                Console.WriteLine("Guessed Letters: " + string.Join(", ", guessedLetters));
                Console.WriteLine("Wrong Letters: " + string.Join(", ", wrongLetters));

                if (guessedLetters.Count > 0)
                {
                    Console.WriteLine("Do you want to receive an additional hint? (Y/N)");
                    string hintInput = Console.ReadLine();
                    if (hintInput.ToUpper() == "Y")
                    {
                        char hint = GetHint(selectedWord, guessedLetters);
                        Console.WriteLine("Hint: " + hint);
                    }
                }

                Console.Write("Enter a letter or the full word: ");
                string input = Console.ReadLine().ToLower();

                if (input.Length == 1)
                {
                    char letter = input[0];

                    if (!IsLetterValid(letter, guessedLetters))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid letter.");
                        continue;
                    }

                    guessedLetters.Add(letter);

                    if (IsLetterInWord(letter, selectedWord))
                    {
                        UpdateHiddenWord(letter, selectedWord, hiddenWord);
                        if (IsWordGuessed(hiddenWord))
                        {
                            int roundScore = CalculateRoundScore(selectedWord.Length, attempts);
                            score += roundScore;
                            if (score > maxScore)
                                maxScore = score;
                            Console.WriteLine("Congratulations! You guessed the word: " + selectedWord);
                            Console.WriteLine("Round Score: " + roundScore);
                            Console.WriteLine("Total Score: " + score);
                            break;
                        }
                    }
                    else
                    {
                        attempts--;
                        wrongLetters.Add(letter);
                        Console.WriteLine("Wrong letter!");
                    }
                }
                else if (input.Length > 1 && input == selectedWord)
                {
                    int roundScore = CalculateRoundScore(selectedWord.Length, attempts);
                    score += roundScore;
                    if (score > maxScore)
                        maxScore = score;
                    Console.WriteLine("Congratulations! You guessed the word: " + selectedWord);
                    Console.WriteLine("Round Score: " + roundScore);
                    Console.WriteLine("Total Score: " + score);
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect guess!");
                    attempts--;
                }
            }

            if (attempts == 0)
            {
                Console.WriteLine("Game over! You ran out of attempts.");
                Console.WriteLine("The word was: " + selectedWord);
            }

            usedWords.Add(selectedWord);

            Console.WriteLine("\nDo you want to play again? (Y/N)");
            string playAgainInput = Console.ReadLine();

            playAgain = (playAgainInput.ToUpper() == "Y");

            if (!playAgain)
            {
                Console.WriteLine("\nThank you for playing Word Guess Game!");
            }
        }

        Console.ReadLine();
    }

    static string SelectWord()
    {
        Console.WriteLine("\nSelect the word source:");
        Console.WriteLine("1. Predefined word list");
        Console.WriteLine("2. Enter a custom word");
        Console.Write("Enter the option number: ");
        int option = Convert.ToInt32(Console.ReadLine());

        string selectedWord = "";

        switch (option)
        {
            case 1:
                selectedWord = GetRandomWord();
                break;
            case 2:
                Console.Write("Enter a custom word: ");
                selectedWord = Console.ReadLine().ToLower();
                if (!IsValidCustomWord(selectedWord))
                {
                    Console.WriteLine("Invalid word. Using a random word instead.");
                    selectedWord = GetRandomWord();
                }
                break;
            default:
                selectedWord = GetRandomWord();
                break;
        }

        return selectedWord;
    }

    static string GetRandomWord()
    {
        Random random = new Random();
        int randomIndex = random.Next(0, wordList.Count);

        string word = wordList[randomIndex];

        if (usedWords.Contains(word) && usedWords.Count < wordList.Count)
        {
            while (usedWords.Contains(word))
            {
                randomIndex = random.Next(0, wordList.Count);
                word = wordList[randomIndex];
            }
        }

        return word;
    }

    static bool IsValidCustomWord(string word)
    {
        return !string.IsNullOrEmpty(word) && !word.Contains(" ");
    }

    static char[] InitializeHiddenWord(string word)
    {
        char[] hiddenWord = new char[word.Length];
        for (int i = 0; i < word.Length; i++)
        {
            hiddenWord[i] = '_';
        }
        return hiddenWord;
    }

    static bool IsLetterValid(char letter, List<char> guessedLetters)
    {
        return Char.IsLetter(letter) && !guessedLetters.Contains(letter);
    }

    static bool IsLetterInWord(char letter, string word)
    {
        return word.Contains(letter);
    }

    static void UpdateHiddenWord(char letter, string word, char[] hiddenWord)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == letter)
            {
                hiddenWord[i] = letter;
            }
        }
    }

    static bool IsWordGuessed(char[] hiddenWord)
    {
        foreach (char letter in hiddenWord)
        {
            if (letter == '_')
                return false;
        }
        return true;
    }

    static char GetHint(string word, List<char> guessedLetters)
    {
        foreach (char letter in word)
        {
            if (!guessedLetters.Contains(letter))
                return letter;
        }
        return '_';
    }

    static int CalculateRoundScore(int wordLength, int attempts)
    {
        int baseScore = wordLength * 10;
        int deduction = (10 - attempts) * 5;
        return Math.Max(0, baseScore - deduction);
    }
}
