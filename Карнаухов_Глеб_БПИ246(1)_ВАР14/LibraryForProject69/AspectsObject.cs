using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LibraryForProject69
{
    public class AspectsObject : IJSONObject
    {
        /// <summary>
        /// Словарь который хранит в себе имя и значения полей во вложенной структуре Aspects.
        /// </summary>
        public Dictionary<string, int> Aspects { get; private set; }

        /// <summary>
        /// Конструктор без аргументов, создает новый пусто словарь.
        /// </summary>
        public AspectsObject() { Aspects = new Dictionary<string, int>();}
        /// <summary>
        /// Конструктор с аргументом - Словарь, значение аргумента присваевается внутреннему словарю Aspects.
        /// </summary>
        /// <param name="aspects"></param>
        public AspectsObject(Dictionary<string, int> aspects)
        {
            Aspects = aspects;
        }
        /// <summary>
        /// Метод возращает  колекцию всех ключей в структуре.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFields()
        {
            return Aspects.Keys;
        }
        /// <summary>
        /// Метод позволяет вернуть значение поля по имени,если такого поля нету вернется нулл. 
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        public string GetField(string nameOfField)
        {
            if (Aspects.TryGetValue(nameOfField.ToLower(), out int valueOfField))
            {
                return valueOfField.ToString();
            }
            return "null";
        }
        /// <summary>
        /// Метод позволяющий устновить значение для поля.
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <param name="valueOfField"></param>
        /// <exception cref="FormatException"></exception>
        public void SetField(string nameOfField, string valueOfField)
        {
            if (int.TryParse(valueOfField, out int fieldValue))
            {
                Aspects[nameOfField.ToLower()] = fieldValue;
            }
            else
            {
                throw new FormatException("Значение поля должно быть типа integer(или типом неявно приводимому к int ).");
            }
        }
    }
}
