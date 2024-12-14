using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordTrainer
{
    public class Word
    {
        
        public string Russian { get; set; }
        public string Translation { get; set; }
        public string Category { get; set; }

        public Word(string russian, string translation, string category)
        {
            Russian = russian;
            Translation = translation;
            Category = category;
        }


    }
}
