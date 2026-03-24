using System;

namespace DamerauLevenshteinDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Поиск с опечатками (расстояние Дамерау-Левенштейна) ===");
            Console.WriteLine("Для выхода введите 'exit' в качестве первой строки.\n");

            while (true)
            {
                Console.Write("Введите первую строку: ");
                string first = Console.ReadLine();

                // Проверка на выход из цикла
                if (first?.ToLower() == "exit")
                    break;

                Console.Write("Введите вторую строку: ");
                string second = Console.ReadLine();

                // Вычисляем и выводим расстояние
                int distance = DamerauLevenshteinDistance(first, second);
                Console.WriteLine($"Расстояние Дамерау-Левенштейна между '{first}' и '{second}' = {distance}");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Вычисление расстояния Дамерау-Левенштейна между двумя строками.
        /// </summary>
        /// <param name="s1">Первая строка</param>
        /// <param name="s2">Вторая строка</param>
        /// <returns>Расстояние Дамерау-Левенштейна. Если одна из строк null, возвращает -1.</returns>
        public static int DamerauLevenshteinDistance(string s1, string s2)
        {
            // Проверка на null
            if (s1 == null || s2 == null)
                return -1;

            int len1 = s1.Length;
            int len2 = s2.Length;

            // Пустые строки
            if (len1 == 0 && len2 == 0) return 0;
            if (len1 == 0) return len2;
            if (len2 == 0) return len1;

            // Приводим к верхнему регистру для регистронезависимого сравнения
            string str1 = s1.ToUpperInvariant();
            string str2 = s2.ToUpperInvariant();

            // Создаём матрицу (len1+1) x (len2+1)
            int[,] matrix = new int[len1 + 1, len2 + 1];

            // Инициализация нулевой строки и нулевого столбца
            for (int i = 0; i <= len1; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= len2; j++)
                matrix[0, j] = j;

            // Заполнение матрицы
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    // Совпадают ли текущие символы?
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

                    // Базовые операции: удаление, вставка, замена
                    int del = matrix[i - 1, j] + 1;
                    int ins = matrix[i, j - 1] + 1;
                    int sub = matrix[i - 1, j - 1] + cost;

                    // Минимум из трёх
                    matrix[i, j] = Math.Min(Math.Min(del, ins), sub);

                    // Дополнение Дамерау: проверка транспозиции соседних символов
                    if (i > 1 && j > 1 &&
                        str1[i - 1] == str2[j - 2] &&
                        str1[i - 2] == str2[j - 1])
                    {
                        // Транспозиция стоит как одна операция
                        int trans = matrix[i - 2, j - 2] + cost;
                        matrix[i, j] = Math.Min(matrix[i, j], trans);
                    }
                }
            }

            // Результат в правом нижнем углу
            return matrix[len1, len2];
        }
    }
}