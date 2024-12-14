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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordTrainer
{
    public partial class MainWindow : Window
    {
        private int correctAnswersCount = 0; // Счетчик правильных ответов
        private int wrongAnswersCount = 0;    // Счетчик неправильных ответов
        private WordManager wordManager = new WordManager();
        private JsonManager jsonManager = new JsonManager();
        
        public MainWindow()
        {
            InitializeComponent();
            UpdateCategoryComboBox();
            wordManager.WordAdded += WordManager_WordAdded;
        }
        private void WordManager_WordAdded(object sender, EventArgs e)
        {
            // Обновляем ComboBox и ListBox при добавлении нового слова
            UpdateCategoryComboBox();
            UpdateWordListBox();
        }

        private void UpdateWordListBox()
        {
            string category = Category.Text;
            WordListBox.ItemsSource = null;
            WordListBox.ItemsSource = wordManager.GetWordsByCategory(category);
        }

        private void UpdateCategoryComboBox()
        {
            CategoryComboBox.ItemsSource = wordManager.GetCategories();
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            string russianWord = Russian.Text;
            string translation = Translation.Text;
            string category = Category.Text;

            Word word = new Word(russianWord, translation, category);

            wordManager.AddWord(word);
        }
       
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is string selectedCategory)
            {
                var wordsInCategory = wordManager.GetWordsByCategory(selectedCategory);
                WordListBox.ItemsSource = wordsInCategory; 
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            JsonManager.SaveWords(wordManager.GetAllWords());
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var words = JsonManager.LoadWords();
            foreach (var word in words)
                wordManager.AddWord(word);

            UpdateWordListBox();
            UpdateCategoryComboBox();
            
        }

        private void WordListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WordListBox.SelectedItem is string selectedWord)
            {
                // Найти выбранное слово в списке всех слов
                var word = wordManager.GetAllWords().FirstOrDefault(w => w.Russian == selectedWord);
            }
        }
       
        private void StartTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is string selectedCategory)
            {
                // Получаем слова из выбранной категории
                var words = wordManager.GetAllWords().Where(w => w.Category == selectedCategory).ToList();

                if (words.Any())
                {
                    // Создаем и открываем окно тренировки
                    var trainingWindow = new TrainingWindow(wordManager.GetAllWords());
                    trainingWindow.ShowDialog();

                    // После закрытия окна тренировки обновляем счетчики
                    correctAnswersCount += trainingWindow.CorrectAnswersCount;
                    wrongAnswersCount += trainingWindow.WrongAnswersCount;

                    RightAnswer.Text = correctAnswersCount.ToString();
                    WrongAnswer.Text = wrongAnswersCount.ToString();
                }
                else
                {
                    MessageBox.Show("Выбранная категория не содержит слов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для тренировки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveWordButton_Click(object sender, RoutedEventArgs e)
        {
            if (WordListBox.SelectedItem is string selectedWord)
            {
                // Найти выбранное слово в списке всех слов
                var wordToRemove = wordManager.GetAllWords().FirstOrDefault(w => w.Russian == selectedWord);
                if (wordToRemove != null)
                {
                    wordManager.RemoveWord(wordToRemove);
                    UpdateWordListBox(); // Обновляем список слов
                    if (CategoryComboBox.SelectedItem is string selectedCategory)
                    {
                        var wordsInCategory = wordManager.GetWordsByCategory(selectedCategory);
                        WordListBox.ItemsSource = wordsInCategory;
                    }
                    MessageBox.Show("Слово удалено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Слово не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                MessageBox.Show("Выберите слово для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
  
}
