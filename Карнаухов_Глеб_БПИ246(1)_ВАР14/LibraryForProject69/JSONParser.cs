using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using LibraryForProject69;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace LibraryForProject69
{
    public static class JsonParser
    {
        /// <summary>
        /// Чтение входных строк 
        /// </summary>
        /// <returns></returns>
        public static string ReadingTheStringStream()
        {
            StringBuilder sb = new();
            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                sb.Append(line);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Метод для записи данных в файл в виде JSON.
        /// </summary>
        /// <param name="outputTextFromJSONFile"></param>
        /// <param name="filePath"></param>
        public static void WriteJson(string outputTextFromJSONFile, string filePath)
        {
            using (FileStream fs = new(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new(fs, Encoding.UTF8))
                {
                    Console.SetOut(sw);
                    Console.WriteLine(outputTextFromJSONFile);
                }
            }
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
        /// <summary>
        /// Метод для чтения JSON файла.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="jsonObject"></param>
        public static void ReadJson<T>(string filePath, ref T jsonObject) where T : IJSONObject
        {
            string jsonString;
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new(fs, Encoding.UTF8))
                {
                    Console.SetIn(sr);
                    jsonString = ReadingTheStringStream();
                }
            }
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            ParseJsonObject(jsonString, ref jsonObject);
        }



        /// <summary>
        /// Парсинг верхнего уровня файла, и разбиение на лист <see cref="Fragment"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textFromJSONFile"></param>
        /// <param name="listOfJSONObject"></param>
        /// <exception cref="Exception"></exception>
        public static void ParseListOfJsonObjects<T>(string textFromJSONFile, List<T> listOfJSONObject) where T : IJSONObject, new()
        {
            int cursorPosition = 0;
            Symbol symbol = GetNextSymbol(textFromJSONFile, ref cursorPosition);
            if (symbol.Type != SymbolType.LeftBracket)
            {
                throw new Exception("Ожидается '[' в начале списка объектов.");
            }
            while (true)
            {
                string getValue = GetValue(textFromJSONFile, ref cursorPosition);
                T jsonObject = new();
                ParseJsonObject(getValue, ref jsonObject);
                listOfJSONObject.Add(jsonObject);
                Symbol comma = GetNextSymbol(textFromJSONFile, ref cursorPosition);
                if (comma.Type == SymbolType.RightBracket)
                {
                    break;
                }
                if (comma.Type != SymbolType.Comma)
                {
                    throw new Exception("Ожидается ',' или ']' после значения объекта.");
                }
            }
        }
        /// <summary>
        /// Парсинг объекта <see cref="Fragment"/>на его состоавляющие.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textFromJSONFile"></param>
        /// <param name="jsonObject"></param>
        public static void ParseJsonObject<T>(string textFromJSONFile, ref T jsonObject) where T : IJSONObject
        {
            int cursorPosition = 0;
            FillJsonObjectsField(textFromJSONFile, ref cursorPosition, ref jsonObject);
        }
        /// <summary>
        /// Парсинг вложенной структуры Slots, которая представляет собой лист из других объектов <see cref="SlotsObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textFromJSONFile"></param>
        /// <param name="cursorPosition"></param>
        /// <param name="jsonObject"></param>
        /// <exception cref="Exception"></exception>
        private static void FillJsonObjectsField<T>(string textFromJSONFile, ref int cursorPosition, ref T jsonObject) where T : IJSONObject
        {
            if (GetNextSymbol(textFromJSONFile, ref cursorPosition).Type != SymbolType.LeftBrace)
            {

                throw new Exception("Ожидается '{' в начале объекта.");
            }
            while (true)
            {
                Symbol symbol = GetNextSymbol(textFromJSONFile, ref cursorPosition);

                if (symbol.Type == SymbolType.RightBrace) break;

                else if (symbol.Type != SymbolType.String) throw new Exception("Ожидается строка в качестве ключа объекта.");
                
                if (GetNextSymbol(textFromJSONFile, ref cursorPosition).Type != SymbolType.Colon) throw new Exception("Ожидается ':' после ключа объекта.");

                jsonObject.SetField(symbol.Value, GetValue(textFromJSONFile, ref cursorPosition));
                Symbol comma = GetNextSymbol(textFromJSONFile, ref cursorPosition);

                if (comma.Type == SymbolType.RightBrace) break;

                else if (comma.Type != SymbolType.Comma) throw new Exception("Ожидается ',' или '}' после значения объекта.");
                
            }
        }
        /// <summary>
        /// Извлекает значение из JSON-строки начиная с указанной позиции.
        /// </summary>
        /// <param name="textFromJSONFile">JSON-строка, из которой извлекается значение.</param>
        /// <param name="cursorPosition">Текущая позиция в JSON-строке, с которой начинается извлечение значения.</param>
        /// <exception cref="Exception">
        /// Выбрасывается, если тип значения не распознан.
        /// </exception>
        private static string GetValue(string textFromJSONFile, ref int cursorPosition)
        {
            Symbol symbol = GetNextSymbol(textFromJSONFile, ref cursorPosition);
            switch (symbol.Type)
            {
                case SymbolType.LeftBrace or SymbolType.LeftBracket:
                    int leftCursorPosition = --cursorPosition;
                    SkippingNestedStructure(textFromJSONFile, ref cursorPosition);
                    return textFromJSONFile.Substring(leftCursorPosition, cursorPosition - leftCursorPosition);
                case SymbolType.String or SymbolType.Number or SymbolType.Bool:
                    return symbol.Value;
                case SymbolType.Null:
                    return "";
                default:
                    throw new Exception($"Неизвестный тип значения: {symbol.Value}");
            }
        }
        /// <summary>
        /// Возращает следующий элемент из структуры  <see cref="Symbol"/>
        /// </summary>
        /// <param name="textFromJSONFile"></param>
        /// <param name="cursorPosition"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Symbol GetNextSymbol(string textFromJSONFile, ref int cursorPosition)
        {
            SkipingWhiteSpaces(textFromJSONFile, ref cursorPosition);
            if (cursorPosition >= textFromJSONFile.Length) throw new Exception("Ожидается } в конце файла");
            switch (textFromJSONFile[cursorPosition])
            {
                case '{':
                    return new Symbol(SymbolType.LeftBrace, textFromJSONFile[cursorPosition++].ToString());
                case '}':
                    return new Symbol(SymbolType.RightBrace, textFromJSONFile[cursorPosition++].ToString());
                case '[':
                    return new Symbol(SymbolType.LeftBracket, textFromJSONFile[cursorPosition++].ToString());
                case ']':
                    return new Symbol(SymbolType.RightBracket, textFromJSONFile[cursorPosition++].ToString());
                case ':':
                    return new Symbol(SymbolType.Colon, textFromJSONFile[cursorPosition++].ToString());
                case ',':
                    return new Symbol(SymbolType.Comma, textFromJSONFile[cursorPosition++].ToString());
                case '"':
                    return GettingAnStringValueFromAField(textFromJSONFile, ref cursorPosition);
                case '-' or '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9':
                    return GettingAnIntegerValueFromAField(textFromJSONFile, ref cursorPosition);
                case 'n' when textFromJSONFile.Substring(cursorPosition, 4) == "null":
                    cursorPosition += 4;
                    return new Symbol(SymbolType.Null, "null");
                case 't' when textFromJSONFile.Substring(cursorPosition, 4) == "true":
                    cursorPosition += 4;
                    return new Symbol(SymbolType.Bool, "true");
                case 'f' when textFromJSONFile.Substring(cursorPosition, 5) == "false":
                    cursorPosition += 4;
                    return new Symbol(SymbolType.Bool, "false");
                default:
                    return new Symbol(SymbolType.Unfamiliar, textFromJSONFile[cursorPosition++].ToString()); ;
            }
        }
        /// <summary>
        /// Метод пропускает пробелы
        /// </summary>
        /// <param name="textFromJSONFile"></param>
        /// <param name="curorPosition"></param>
        private static void SkipingWhiteSpaces(string textFromJSONFile, ref int curorPosition)
        {
            while (curorPosition < textFromJSONFile.Length && char.IsWhiteSpace(textFromJSONFile[curorPosition]))
            {
                curorPosition++;
            }
        }
        /// <summary>
        /// Метод пропускающий вложенные объекты(поддерживает одноуровневость парсинга).
        /// </summary>
        /// <param name="textFromJSONFile"></param>
        /// <param name="cursorPosition"></param>
        /// <exception cref="Exception"></exception>
        private static void SkippingNestedStructure(string textFromJSONFile, ref int cursorPosition)
        {
            Stack<Symbol> bracketStack = new();
            do
            {
                Symbol symbol = GetNextSymbol(textFromJSONFile, ref cursorPosition);
                switch (symbol.Type)
                {
                    case SymbolType.LeftBrace or SymbolType.LeftBracket:
                        bracketStack.Push(symbol);
                        break;
                    case SymbolType.RightBrace or SymbolType.RightBracket:
                        bracketStack.Pop();
                        break;
                }
            } while (cursorPosition < textFromJSONFile.Length && bracketStack.Count != 0);
            if (cursorPosition >= textFromJSONFile.Length) throw new Exception("Нету '}' в конце файла.");

        }
        /// <summary>
        /// Вырезание строки находящийся между двойных кавычек.
        /// </summary>
        /// <param name="lineForCuttingOutValue"></param>
        /// <param name="cursorPosition"></param>
        /// <returns></returns>
        private static Symbol GettingAnStringValueFromAField(string lineForCuttingOutValue, ref int cursorPosition)
        {
            cursorPosition++;
            StringBuilder sb = new();
            while (cursorPosition < lineForCuttingOutValue.Length && lineForCuttingOutValue[cursorPosition] != '"')
            {
                sb.Append(lineForCuttingOutValue[cursorPosition++]);
            }
            cursorPosition++;
            return new Symbol(SymbolType.String, sb.ToString());
        }
        /// <summary>
        /// Получение числового значение из поля.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="curPosition"></param>
        /// <returns></returns>
        private static Symbol GettingAnIntegerValueFromAField(string jsonString, ref int curPosition)
        {
            StringBuilder sb = new();
            while (curPosition < jsonString.Length &&
                (char.IsDigit(jsonString[curPosition]) || jsonString[curPosition] is '.' or '-'))
            {
                sb.Append(jsonString[curPosition++]);
            }
            return new Symbol(SymbolType.Number, sb.ToString());
        }
        ///Для типа bool не написан отдельный метод так как парсится как string.
    }
}

