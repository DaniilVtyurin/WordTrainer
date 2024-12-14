using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordTrainer
{
    public class WordManager
    {
        private List<Word> words = new List<Word>();

        // Определение события
        public event EventHandler WordAdded;

        public void AddWord(Word word)
        {
            words.Add(word);
            // Вызываем событие после добавления слова
            OnWordAdded(EventArgs.Empty);
        }
        public void RemoveWord(Word word)
        {
            words.Remove(word);         
        }

        protected virtual void OnWordAdded(EventArgs e)
        {
            WordAdded?.Invoke(this, e);
        }

        public List<string> GetWordsByCategory(string category)
        {
            return words.Where(w => w.Category == category)
                        .Select(w => w.Russian)
                        .ToList();
        }

        public List<string> GetCategories()
        {
            return words.Select(w => w.Category).Distinct().ToList();
        }

        public List<Word> GetAllWords()
        {
            return words;
        }

    }
}
