using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    public  class ElementsObject : IJSONObject
    {
        /// <summary>
        /// Свойство представляющие собой Лист из объектов стуктуры Fragment.
        /// </summary>
        public List<Fragment> Fragments { get; private set; }
        /// <summary>
        /// Стандартный конструктор - создает пустой лист стуктуры Fragment.
        /// </summary>
        public ElementsObject()  { Fragments = new List<Fragment>(); }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ElementsObject"/> с указанным списком фрагментов.
        /// </summary>
        /// <param name="fragment">Список фрагментов, который будет присвоен свойству <see cref="Fragments"/>.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="fragment"/> равен <c>null</c>.</exception>
        public ElementsObject(List<Fragment> fragment)
        {
            Fragments = fragment;
        }
        /// <summary>
        /// Возращает единтсвенное поле - elements
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllFields()
        {
            return ["elements"];
        }
        /// <summary>
        /// Возращает значение поля elements. 
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        public string GetField(string nameOfField)
        {
            if (nameOfField.ToLower() == "elements")
            {
                return $"\"{JsonDataSerializer<Fragment>.SerializeListOfJsonObjectsToString(Fragments)}\""; ;
            }
            return "null";
        }
        /// <summary>
        /// Позволяет установить значение  полю elements.
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <param name="valueOfField"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetField(string nameOfField, string valueOfField)
        {
            if (nameOfField.ToLower() != "elements")
            {
                throw new KeyNotFoundException("Такого поля нету");
            }
            JsonParser.ParseListOfJsonObjects(valueOfField, Fragments);
        }
        /// <summary>
        /// Метод фильтрует данные пользователя по переданному полю.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="objectValue"></param>
        /// <param name="Fragments"></param>
        /// <returns></returns>
        public static void FragmentFilter(string fieldName, List<string> objectValue, ref ElementsObject Fragments)
        {
            Fragments.Fragments = Fragments.Fragments.Where(f => objectValue.Contains(f.GetField(fieldName).Trim('"'))).ToList();
        }
        /// <summary>
        /// Сортировка данных по убюыванию или по возрастанию.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="Fragments"></param>
        /// <param name="DefualtSortCondition"></param>
        /// <returns></returns>
        public static ElementsObject FragmentSorter(string fieldName, ElementsObject Fragments, bool DefualtSortCondition = true)
        {
            if (DefualtSortCondition)// По возрастанию.
            {
                if (int.TryParse(Fragments.Fragments[0].GetField(fieldName), out int _))
                {
                    return new ElementsObject(Fragments.Fragments.OrderBy(f => int.Parse(f.GetField(fieldName))).ToList());
                }
                return new ElementsObject(Fragments.Fragments.OrderBy(f => f.GetField(fieldName).Trim('"')).ToList());
            }
            else // По убыванию.
            {
                if (int.TryParse(Fragments.Fragments[0].GetField(fieldName), out int _))
                {
                    return new ElementsObject(Fragments.Fragments.OrderBy(f => int.Parse(f.GetField(fieldName))).Reverse().ToList());
                }
                return new ElementsObject(Fragments.Fragments.OrderBy(f => f.GetField(fieldName).Trim('"')).Reverse().ToList());
            }
        }
    }
}
