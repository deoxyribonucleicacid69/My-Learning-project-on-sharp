using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    public class XtriggersObject : IJSONObject
    {
        /// <summary>
        /// Свойство в виде словаря для которое зранит в себе имя поля и его значение из вложенной стукрутры JSON-файла Xtringgers. 
        /// </summary>
        public Dictionary<string, string> Xtriggers { get; private set; }
        /// <summary>
        /// Дефолтный конструктор который создает пустой словарь и устанавливает его в свойство.
        /// </summary>
        public XtriggersObject() { Xtriggers = new Dictionary<string, string>(); }
        /// <summary>
        /// Конструктор который устанавливает переданный словарь в ствойство.  
        /// </summary>
        /// <param name="aspects"></param>
        public XtriggersObject(Dictionary<string, string> xtriggers)
        {
            Xtriggers = xtriggers;
        }
        /// <summary>
        /// Метод возращающий строковую колекцию всех полей вложенной стукрутры JSON-файла Xtringgers. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFields()
        {
            return Xtriggers.Keys;
        }
        /// <summary>
        /// Метод по переданному имени ищет такое поле во вложенной стукрутры JSON-файла Xtringgers  и возращает его при удачном нахождении.
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        public string GetField(string nameOfField)
        {
            if (Xtriggers.TryGetValue(nameOfField.ToLower(), out string? valueOfField) && valueOfField is not null)
            {
                return $"\"{valueOfField}\"";
            }
            return "null";
        }
        /// <summary>
        /// Метод устанавливающий переданное значение в поле по переданному имени.
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <param name="value"></param>
        /// <exception cref="FormatException"></exception>
        public void SetField(string nameOfField, string value)
        {
            Xtriggers[nameOfField.ToLower()] = value;

        }
    }
}
