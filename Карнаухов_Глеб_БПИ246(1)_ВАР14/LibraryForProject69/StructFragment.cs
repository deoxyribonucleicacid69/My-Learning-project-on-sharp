using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    public struct Fragment : IJSONObject
    {
        /// <summary>
        /// ID элемента.
        /// </summary>
        public string? Id { get; private set; }
        /// <summary>
        /// Локализация или что то такое, я не понял.
        /// </summary>
        public string? BurnTo { get; private set; }

        /// <summary>
        /// Название элемента, используемое для отображения.
        /// </summary>
        public string? Label { get; private set; }

        /// <summary>
        /// Объект, содержащий аспекты элемента.
        /// </summary>
        public AspectsObject Aspects { get; private set; }

        /// <summary>
        /// Список слотов, связанных с элементом.
        /// </summary>
        public List<SlotsObject> Slots { get; private set; }

        /// <summary>
        /// Описание элемента.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Необходимые аспкекты.
        /// </summary>
        public XtriggersObject Xtriggers { get; private set; }

        /// <summary>
        /// Уникальный идентификатор, указывающий на уникальность элемента в системе.
        /// </summary>
        public string? Unique { get; private set; }

        /// <summary>
        /// Флаг,указывающий, требуется ли дополнительная обработка или артефакты для элемента.
        /// </summary>
        public string? Noartneeded { get; private set; }

        /// <summary>
        /// Комментарии  связанные с элементом.
        /// </summary>
        public string? Comments { get; private set; }
        /// <summary>
        /// Дэфолтный Конструктор инициализирующий в 3 свойство значения(Aspects,Slots,Xtriggers). 
        /// </summary>
        public Fragment() 
        {
            Aspects = new AspectsObject();
            Slots = new List<SlotsObject>();
            Xtriggers = new XtriggersObject();
            //Unique = "false";
            //Noartneeded = "false";//Сделано для удобства сортирвоки, что бы поля не имеющие такого поля считались как false и сортировка работала коректно, если же убрать, то работа программы останется коректной только сортировка булевых значений будет работать как вшито в C#
        }
        /// <summary>
        /// Метод возращающий строковую колекцию - все не вложенныые поля.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetNonNestedFields()
        {
            List<string> fields = new();
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.GetValue(this) is not null and not IJSONObject && propertyInfo.Name.ToLower()!="slots")
                {
                    fields.Add(propertyInfo.Name.ToLower());
                }
            }
            return fields;
        }
        /// <summary>
        /// Метод возращающий строковую колекцию - все пол.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFields()
        {
            List<string> listOfField = new();
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.GetValue(this) is not null)
                {
                    listOfField.Add(propertyInfo.Name.ToLower());
                }
            }
            return listOfField;
        }
        /// <summary>
        /// Метод возращающий значение переданного поля.
        /// </summary>
        /// <param name="ameOfField"></param>
        /// <returns></returns>
        public string GetField(string ameOfField)
        {
            switch (ameOfField.ToLower())
            {
                case "id":
                    return $"\"{Id}\"";
                case "burnto":
                    return $"\"{BurnTo}\"";
                case "label":
                    return $"\"{Label}\"";
                case "aspects":
                    return $"\"{JsonDataSerializer<AspectsObject>.SerializeJsonObjectToString(Aspects, 4)}\"";
                case "description":
                    return $"\"{Description}\"";
                case "xtriggers":
                    return $"\"{JsonDataSerializer<XtriggersObject>.SerializeJsonObjectToString(Xtriggers, 4)}\"";
                case "unique":
                    return $"{Unique}";
                case "noartneeded":
                    return $"{Noartneeded}";
                case "comments":
                    return $"\"{Comments}\"";
                case "slots":
                    return $"{JsonDataSerializer<SlotsObject>.SerializeListOfSlotsObjectsToString(Slots)}";
                default:
                    return "null";
            }
        }
        /// <summary>
        /// Метод устанавливающий переданное значение в переданное поле.
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <param name="valueOfField"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetField(string nameOfField, string valueOfField)
        {
            switch (nameOfField.ToLower())
            {
                case "id":
                    Id = valueOfField;
                    break;
                case "burnto":
                    BurnTo = valueOfField;
                    break;
                case "label":
                    Label = valueOfField;
                    break;
                case "aspects":
                    AspectsObject aspects = new();
                    JsonParser.ParseJsonObject(valueOfField, ref aspects);
                    Aspects = aspects;
                    break;
                case "description":
                    Description = valueOfField;
                    break;
                case "xtriggers":
                    XtriggersObject xtriggers = new();
                    JsonParser.ParseJsonObject(valueOfField, ref xtriggers);
                    Xtriggers = xtriggers;
                    break;
                case "unique":
                    Unique = Convert.ToString(valueOfField);
                    break;
                case "noartneeded":
                    Noartneeded = valueOfField;
                    break;
                case "comments":
                    Comments = Convert.ToString(valueOfField);
                    break;
                case "slots":
                    List<SlotsObject> listOfSlotsObjects = new();
                    JsonParser.ParseListOfJsonObjects(valueOfField, listOfSlotsObjects);
                    Slots = listOfSlotsObjects;
                    break;
                default:
                    Console.WriteLine(nameOfField);
                    throw new KeyNotFoundException("Поля с таким ключом нет.");
            }
        }
    }
}
    
