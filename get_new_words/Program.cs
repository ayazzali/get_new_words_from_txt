using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace get_new_words
{
    class Program
    {
        static void Main(string[] args)
        {
            //var res = " headline: ‘The Death of Equities: How inflation is destroying the stock market’. Readers".getLexiems();

            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Start");
            var path_to_file_txt = args[0];
            var t1 = await File.ReadAllTextAsync(path_to_file_txt);
            var t2 = t1.Replace(".", " ").Replace(",", " ").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")
                //.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .GetWordsOrPhrases()
                .GroupBy(_ => _.ToUpper()).Select(_ => _.First())
                //.Distinct()
                .Where(_ => _.Length > 2)
                .Where(word =>
                    !word.Any(lr =>
                        char.IsDigit(lr)));

            Console.WriteLine("res lines count = " + t2.Count());
            var dir = Path.GetDirectoryName(path_to_file_txt);
            var name_file_txt = Path.GetFileNameWithoutExtension(path_to_file_txt);
            var resFilePath = Path.Combine(dir, name_file_txt + "_words.txt");
            await File.WriteAllLinesAsync(resFilePath, t2);
            Console.WriteLine("End");
        }
    }

    public static class strqwe
    {
        /// <summary>
        /// для сохранения цитат
        /// </summary>
        public static IEnumerable<string> GetWordsOrPhrases(this string text)
        {
            var res = new List<string>();
            string curLex = null;
            foreach (var word in text.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            {
                if (word.Contains("‘") && word.Contains("’")) //цитата из одного слова
                {
                    res.Add(word);
                }
                else if (word.Contains("‘")) //начало цитаты
                {
                    curLex = word;
                }
                else if (curLex != null)
                {
                    if (curLex.Length > 100)
                    {
                        Console.WriteLine("curLex.Length too loong (its OK.): " + curLex);
                        res.AddRange(curLex.Split(" ")); // ошибка
                        curLex = null;
                    }
                    else if (word.Contains("’")) //конец цитаты
                    {
                        curLex += (" " + word);
                        res.Add(curLex);
                        curLex = null;
                    }
                    else
                    {
                        curLex += (" " + word);
                    }
                }
                else
                {
                    res.Add(word);
                }
            }
            return res;
        }
    }
}
