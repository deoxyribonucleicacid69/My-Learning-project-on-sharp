using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace LibraryForProject69
{
        /// <summary>
        /// Класс для формирования строк в формате JSON-файла.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static class JsonDataSerializer<T> where T : IJSONObject
        {
        /// <summary>
        /// Формирует строку,которая хранит в себе один объект формата JSON-файла
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="objectDepthLevel"></param>
        /// <returns></returns>
            public static string SerializeJsonObjectToString(T jsonObject, int objectDepthLevel = 0)
            {
                StringBuilder sb = new();
                sb.Append($"{(objectDepthLevel < 3 ? GetIndention(objectDepthLevel) : "")}{{{Environment.NewLine}");
                int fieldNumber = 0;
                List<string> fields = jsonObject.GetAllFields().ToList();
                foreach (string fieldName in fields)
                {
                    sb.Append($"{GetIndention(objectDepthLevel + 1)}\"{fieldName}\": ");
                    string fieldValue = jsonObject.GetField(fieldName);
                    sb.Append(fieldValue.StartsWith("\"{") ? fieldValue.Trim('"') : fieldValue);
                    if (++fieldNumber < fields.Count)
                    {
                        sb.Append(",");
                    }
                    sb.Append(Environment.NewLine);
                }
                sb.Append($"{GetIndention(objectDepthLevel)}}}");
                return sb.ToString();
            }
            /// <summary>
            /// Метод формирует строку содержащая Лист из объектов в формате JSON-файла
            /// </summary>
            /// <param name="jsonObjects"></param>
            /// <returns></returns>
            public static string SerializeListOfJsonObjectsToString(List<T> jsonObjects)
            {
                StringBuilder sb = new();
                sb.Append($"{{{Environment.NewLine}{GetIndention(1)}\"elements\": [{Environment.NewLine}");
                for (int jsonObjectNumber = 0; jsonObjectNumber < jsonObjects.Count; jsonObjectNumber++)
                {
                    sb.Append(SerializeJsonObjectToString(jsonObjects[jsonObjectNumber], 2));
                    if (jsonObjectNumber != jsonObjects.Count - 1)
                    {
                        sb.Append(",");
                    }
                    sb.Append(Environment.NewLine); 
                }
                sb.Append($"{GetIndention(1)}]{Environment.NewLine}}}");
                return sb.ToString();
            }
        /// <summary>
        /// Метод формирующий строку содержащий лист объектов внутри объекта в формате JSON-файла.
        /// </summary>
        /// <param name="jsonObjects"></param>
        /// <returns></returns>
        public static string SerializeListOfSlotsObjectsToString(List<T> jsonObjects)
        {
            StringBuilder sb = new();
            sb.Append($"[{Environment.NewLine}");
            for (int jsonObjectNumber = 0; jsonObjectNumber < jsonObjects.Count; jsonObjectNumber++)
            {
                sb.Append(GetIndention(4));
                sb.Append(SerializeJsonObjectToString(jsonObjects[jsonObjectNumber], 4));
                if (jsonObjectNumber != jsonObjects.Count - 1)
                {
                    sb.Append(",");
                }
                sb.Append(Environment.NewLine);
            }
            sb.Append($"{GetIndention(3)}]");
            return sb.ToString();
        }

        private static string GetIndention(int objectLevel) => new string(' ', objectLevel * 2);
        }
}
