using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    /// <summary>
    /// Класс реализованный для отображения консольного меню.
    /// </summary>
    public class OutputMenuInConsole
    {
        public const int COUNT_OF_ITEMS_IN_MENU = 8; //Количество пунктов меню.
        /// <summary>
        /// Стандартный конструктов.
        /// </summary>
        public OutputMenuInConsole() { }

        private string? nameFile = String.Empty; //Путь или имя файла.
        /// <summary>
        /// Конструктов в котором устанавливается путь к файлу или его имяю.
        /// </summary>
        /// <param name="name"></param>
        public OutputMenuInConsole(string? name)
        {
            NameFile = name;
        }
        /// <summary>
        /// Свойство для работы с nameFile(Путь к файлу или его имя).
        /// </summary>
        ///

        public string? NameFile
        {
            get
            {
                try
                {
                    return nameFile[(nameFile.LastIndexOf(Path.DirectorySeparatorChar) + 1)..];//Если файл передан путем, то  возращается только имя файла, без пути к нему.
                }
                catch
                {
                    return nameFile;
                }
            }
            set
            {
                nameFile = value;
            }
        }
        /// <summary>
        /// Позвоялет выводить наличия файла в системе. 
        /// </summary>
        private void FileStatus(ElementsObject fragments,ref string filePath)
        {
            Console.Write("Имя файл:");
            if (IsListFragmentsEmpty(fragments,String.Empty,false) || filePath.Length==0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Нету");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Метод выводящий пункты меню.
        /// Выбор пунктов происходит не вводом в коносль а выбором строки через Enter.
        /// </summary>
        /// <param name="index"></param>
        public void PrintMenu(int index, ElementsObject fragments,ref string filePath)
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);//Курсор устанавливаем в левом верхнем углу.
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("===============MENU===============");
            Console.ResetColor();
            for (int i = 0; i < COUNT_OF_ITEMS_IN_MENU; i++)//Перебираем все пункты меню 
            {
                if (i == index)//В строке на которой курсор меняем увет текста на черный а задний фон на белый, что выделяет строку
                {
                    Console.Write("->");
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    
                }
                PrintMenuElement(i + 1);//Метод который выводит пукнт в меню в зависимости от индекса с форматированием для каждого отдельно
                Console.ResetColor();


            }
            FileStatus(fragments,ref filePath);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("===============END===============");
            Console.ResetColor();
        }
        /// <summary>
        /// Метод Проверяющий строку на пустоту и отсутсвием ссылки.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsNotNullOrEmptyString(string? name)
        {
            if (name == null || name.Length == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Выводит в консоль сообщение об ошибке с переменной с форматированием
        /// </summary>
        private void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ОШИБКА!!!!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Введите коректное имя файла");
        }
        /// <summary>
        /// Метод проверяющий название файла на коректность(наличие файла,коректность структуры файла, пустота, наличие ссылки но строку).
        /// </summary>
        /// <returns></returns>
        public string? CorrectName()
        {
            string? inputNameFile = Console.ReadLine();
            while (!IsNotNullOrEmptyString(inputNameFile))
            {
                PrintError();
                inputNameFile = Console.ReadLine();
            }
            return inputNameFile;


        }
        /// <summary>
        /// Метод, который выводит в зависимости с параметром в который в него передали пункт меню со своим определенным стилем.
        /// </summary>
        /// <param name="menuItemNumber"></param>
        private void PrintMenuElement(int menuItemNumber)//Можно было бы проще сделать и даже универсальней, но я захотел добавить выделения в пунктах, поэтому решил их строго зафиксировать.
        {
            switch (menuItemNumber)
            {
                case 1:
                    Console.Write("Ввести данные через");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Консоль"); ;
                    break;
                case 2:
                    Console.Write("Ввести данные из");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Файл");
                    break;
                case 3:

                    Console.Write("Отфильтровать данные по");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Значению");
                    break;
                case 4:

                    Console.Write("Отсортировать данные по");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Значению");
                    break;
                case 5:

                    Console.Write("Информация об ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Фрагменте Знаний");
                    break;
                case 6:

                    Console.Write("Вывести данные через");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Консоль");
                    break;
                case 7:

                    Console.Write("Вывести данные в");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" Файл");
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Выход");
                    break;

            }
            Console.ResetColor();


        }
        /// <summary>
        /// Метод проверяет ввели ли данные пользователь.
        /// </summary>
        /// <param name="fragments"></param>
        /// <param name="emptyMessage"></param>
        /// <param name="sendMessage"></param>
        /// <returns></returns>
        public static bool IsListFragmentsEmpty(ElementsObject fragments, string emptyMessage = "Сначала необходимо ввести данные.", bool sendMessage = true)
        {
            if (fragments.Fragments.Count > 0)
            {
                return false;
            }
            else
            {
                if (sendMessage)
                {
                    Console.WriteLine(emptyMessage);
                }
                return true;
            }
        }

       

    }
}
