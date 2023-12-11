using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherManager.Controllers
{
    public class Recommend
    {
        public double ComputeSimilarity(string s1, string s2)
        {
            // Tách các từ trong chuỗi s1 và s2
            string[] words1 = s1.Split(' ');
            string[] words2 = s2.Split(' ');

            // Tạo một tập hợp chứa các từ đc nhất trong hai chuỗi
            HashSet<string> uniqueWords = new HashSet<string>(words1.Concat(words2));

            // Tạo vector tần suất từ cho hai chuỗi
            Dictionary<string, int> freqVector1 = new Dictionary<string, int>();
            foreach (string word in words1)
            {
                if (freqVector1.ContainsKey(word))
                {
                    freqVector1[word]++;
                }
                else
                {
                    freqVector1.Add(word, 1);
                }
            }

            Dictionary<string, int> freqVector2 = new Dictionary<string, int>();
            foreach (string word in words2)
            {
                if (freqVector2.ContainsKey(word))
                {
                    freqVector2[word]++;
                }
                else
                {
                    freqVector2.Add(word, 1);
                }
            }

            // Tính toán độ tương đồng dựa trên Cosine Similarity
            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            foreach (string word in uniqueWords)
            {
                int freq1 = 0;
                int freq2 = 0;

                if (freqVector1.ContainsKey(word))
                {
                    freq1 = freqVector1[word];
                }

                if (freqVector2.ContainsKey(word))
                {
                    freq2 = freqVector2[word];
                }

                dotProduct += (double)freq1 * freq2;
                magnitude1 += Math.Pow(freq1, 2);
                magnitude2 += Math.Pow(freq2, 2);
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            double similarity = dotProduct / (magnitude1 * magnitude2);

            return similarity;
        }
    }
}