using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WordTrainer
{
    public class JsonManager
    {
        private const string FileName = "words.json";

        public static void SaveWords(List<Word> words)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Save";
            Nullable<bool> result = dlg.ShowDialog();
            File.WriteAllText(dlg.FileName + ".json", JsonConvert.SerializeObject(words));
            if (result == true)
            {
                MessageBox.Show("Слова сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static List<Word> LoadWords()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Save";
            Nullable<bool> result = dlg.ShowDialog();

            if (!File.Exists(dlg.FileName))
                return new List<Word>();

            var json = File.ReadAllText(dlg.FileName);
            if (result == true)
            {
                MessageBox.Show("Слова загружены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return JsonConvert.DeserializeObject<List<Word>>(json) ?? new List<Word>();
            
        }
    }
}
