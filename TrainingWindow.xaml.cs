using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WordTrainer
{
    public partial class TrainingWindow : Window
    {
        private List<Word> words;
        private int currentWordIndex = 0;
        private Random random = new Random();
        public int CorrectAnswersCount { get; private set; }
        public int WrongAnswersCount { get; private set; }

        public TrainingWindow(List<Word> words)
        {
            InitializeComponent();
            this.words = words;
            LoadNextWord();
        }

        private void LoadNextWord()
        {
            if (currentWordIndex >= words.Count)
            {
                MessageBox.Show("Тренировка завершена!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }

            var currentWord = words[currentWordIndex];
            WordText.Text = currentWord.Russian;

            // Генерация вариантов ответа
            var options = new List<string> { currentWord.Translation };

            var allWords = words; 

            while (options.Count < 4)
            {
                var randomWord = allWords[random.Next(allWords.Count)];
                if (!options.Contains(randomWord.Translation) && randomWord != currentWord) 
                    options.Add(randomWord.Translation);
            }

            // Перемешиваем варианты
            options = options.OrderBy(x => random.Next()).ToList();

            // Устанавливаем текст на кнопки
            Variant1Button.Content = options[0];
            Variant2Button.Content = options[1];
            Variant3Button.Content = options[2];
            Variant4Button.Content = options[3];
        }

        private void VariantButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedTranslation = button.Content.ToString();
            var correctTranslation = words[currentWordIndex].Translation;

            if (selectedTranslation == correctTranslation)
            {
                button.Background = Brushes.Green;
                CorrectAnswersCount++;
            }
            else
            {
                button.Background = Brushes.Red;
                WrongAnswersCount++;

                // Подсветить правильный ответ
                HighlightCorrectAnswer(correctTranslation);
            }

            currentWordIndex++;
            Task.Delay(1000).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    ResetButtonColors();
                    LoadNextWord();
                });
            });
        }
        private void HighlightCorrectAnswer(string correctTranslation)
        {
            foreach (var button in new[] { Variant1Button, Variant2Button, Variant3Button, Variant4Button })
            {
                if (button.Content.ToString() == correctTranslation)
                {
                    button.Background = Brushes.Green;
                }
            }
        }
        private void ResetButtonColors()
        {
            foreach (var button in new[] { Variant1Button, Variant2Button, Variant3Button, Variant4Button })
            {
                button.Background = Brushes.LightGray; // Цвет по умолчанию
            }
        }
    }
}
