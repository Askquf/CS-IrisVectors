using System;
using System.Collections.Generic;
using System.Text;
using LinearAlgebra;

namespace IrisOpener
{
    /// <summary>
    /// Класс обработчика ирисов
    /// </summary>
    public class IrisModel : IIrisHandler
    {
        private List<List<MathVector>> _irisVectors;
        private List<MathVector> _results;
        private const int _numberIrises = 3;
        private const int _numberCharts = 4;

        /// <summary>
        /// создание объекта ирис-обработчик, запись в него матрицы векторов
        /// </summary>
        /// <param name="vectorsList">Матрица векторов для записи</param>
        public IrisModel(List<List<MathVector>> vectorsList)
        {
            _irisVectors = new List<List<MathVector>>();
            int counter = 0;
            foreach (List<MathVector> i in vectorsList)
            {
                _irisVectors.Add(new List<MathVector>());
                foreach (MathVector j in i)
                {
                    _irisVectors[counter].Add(new MathVector(j));
                }
                counter++;
            }
        }

        /// <summary>
        /// Создание 3 усредненных векторов
        /// </summary>
        /// <returns>Массив усредненных векторов</returns>
        public List<MathVector> AverageForAllCount()
        {
            _results = new List<MathVector>();
            for (int i = 0; i < _numberIrises; i++)
            {
                MathVector tmp = new MathVector(_numberCharts);
                for (int j = 0; j < _numberCharts; j++)
                {
                    tmp[j] = OneAverageCount(j, i);
                }
                _results.Add(tmp);
            }
            return _results;
        }

        /// <summary>
        /// Подсчет одного среднего значения
        /// </summary>
        /// <param name="parametr">Параметр, который необходимо считать</param>
        /// <param name="vector">Тип ирис-вектора, для которых среднее подсчитывается</param>
        /// <returns></returns>
        public double OneAverageCount(int parametr, int vector)
        {
            double sum = 0;
            for (int i = 0; i < _irisVectors[vector].Count; i++)
            {
                sum += _irisVectors[vector][i][parametr];
            }
            double result;
            if (_irisVectors[vector].Count == 0)
                result = 0;
            else
                result = sum / _irisVectors[vector].Count;
            return result;
        }

        /// <summary>
        /// Подсчет евклидовых расстояний для 3 усредненных векторов
        /// </summary>
        /// <returns>Массив из 3 чисел - евклидовых расстояний</returns>
        public MathVector EvcledeanCount ()
        {
            MathVector resultVector = new MathVector(_results.Count);
            for (int i = 0; i < _results.Count; i++)
            {
                int сounter = i+1;
                if (i == _results.Count - 1)
                    сounter = 0;
                resultVector[i] = _results[i].CalcDistance(_results[сounter]);
            }
            return resultVector;
        }
    }
}