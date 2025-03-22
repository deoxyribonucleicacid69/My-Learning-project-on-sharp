using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    public struct SlotsObject : IJSONObject
    {
        /// <summary>Уникальный идентификатор элемента.</summary>
        public string? Id { get; private set; }

        /// <summary>Название элемента.</summary>
        public string? Label { get; private set; }

        /// <summary>Описание элемента.</summary>
        public string? Description { get; private set; }

        /// <summary>Обязательные параметры элемента.</summary>
        public RequiredObject Required { get; private set; }

        /// <summary>Идентификатор действия.</summary>
        public string? Actionid { get; private set; }
        /// <summary>
        /// Стандартный конструктор. 
        /// </summary>
        public SlotsObject()
        {
            Required = new RequiredObject();
        }

        /// <summary>
        /// Метод возращающий коллекцию строк - названия полей не вложенных структур. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetNonNestedFields()
        {
            List<string> listOfFields = new();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                if (property.GetValue(this) is not null and not IJSONObject)
                {
                    listOfFields.Add(property.Name.ToLower());
                }
            }
            return listOfFields;
        }
        /// <summary>
        /// Метод возращающий коллекцию строк - названия полей. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFields()
        {
            List<string> listOfFields = new();
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.GetValue(this) is not null)
                {
                    listOfFields.Add(propertyInfo.Name.ToLower());
                }
            }
            return listOfFields;
        }
        /// <summary>
        /// Метод возращающий значение переданного поля.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetField(string fieldName)
        {
            switch (fieldName.ToLower())
            {
                case "id":
                    return $"\"{Id}\"";
                case "label":
                    return $"\"{Label}\"";
                case "description":
                    return $"\"{Description}\"";
                case "required":
                    return $"\"{JsonDataSerializer<RequiredObject>.SerializeJsonObjectToString(Required,6)}\"";
                case "actionid":
                    return $"\"{Actionid}\"";
                default:
                    return "null";
            }
        }
        /// <summary>
        /// Позволяет установить переданное значение в поле с переданным именем.
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <param name="valuOfField"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetField(string nameOfField, string valuOfField)
        {
            switch (nameOfField.ToLower())
            {
                case "id":
                    Id = valuOfField;
                    break;
                case "label":
                    Label = valuOfField;
                    break;
                case "description":
                    Description = valuOfField;
                    break;
                case "required":
                    RequiredObject required = new();
                    JsonParser.ParseJsonObject(valuOfField, ref required);
                    Required = required;
                    break;
                case "actionid":
                    Actionid = valuOfField; 
                    break;
                default:
                    throw new KeyNotFoundException("Поля с таким ключом нет."); ;
            }
        }
    }
}
