using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryForProject69
{
    /// <summary>
    /// Класс хранящий в себе реализацию всех пунктов меню.
    /// </summary>
    public static  class ExecuteAMenuItem
    {
        /// <summary>
        /// Метод выполняющий чтение данных из консоли в формате JSON.
        /// </summary>
        /// <param name="fragments"></param>
        public static void  EnterDataViaTheConsole(ref ElementsObject fragments)
        {
            Console.Write("Введите данные: ");
            StringBuilder sb = new();
            string? text;
            while ((text = Console.ReadLine()) != null) { sb.Append(text); }
            
            try
            { JsonParser.ParseJsonObject(sb.ToString(), ref fragments);}

            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            Console.WriteLine("Нажмите для проджолжения");
            Console.ReadKey();

        }
        /// <summary>
        ///  Метод выполняющий чтение данных из JSON-файла.
        /// </summary>
        /// <param name="fragments"></param>
        /// <param name="inputFilePath"></param>
        public static void EnterDataFromTheFile(ref ElementsObject fragments,ref string inputFilePath)
        {
            Console.Write("Напишите имя файла: ");
            inputFilePath = Console.ReadLine();
            if (inputFilePath is not null)
            {
                fragments = new();
                
                try
                { JsonParser.ReadJson(inputFilePath, ref fragments); }

                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
            }
            Console.WriteLine("Нажмите для проджолжения");
            Console.ReadKey();
        }
        /// <summary>
        /// Метод выполняющий сортировку данных по входящему ключу.
        /// </summary>
        /// <param name="fragments"></param>
        public static void SortDataByKey(ref ElementsObject fragments)
        {
            if (!OutputMenuInConsole.IsListFragmentsEmpty(fragments))
            {
                Console.WriteLine("Возможные поля для сортировки: ");
                //Пробегаем по всем fragment и формируем лист с полями
                List<string> fields = new List<string>();
                foreach (var field in fragments.Fragments.SelectMany(fragment => fragment.GetNonNestedFields())) //Сделало само visual studio.Я хотел вывести в отдельный метод свои foreach'и там есть быстрое форматитрование ит предложил так бахнуть - я согласился:).
                {
                    if (fields.Contains(field)) //Если такое поле уже есть , то не добавляем
                    { continue; }
                    else
                    {
                        fields.Add(field);
                    }
                }

                for (int i = 0; i < fields.Count; i++)
                {
                    
                    Console.WriteLine($"{i + 1}. {fields[i]}");
                }
                Console.Write("Введите номер поля: ");
                string? filedNumber = Console.ReadLine();
                
                if (filedNumber is not null && int.TryParse(filedNumber, out int numberOfTheSortingField) && numberOfTheSortingField >= 0 && numberOfTheSortingField <= fields.Count )
                {
                    Console.Write($"{Environment.NewLine}Введите ДА если хотите отсортировать в обычном порядке и НЕТ если в обратном порядке: "); // Если пользовател ничего не введет я считаю что должно быть дефолтным значение 
                    string? sortingStatus = Console.ReadLine() ?? "ДА";
                    bool isSortingInTheUsualOrder = (sortingStatus.ToLower() == "да" ? true : false);
                    fragments = ElementsObject.FragmentSorter(fields[numberOfTheSortingField - 1], fragments,isSortingInTheUsualOrder);
                    Console.WriteLine($"Сортировка по полю \"{fields[numberOfTheSortingField - 1]}\": ");
                    Console.WriteLine(JsonDataSerializer<Fragment>.SerializeListOfJsonObjectsToString(fragments.Fragments));
                }
            }
            Console.WriteLine("Нажмите для проджолжения");
            Console.ReadKey();

        }
        /// <summary>
        /// Метод позволяет фильтровать объекты  типа <see cref="Fragment"/>. 
        /// </summary>
        /// <param name="fragments"></param>
        public static void FilterDataByKey(ref ElementsObject fragments)
        {
            if (!OutputMenuInConsole.IsListFragmentsEmpty(fragments))
            {
                Console.WriteLine("Доступна фильтрация по следующим полям: ");
                List<string> fields = fragments.Fragments.SelectMany(f => f.GetNonNestedFields()).Distinct().ToList();
                for (int i = 0; i < fields.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {fields[i]}");
                }
                Console.Write("Введите номер поля: ");
                string? filter = Console.ReadLine();
                if (int.TryParse(filter, out int filterNumber) && filterNumber >= 1 && filterNumber <= fields.Count)
                {
                    List<string> values = new();
                    Console.WriteLine($"Завершение: нажмите Ctrl + Z (Windows) {Environment.NewLine} Ctrl + D (Linux/macOS) {Environment.NewLine} Отправьте пустое значение.");
                    while (true)
                    {
                        Console.Write("Введите значение: ");
                        string? value = Console.ReadLine();
                        if (!string.IsNullOrEmpty(value)) values.Add(value);
                        else if (values.Count > 0) break;
                        else Console.WriteLine("Необходимо ввести хотя бы одно значение.");
                    }
                    ElementsObject.FragmentFilter(fields[filterNumber - 1], values,ref fragments);
                    if ((fragments.Fragments.Count()>0))
                    {
                        Console.WriteLine($"Фильтрация по полю \"{fields[filterNumber - 1]}\": ");
                        string serializedFragments = JsonDataSerializer<Fragment>.SerializeListOfJsonObjectsToString(fragments.Fragments);
                        Console.WriteLine(serializedFragments);
                    }
                }
                else
                {
                    Console.WriteLine("Введен некорректный номер поля.");
                }
                Console.WriteLine("Нажмите для проджолжения");
                Console.ReadKey();
            }
        }
        
        /// <summary>
        /// Метод Выводит ифнформацию о фрагменте знаний по id, а так же предлагает сделать отчет об этом объекте в PDF-файл.
        /// </summary>
        /// <param name="fragments"></param>
        public static void OutputInformationAboutAPieceOfKnowledge(ref ElementsObject fragments)
        {
            if (!OutputMenuInConsole.IsListFragmentsEmpty(fragments))
            {
                Console.WriteLine("Все фрагменты: ");
                foreach (var fragment in fragments.Fragments)
                {
                    Console.WriteLine(fragment.Id.Replace("\"", ""));
                }
                Console.Write("Введите название id(например fragmentedge): ");
                
                string? id = Console.ReadLine();
                FULLInformationAboutTheObject fullInformationAboutTheObject = new FULLInformationAboutTheObject();
                if (id is not null)
                {
                    string dataAboutPieceOfKnowledge = fullInformationAboutTheObject.FullInformationAboutObject(fragments, id);
                    Console.WriteLine(dataAboutPieceOfKnowledge);
                    Console.Write("Хотите сделать запимь в PDF?: ");
                    string answer = Console.ReadLine() ?? "Нет";

                    if (answer.Trim().ToLower() == "да")
                    {
                        Console.Write("Введите название файла: ");
                        string nameFile = Console.ReadLine() ?? "Report69";
                        MakePDFReport makePDFReport = new MakePDFReport();
                        makePDFReport.DownloadPdfFileWithReport(dataAboutPieceOfKnowledge, nameFile);
                    }
                    else
                    {
                        Console.WriteLine("Мое дело предложить");
                    }
                }
                else
                {
                    Console.WriteLine("Введена некоректный ID");
                }
            }
            Console.WriteLine("Нажмите для проджолжения");
            Console.ReadKey();

        }
        /// <summary>
        /// Метод реализующий вывод данных из файла или введеных в консоль (отсортированные тоже) в консоль.
        /// </summary>
        /// <param name="fragments"></param>
        public static void OutputDataToTheConsole(ref ElementsObject fragments)
        {
            if (!OutputMenuInConsole.IsListFragmentsEmpty(fragments))
            {
                Console.WriteLine(JsonDataSerializer<Fragment>.SerializeListOfJsonObjectsToString(fragments.Fragments));
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Метод реализующий вывод данных из файла или введеных в консоль (отсортированные тоже) в файл.
        /// </summary>
        /// <param name="fragments"></param>
        /// <param name="outputFilePath"></param>
        public static void WriteDataToAFile(ref ElementsObject fragments,ref string outputFilePath)
        {
            if (!OutputMenuInConsole.IsListFragmentsEmpty(fragments))
            {
                Console.Write("Напишите путь до файла: ");
                outputFilePath = Console.ReadLine();
                if (outputFilePath is not null)
                {
                    try
                    {
                        JsonParser.WriteJson(JsonDataSerializer<Fragment>.SerializeListOfJsonObjectsToString(fragments.Fragments), outputFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.WriteLine("Нажмите для проджолжения");
            Console.ReadKey();
        }
    }
}
