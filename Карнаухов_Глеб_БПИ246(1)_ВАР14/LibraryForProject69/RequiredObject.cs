using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    /// <summary>
    /// Класс реализующий вложенную структуру в <see cref="ElementsObject"> "required".
    /// </summary>
    public class RequiredObject : IJSONObject
    {
        /// <summary>
        /// Свойство реализуемое в виде словаря типа ключ:<see cref="string"> значение:<see cref="int">   
        /// </summary>
        public Dictionary<string,int> Required {  get; private set; }
        /// <summary>
        /// Дэфолтный констурктор который устанавливает в свойство пусто словарь.
        /// </summary>        
        public RequiredObject() 
        {
            Required = new Dictionary<string,int>();
        }
        /// <summary>
        /// Констурктор который устанавливает в свойство переданный словарь.
        /// </summary>
        /// <param name="required"></param>
        public RequiredObject(Dictionary<string, int> required)
        {
            Required =  required;
        }
        /// <summary>
        /// Возращает все поля вложенной структуры  <see cref="RequiredObject"> то есть все ключи из словаря    
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFields()
        {
            return Required.Keys;
        }
        /// <summary>
        /// Возращает значение переданного поля.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetField(string fieldName)
        {
            if (Required.TryGetValue(fieldName.ToLower(), out int fieldValue))
            {
                return fieldValue.ToString();
            }
            return "null";
        }
        /// <summary>
        /// Устанавливает переданное значение в поля с переданным именем.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <exception cref="FormatException"></exception>
        public void SetField(string fieldName, string value)
        {
            if (int.TryParse(value, out int fieldValue))
            {
                Required[fieldName.ToLower()] = fieldValue;
            }
            else
            {
                throw new FormatException("Значение поля должно быть целым числом.");
            }
        }
    }
}
