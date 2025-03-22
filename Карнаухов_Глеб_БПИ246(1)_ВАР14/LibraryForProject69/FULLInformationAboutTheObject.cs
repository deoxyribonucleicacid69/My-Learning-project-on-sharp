using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryForProject69
{
    /// <summary>
    /// Класс хранящий в себе метод выводящий информацию о фрагменте по id
    /// </summary>
    public class FULLInformationAboutTheObject
    {
        /// <summary>
        /// Метод находящий фрагмент по переданному id и ввозращает его Аспкекты, сочетания и их описание, и требуемые аспекты.
        /// </summary>
        /// <param name="fragments"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string FullInformationAboutObject(ElementsObject fragments,string ID)
        {
            
            string outputObject = String.Empty;
            List<string> requiredList = new List<string>();
            if (!OutputMenuInConsole.IsListFragmentsEmpty(fragments) && ID != null && ID.Length!=0)
            {
                foreach (Fragment fragment in fragments.Fragments)
                {
                    if (fragment.Id == ID)
                    {
                        outputObject += $"Фрагмент знания: {fragment.Label} ({fragment.Id}){Environment.NewLine}".Replace("\"", "").Replace("{", "").Replace("}", "").Replace(",", "");
                        outputObject += $"{Environment.NewLine}Аспекты:{JsonDataSerializer<AspectsObject>.SerializeJsonObjectToString(fragment.Aspects, 2)}".Replace("\"", "").Replace("{", "").Replace("}", "").Replace(",", "").Replace("\t","  ");
                        outputObject += $"{Environment.NewLine}Сочетания:{Environment.NewLine}";
                        foreach (SlotsObject slot in fragment.Slots)
                        {
                            outputObject += $"  {slot.Label.Replace(":","")}: {slot.Description}{Environment.NewLine}".Replace("[","").Replace("]","").Replace("{", "").Replace("}", ""); //Круглые скобки не меняю так как считаю что они являются частью описание которое показывает доп инфомрацию
                            outputObject += "  Требуемые аспкекты";
                            outputObject += JsonDataSerializer<RequiredObject>.SerializeJsonObjectToString(slot.Required, 4).Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\"", "") + Environment.NewLine;
                        }
                        
                        foreach (SlotsObject slotsObject in fragment.Slots)
                        {
                            foreach (string fields in slotsObject.Required.GetAllFields())
                            {
                                requiredList.Add(fields + Environment.NewLine);
                            }
                        }


                    }

                }
                //Я если четсно не понял что имеется в виду под описанием, но если описание то есть description который находится не в slots а в в основном уровне объекта то вот он 
                //foreach (string fields in requiredList)
                //{
                //    foreach (Fragment fragment in fragments.Fragments)
                //    {
                //        if (fragment.Id == fields)
                //        {
                //            
                //            outputObject += $"Описание {fields}: {fragment.Description}";
                //        }
                //    }
                //}
                return outputObject;
            }
            else
            {
                Console.WriteLine("Такого объекта нету");//Вывод на информации в консоль
                return "Не найдено!!";//Я решил что если объект не найден,то и отчет можно сделать, а в него надо что то записать, это заглушка которая пойдет в файл
            }
        }
           

    }
}
