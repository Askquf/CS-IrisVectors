using System;
using System.Collections.Generic;
using System.Text;
using LinearAlgebra;
using System.IO;

namespace IrisOpener
{
    /// <summary>
    /// Класс обработчика файла
    /// </summary>
    public class FileWorker : IFileWorker
    {
        string _path;
        const int _maxFileSizeByte = 10240; //размер файла в байтах 
        private const int _numberIrises = 3;
        private const int _numberCharts = 4;

        public FileWorker(string fileName)
        {
            _path = fileName;
            if (fileName == "")
            {
                throw new FileNotFoundException();
            }
            FileInfo file = new FileInfo(_path);
            if (file.Length >= _maxFileSizeByte)
            {
                throw new FileLoadException();
            }
        }

       /// <summary>
       /// Считывает матрицу векторов
       /// Одна строка матрицы - один тип векторов (iris species)
       /// Изначально создает массив строк, который потом передает в другую функцию
       /// </summary>
       /// <returns>Возвращает матрицу векторов-ирис</returns>
       /// <exception cref="FileNotFoundException">
       /// Исключение для случая, когда файл не найден
       /// </exception>"
        public List<List<MathVector>> ReadFullFile()
        {
            if (Existion())
            {
                List<List<MathVector>> irisVectors = new List<List<MathVector>>();
                for (int i = 0; i < _numberIrises; i++)
                    irisVectors.Add(new List<MathVector>());
                string[] fileStrings = File.ReadAllLines(_path);
                AllVectorsCreate(fileStrings, irisVectors);
                return irisVectors;
            }
            else
                throw new FileNotFoundException();
        }

        /// <summary>
        /// Проверка на существование файла
        /// </summary>
        /// <returns>Существует файл или нет</returns>
        public bool Existion()
        {
            bool result;
            if (File.Exists(_path) && _path != "")
                result = true;
            else
                result = false;
            return result;
        }

        /// <summary>
        /// Считывание данных всех векторов
        /// </summary>
        /// <param name="fileStrings">Массив строк, которые необходимо преобразовать в векторы</param>
        /// <param name="irisVectors">Матрица для записи получившихся векторов</param>
        public void AllVectorsCreate(string[] fileStrings, List<List<MathVector>> irisVectors)
        {
            for (int i = 1; i < fileStrings.Length; i++)
            {
                MathVector tmp = new MathVector(4);
                string[] subs = fileStrings[i].Split(',');
                SplitNumberCheck(subs);
                VectorCreate(tmp, subs);
                IrisTypeChoose(subs[4], irisVectors, tmp);
            }
        }

        /// <summary>
        /// Проверяет количество параметров в строке
        /// </summary>
        /// <param name="strings">Строка для проверки</param>
        /// <exception cref="Exception("Wrong number of iris parametrs!")>Ошибка, возвращаемая в случае, если количество параметров больше или меньше нужного</exception>"
        public void SplitNumberCheck(string[] strings)
        {
            if (strings.Length != _numberCharts + 1)
                throw new Exception("Wrong number of iris parametrs!");
        }

        /// <summary>
        /// Записывает данные в один вектор.
        /// </summary>
        /// <param name="vectorToWrite">Вектор для записи</param>
        /// <param name="subs"></param>
        public void VectorCreate(MathVector vectorToWrite, string[] subs)
        {
            for (int j = 0; j < _numberCharts; j++)
            {
                if (DoubleCheck(subs[j]))
                    vectorToWrite[j] = Convert.ToDouble(subs[j], System.Globalization.CultureInfo.InvariantCulture);
                else
                    throw new Exception("Parametr not a number");
            }
        }

        /// <summary>
        /// Проверка, ялвяется ли строка числом
        /// </summary>
        /// <param name="toCheck">Строка для проверки</param>
        public bool DoubleCheck(string toCheck)
        {
            bool result = true;
            bool dot = false;
            for (int i = 0; i < toCheck.Length; i++)
            {
                if (Char.IsNumber(toCheck[i]) || (dot == false && toCheck[i] == '.'))
                {
                    if (toCheck[i] == '.')
                        dot = true;
                }
                else
                    result = false;
            }
            return result;
        }

        /// <summary>
        /// Выбирает, какой тип ирис-вектора был получен
        /// Записывает его в соответвующую строку матрицы
        /// </summary>
        /// <param name="irisName">Тип данного ирис-вектора</param>
        /// <param name="irisVectors">Матрица ирис-векторов, в которую будет записан новый</param>
        /// <param name="currentVector">Данный ирис-вектор</param>
        /// <exception cref="Exception("Unknown iris type")">
        /// Ошибка, выбрасываемая в случае, если тип ирис-вектора неправильный
        /// </exception>"
        public void IrisTypeChoose(string irisName, List<List<MathVector>> irisVectors, MathVector currentVector)
        {
            if (irisName == "setosa")
                irisVectors[0].Add(currentVector);
            else if (irisName == "versicolor")
                irisVectors[1].Add(currentVector);
            else if (irisName == "virginica")
                irisVectors[2].Add(currentVector);
            else
                throw new Exception("Unknow iris type!");
        }
    }
}
