using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinearAlgebra;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace IrisOpener
{
    class Controller
    {
        private IrisModel _handler;
        private FileWorker _worker;

        public Controller(string fileName)
        {
            WorkerCreating(fileName);
        }

        /// <summary>
        /// Создает объект Обработчик файла, передавая ему имя файла, выбранного пользователем
        /// </summary>
        /// <param name="filename">Имя файла, выбранное пользователем</param>
        public void WorkerCreating(string fileName)
        {
            _worker = new FileWorker(fileName);
        }
       
        /// <summary>
        /// Создает обработчик векторов-ирис и запускает метод рисования
        /// </summary>
        public List<MathVector> WorkerHandlerAsking()
        {
            _handler = new IrisModel(_worker.ReadFullFile());
            return _handler.AverageForAllCount();
        }
        
        public MathVector EvcledeanAsk()
        {
            return _handler.EvcledeanCount();
        }
    }
}
