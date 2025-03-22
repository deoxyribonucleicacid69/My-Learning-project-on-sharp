using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    /// <summary>
    /// Класс для вывода пунктов меню на экран и взаимодействия с ним
    /// </summary>
    public static class LaunchingTheMenu
    {
        /// <summary>
        /// Метод выводит на экран пукнты меню и реализует выбор их путем стрелочек и enter.
        /// </summary>
        /// <param name="fragments">Объект с которым мы работаем</param>
        /// <param name="index">Номер пункта меню</param>
        /// <param name="menu">Объект класса в котором находится метод для вывода пунктов меню</param>
        /// <param name="inputFilePath">Файл для чтения</param>
        /// <param name="outputFilePath">Файл для вывода</param>
        public static void LaunchingMenu(ref ElementsObject fragments,ref int index,ref OutputMenuInConsole menu,ref string? inputFilePath,ref string? outputFilePath)
        {
            while (true)
            {
                menu.PrintMenu(index, fragments, ref inputFilePath);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < 7)
                            index++;
                        else
                        {
                            index = 0;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                            index--;
                        else
                        {
                            index = 7;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (index + 1)
                        {
                            case 1:
                                //"Ввести данные через консоль"
                                ExecuteAMenuItem.EnterDataViaTheConsole(ref fragments);
                                break;
                            case 2:
                                //"Ввести данные from file"
                                ExecuteAMenuItem.EnterDataFromTheFile(ref fragments, ref inputFilePath);
                                break;
                            case 3:
                                //"Отфильтровать данные"
                                ExecuteAMenuItem.FilterDataByKey(ref fragments);
                                break;
                            case 4:
                                //"Отсортировать данные"
                                ExecuteAMenuItem.SortDataByKey(ref fragments);
                                break;
                            case 5:
                                //Фрагмент знаний
                                ExecuteAMenuItem.OutputInformationAboutAPieceOfKnowledge(ref fragments);
                                break;
                            case 6:
                                //Вывести данные в кносль
                                ExecuteAMenuItem.OutputDataToTheConsole(ref fragments);
                                break;
                            case 7:
                                //"Записать данные в файл"
                                ExecuteAMenuItem.WriteDataToAFile(ref fragments, ref outputFilePath);
                                Console.ReadKey();
                                break;
                            case 8:
                                return;

                        }
                        break;
                }
            }

        }
    }
}
