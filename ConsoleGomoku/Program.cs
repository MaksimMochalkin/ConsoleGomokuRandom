using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleGomoku
{
    class Program
    {
        static string X = "X";
        static string O = "O";
        static int count = 2; // Подсчет ходов. Первые 2 уже стоят в центре
        static bool XorO = true; // Кто ходит true X false O
        static int final = 0; // Переменная оконания игры. Если 0, то не окончена

        static void Main(string[] args)
        {

            string[,] board = new string[15, 15];

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    board[i, j] = "-";
                }
            }

            board[7, 7] = X;
            board[7, 8] = O;

            drawBoard(board);
            do
            {
                count++;

                board = Move(board, XorO);

                //Console.ReadLine();
                Thread.Sleep(1000);
                Console.Clear();
                CheckWin(board);
                drawBoard(board);
                XorO = !XorO;
            } while (final == 0);


            switch (final)
            {
                case 1:
                    Console.WriteLine("Победил X");
                    break;
                case 2:
                    Console.WriteLine("Победил О");
                    break;
                case 3:
                    Console.WriteLine("Ничья!");
                    break;
            }

            Console.ReadLine();
        }

        public static void drawBoard(string[,] board)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void CheckWin(string[,] board)
        {
            for (int i = 0; i < 15; i++) // Проверка всех пятерок по горизонтали
            {
                for (int j = 0; j < 11; j++)
                {
                    if (board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2] && board[i, j] == board[i, j + 3]
                        && board[i, j] == board[i, j + 4] && board[i, j] != "-")
                    {
                        if (board[i, j] == "X")
                            final = 1;
                        else
                            final = 2;
                        board[i, j] = board[i, j + 1] = board[i, j + 2] = board[i, j + 3] = board[i, j + 4] = "П";
                        break;
                    }
                }
            }

            for (int j = 0; j < 15; j++) // Проверка всех пятерок по вертикали
            {
                for (int i = 0; i < 11; i++)
                {
                    if (board[i, j] == board[i + 1, j] && board[i, j] == board[i + 2, j] && board[i, j] == board[i + 3, j]
                        && board[i, j] == board[i + 4, j] && board[i, j] != "-")
                    {
                        if (board[i, j] == "X")
                            final = 1;
                        else
                            final = 2;
                        board[i, j] = board[i + 1, j] = board[i + 2, j] = board[i + 3, j] = board[i + 4, j] = "П";
                        break;
                    }
                }
            }

            for (int i = 0; i < 11; i++) // Проверка всех пятерок по диагонали
            {
                for (int j = 0; j < 11; j++)
                {
                    if (board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2] &&
                    board[i, j] == board[i + 3, j + 3] && board[i, j] == board[i + 4, j + 4] && board[i, j] != "-")
                    {
                        if (board[i, j] == "X")
                            final = 1;
                        else
                            final = 2;
                        board[i, j] = board[i + 1, j + 1] = board[i + 2, j + 2] = board[i + 3, j + 3] = board[i + 4, j + 4] = "П";
                        break;
                    }

                    if (board[i, j + 4] == board[i + 1, j + 3] && board[i, j + 4] == board[i + 2, j + 2] &&
                    board[i, j + 4] == board[i + 3, j + 1] && board[i, j + 4] == board[i + 4, j] && board[i, j + 4] != "-")
                    {
                        if (board[i, j] == "X")
                            final = 1;
                        else
                            final = 2;
                        board[i, j + 4] = board[i + 1, j + 3] = board[i + 2, j + 2] = board[i + 3, j + 1] = board[i + 4, j] = "П";
                        break;
                    }
                }
            }

            if (count == 225) // Ничья
            {
                final = 3;
            }
        }


        public static string[,] Move(string[,] board, bool XorO)
        {
            int priority = 0; // Приоритет рассматриваемой пятерки
            int maxPriority = 0; // Максимальный приоритет данного хода
            int iAI = 0, jAI = 0; // Координаты, на которые нужно сделать ход
            int iAIvacant = 0; // Первая координата пустого, вакантного места(например Х_Х_Х, первая "_" и есть координаты)
            int jAIvacant = 0; // Вторая координата пустого, вакантного места(Х_Х_Х, вторая "_")
            int stone = 0; // Камень, количество камней в рассматриваемой пятерке
            int window = 0; // Окно, количество пустых мест в рассматриваемой пятерке
            string friend; // Друг, союзный камень
            string enemy; // Враг, камень соперника
            string currentAI; // Текущий игрок: X или O
            int koef = 1; // Коэфициент усиления приоритета

            Random rnd = new Random();

            if (XorO == false)
            {
                enemy = "X";
                friend = "O";
            }
            else
            {
                enemy = "O";
                friend = "X";
            }




            for (int i = 0; i < 15; i++) // ГОРИЗОНТАЛИ
            {
                for (int j = 0; j < 11; j++) // Смотрит 5 элементов СПРАВА от найденного, проверяет на пустоту 1 левый
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        stone = window = priority = jAIvacant = 0;
                        koef = 1;
                        for (int jFive = j; jFive < j + 5; jFive++) // Рассматриваем пятерку
                        {
                            if (board[i, jFive] == currentAI)
                                stone++;
                            if (board[i, jFive] == "-")
                            {
                                window++;
                                if (window == 1)
                                    jAIvacant = jFive;
                            }
                            if (window + stone == 5) // Если в пятерке камни только 1го игрока и "окна", то имеет смысл оценить приоритет хода в эту пятерку
                            {
                                if (j > 0 && board[i, j - 1] == "-") // Если клетка позади пятерки пустая, значимость пятерки увеличивается относительно пятерок с таким же количеством камней
                                    koef++;
                                if (stone == 1)
                                    priority = 1 * koef;
                                if (stone == 2)
                                    priority = 3 * koef;
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (j > 0 && board[i, j - 1] == "-" && currentAI == friend) // Если клетка позади пуста, дружеских камней в пятерке 3, возможно выйти на победу
                                        priority++;
                                }
                                if (stone == 4) // 4 камня требуют наибольшего внимания
                                {
                                    priority = 100;
                                    if (currentAI == friend) // Если четверка дружественная, наибольший приоритет: победа
                                        priority += 100;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = i;
                                    jAI = jAIvacant;
                                }

                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = i;
                                        jAI = jAIvacant;
                                    }
                                }
                            }
                        }
                    }
                }
                for (int j = 4; j < 15; j++)  // Смотрит 5 элементов СЛЕВА от найденного, проверяет на пустоту 1 правый
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        stone = window = priority = jAIvacant = 0;
                        koef = 1;
                        for (int jFive = j; jFive > j - 5; jFive--)
                        {
                            if (board[i, jFive] == currentAI)
                                stone++;
                            if (board[i, jFive] == "-")
                            {
                                window++;
                                if (window == 1)
                                    jAIvacant = jFive;
                            }
                            if (window + stone == 5)
                            {
                                if (j < 14 && board[i, j + 1] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1 * koef;
                                if (stone == 2)
                                { priority = 3 * koef; }
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (j < 14 && board[i, j + 1] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = i;
                                    jAI = jAIvacant;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = i;
                                        jAI = jAIvacant;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            for (int j = 0; j < 15; j++) // ВЕРТИКАЛИ
            {
                for (int i = 0; i < 11; i++)   // Смотрит 5 элементов СНИЗУ от найденного, проверяет на пустоту 1 верхний
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        stone = window = priority = jAIvacant = 0;
                        koef = 1;
                        for (int iFive = i; iFive < i + 5; iFive++)
                        {
                            if (board[iFive, j] == currentAI)
                                stone++;
                            if (board[iFive, j] == "-")
                            {
                                window++;
                                if (window == 1)
                                    iAIvacant = iFive;
                            }
                            if (window + stone == 5)
                            {
                                if (i > 0 && board[i - 1, j] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1;
                                if (stone == 2)
                                { priority = 3 * koef; }
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (i > 0 && board[i - 1, j] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = iAIvacant;
                                    jAI = j;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = iAIvacant;
                                        jAI = j;
                                    }
                                }
                            }
                        }
                    }
                }
                for (int i = 4; i < 15; i++)  // Смотрит 5 элементов СВЕРХУ от найденного, проверяет на пустоту 1 нижний
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        stone = window = priority = jAIvacant = 0;
                        koef = 1;
                        for (int iFive = i; iFive > i - 5; iFive--)
                        {
                            if (board[iFive, j] == currentAI)
                                stone++;
                            if (board[iFive, j] == "-")
                            {
                                window++;
                                if (window == 1)
                                    iAIvacant = iFive;
                            }
                            if (window + stone == 5)
                            {
                                if (i < 15 - 1 && board[i + 1, j] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1;
                                if (stone == 2)
                                { priority = 3 * koef; }
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (i < 14 && board[i + 1, j] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = iAIvacant;
                                    jAI = j;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = iAIvacant;
                                        jAI = j;
                                    }
                                }
                            }
                        }
                    }
                }
            }



            for (int i = 0; i < 11; i++) // ДИАГОНАЛЬ \
            {
                int iFive;
                for (int j = 0; j < 11; j++) // Смотрит 5 элементов ПО ДИАГОНАЛИ СНИЗУ от найденного, проверяет на пустоту 1 верхний
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        iFive = i;
                        stone = window = priority = iAIvacant = jAIvacant = 0;
                        koef = 1;
                        for (int jFive = j; jFive < j + 5; jFive++)
                        {
                            if (board[iFive, jFive] == currentAI)
                                stone++;
                            if (board[iFive, jFive] == "-")
                            {
                                window++;
                                if (window == 1)
                                {
                                    iAIvacant = iFive;
                                    jAIvacant = jFive;
                                }
                            }
                            if (window + stone == 5)
                            {
                                if (i > 0 && j > 0 && board[i - 1, j - 1] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1;
                                if (stone == 2)
                                    priority = 3 * koef;
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (i > 0 && j > 0 && board[i - 1, j - 1] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = iAIvacant;
                                    jAI = jAIvacant;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = iAIvacant;
                                        jAI = jAIvacant;
                                    }
                                }
                            }
                            iFive++;
                        }
                    }
                }
            }
            for (int i = 4; i < 15; i++) // диагональ \ от низа к верху
            {
                for (int j = 4; j < 15; j++) // Смотрит 5 элементов ПО ДИАГОНАЛИ СВЕРХУ от найденного, проверяет на пустоту 1 нижний
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        int iFive = i;
                        stone = window = priority = iAIvacant = jAIvacant = 0;
                        koef = 1;
                        for (int jFive = j; jFive > j - 5; jFive--)
                        {
                            if (board[iFive, jFive] == currentAI)
                                stone++;
                            if (board[iFive, jFive] == "-")
                            {
                                window++;
                                if (window == 1)
                                {
                                    iAIvacant = iFive;
                                    jAIvacant = jFive;
                                }
                            }
                            if (window + stone == 5)
                            {
                                if (i < 14 && j < 14 && board[i + 1, j + 1] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1;
                                if (stone == 2)
                                { priority = 3 * koef; }
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (i < 14 && j < 14 && board[i + 1, j + 1] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = iAIvacant;
                                    jAI = jAIvacant;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = iAIvacant;
                                        jAI = jAIvacant;
                                    }
                                }
                            }
                            iFive--;
                        }
                    }
                }
            }


            for (int i = 4; i < 15; i++) // ДИАГОНАЛЬ / 
            {
                for (int j = 0; j < 11; j++) // Смотрит 5 элементов ПО ДИАГОНАЛИ СВЕРХУ от найденного, проверяет на пустоту 1 нижний
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        int iFive = i;
                        stone = window = priority = iAIvacant = jAIvacant = 0;
                        koef = 1;
                        for (int jFive = j; jFive < j + 5; jFive++)
                        {
                            if (board[iFive, jFive] == currentAI)
                                stone++;
                            if (board[iFive, jFive] == "-")
                            {
                                window++;
                                if (window == 1)
                                {
                                    iAIvacant = iFive;
                                    jAIvacant = jFive;
                                }
                            }
                            if (window + stone == 5)
                            {
                                if (i < 14 && j > 0 && board[i + 1, j - 1] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1;
                                if (stone == 2)
                                { priority = 3 * koef; }
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (i < 14 && j > 0 && board[i + 1, j - 1] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = iAIvacant;
                                    jAI = jAIvacant;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = iAIvacant;
                                        jAI = jAIvacant;
                                    }
                                }
                            }
                            iFive--;
                        }
                    }
                }
            }
            for (int i = 0; i < 11; i++) // Смотрит 5 элементов ПО ДИАГОНАЛИ СНИЗУ от найденного, проверяет на пустоту 1 верхний
            {
                for (int j = 4; j < 15; j++)
                {
                    if (board[i, j] != "-") // Если клетка не пустая, нужно отсмотреть от нее пятерку
                    {
                        currentAI = board[i, j];
                        int iFive = i;
                        stone = window = priority = iAIvacant = jAIvacant = 0;
                        koef = 1;
                        for (int jFive = j; jFive > j - 5; jFive--)
                        {
                            if (board[iFive, jFive] == currentAI)
                                stone++;
                            if (board[iFive, jFive] == "-")
                            {
                                window++;
                                if (window == 1)
                                {
                                    iAIvacant = iFive;
                                    jAIvacant = jFive;
                                }
                            }
                            if (window + stone == 5)
                            {
                                if (j < 14 && i > 0 && board[i - 1, j + 1] == "-")
                                    koef++;
                                if (stone == 1)
                                    priority = 1;
                                if (stone == 2)
                                { priority = 3 * koef; }
                                if (stone == 3)
                                {
                                    priority = 10 * koef;
                                    if (j < 14 && i > 0 && board[i - 1, j + 1] == "-" && currentAI == friend)
                                        priority++;
                                }
                                if (stone == 4)
                                {
                                    priority = 100;
                                    if (currentAI == friend)
                                        priority++;
                                }

                                if (priority > maxPriority)
                                {
                                    maxPriority = priority;
                                    iAI = iAIvacant;
                                    jAI = jAIvacant;
                                }
                                if (priority == maxPriority)
                                {
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        maxPriority = priority;
                                        iAI = iAIvacant;
                                        jAI = jAIvacant;
                                    }
                                }
                            }
                            iFive++;
                        }
                    }
                }

            } // алгоритм AI закрывается

            if (maxPriority == 0) // если алгоритм не нашел ни одного хода, то выбирается случайная клетка, чтобы дойти до ничьей
            {
                do            // Случайно находится пустая клетка
                {
                    iAI = rnd.Next(0, 15);
                    jAI = rnd.Next(0, 15);
                } while (board[iAI, jAI] != "-");
            }
            board[iAI, jAI] = friend;
            return board;
        }

    }
}