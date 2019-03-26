using System;
using System.Collections.Generic;
using System.Text;

namespace RussianPostClassLibrary
{
    public class OperationsInfo
    {
        string indexOperation = "Не указано";//Индекс места промежуточной операции
        string addressOperation = "Не указано";//Адрес/наименование места промежуточной операции
        string countryOperation = "Не указано";//Страна промежуточной операции
        string codeCountryOperation = "Не указано";//Код 3А страны промежуточной операции
        string nameOperation = "Не указано";//Наименование промежуточной операции
        int nameOperationCode;//Код операции
        string nameOperationAttribute = "Не указано";//Атрибут операции (операция: обработка, атрибут: прибыло в сортировочный центр)
        int nameOperationAttributeCode;//Код атрибута операции
        DateTime dataOperation;//Дата промежуточной операции
        string complexItemName = "Не указано";//Словесное описание посылки 

        public string IndexOperation
        {
            get { return indexOperation; }
            set { indexOperation = value; }
        }
        public string AddressOperation
        {
            get { return addressOperation; }
            set { addressOperation = value; }
        }
        public string CountryOperation
        {
            get { return countryOperation; }
            set { countryOperation = value; }
        }
        public string CodeCountryOperation
        {
            get { return codeCountryOperation; }
            set { codeCountryOperation = value; }
        }
        public string NameOperation
        {
            get { return nameOperation; }
            set { nameOperation = value; }
        }
        public int NameOperationCode
        {
            get { return nameOperationCode; }
            set { nameOperationCode = value; }
        }
        public string NameOperationAttribute
        {
            get { return nameOperationAttribute; }
            set { nameOperationAttribute = value; }
        }
        public int NameOperationAttributeCode
        {
            get { return nameOperationAttributeCode; }
            set { nameOperationAttributeCode = value; }
        }
        public DateTime DataOperation
        {
            get { return dataOperation; }
            set { dataOperation = value; }
        }
        public string ComplexItemName
        {
            get { return complexItemName; }
            set { complexItemName = value; }
        }
    }
}
