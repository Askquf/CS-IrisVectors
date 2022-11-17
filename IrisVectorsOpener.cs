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
    public partial class IrisVectorsOpener : Form
    {
        private Controller _controller;
        private const int _numberCharts = 4;
        private const int _numberIrises = 3;

        public IrisVectorsOpener()
        {
            InitializeComponent();
        }

       /// <summary>
       /// Метод, вызываемый при нажатии на кнопку "Open File"
       /// </summary>
        public void button1_Click(object sender, EventArgs e)
        {
            string filename = FileAsking();
            try
            {
                _controller = new Controller(filename);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found");
                return;
            }
            catch (FileLoadException)
            {
                MessageBox.Show("Too big file");
                return;
            }
            Drawing();
            FilePathLabel.Text = filename;
        }


        /// <summary>
        /// Метод, вызывающий диалоговое окно для открытия файла
        /// </summary>
        /// <returns>Имя выбранного пользователем файла</returns>
        public string FileAsking()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Comma separeted files (*.csv)|*.csv"; //фильтр, чтобы нельзя было открыть ничего кроме csv
            dialog.ShowDialog();
            return dialog.FileName;
        }

        /// <summary>
        /// Рисует все необходимым графики
        /// Сначала обращается к контроллеру для получения усредненных векторов.
        /// Добавляет их в чарты.
        /// Затем отрисовывает круговую диаграмму.
        /// </summary>
        public void Drawing ()
        {
            try
            {
                AllChartsFill(_controller.WorkerHandlerAsking());
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong data!");
                return;
            }
            EvcledeanBuild(_controller.EvcledeanAsk());
        }

        /// <summary>
        /// Заполняет столбчатые диаграммы массивом усредненных векторов
        /// </summary>
        /// <param name="data">Массив усредненных векторов</param>
        public void AllChartsFill(List<MathVector> data)
        {
            string[] speciesname = { "setosa", "versicolor", "virginica" };
            string[] titles = { "sepal_length", "sepal_width", "petal_length", "petal_width" };
            Chart[] charts = new Chart[_numberCharts];
            Color[] colours = new Color[_numberIrises];
            colours[0] = Color.Gold; colours[1] = Color.Pink; colours[2] = Color.Silver;
            charts[0] = chart_slength; charts[1] = chart_swidth;
            charts[2] = chart_plength; charts[3] = chart_pwidth;
            for (int j = 0; j < _numberCharts; j++)
            {
                ClearChart(charts[j]);
                AxesCreate(charts[j], titles[j]);
                for (int i = 0; i < _numberIrises; i++)
                {
                    OneChartColumnFill(charts[j], speciesname[i], data[i][j], colours[i]);
                }
            }
        }

        /// <summary>
        /// Отрисовывает один столбик диаграммы
        /// </summary>
        /// <param name="chartToFill">Элемент, в котором рисуется диаграмма</param>
        /// <param name="serie">Строка-наименование столбца</param>
        /// <param name="dataPiece">Число, которое соответствует данной строке</param>
        public void OneChartColumnFill(Chart chartToFill, string serie, double dataPiece, Color colour)
        {
            Series series = chartToFill.Series.Add(serie);
            series.Color = colour;
            series.BorderColor = Color.Black;
            series.Points.Add(dataPiece);
            series.IsValueShownAsLabel = true;
            series.SmartLabelStyle.Enabled = false;
        }

        /// <summary>
        /// Создает оси для диаграммы
        /// </summary>
        /// <param name="charttofill">Диаграмма для создания осей</param>
        public void AxesCreate(Chart chartToFill, string title)
        {
            Axis ax = new Axis();
            ax.Title = title;
            chartToFill.ChartAreas[0].AxisX = ax;
            Axis ay = new Axis();
            ay.Title = "Definitions";
            chartToFill.ChartAreas[0].AxisY = ay;
        }

        /// <summary>
        /// Построение круговой диаграммы по Евклидовым расстояниям
        /// </summary>
        /// <param name="data">Массив евклидовых расстояний</param>
        public void EvcledeanBuild(MathVector data)
        {
            ClearChart(chartpie);
            chartpie.Series.Add("Evcledean distance");
            chartpie.Titles.Add("Evcledean distance");
            string[] names = { "setose-versicolor", "versicolor-virginica", "virginica-setosa" };
            Color[] colours = new Color[_numberIrises];
            colours[0] = Color.DarkCyan; colours[1] = Color.LightGoldenrodYellow; colours[2] = Color.IndianRed;
            chartpie.Series[0].ChartType = SeriesChartType.Pie;
            for (int i = 0; i < data.Dimensions; i++)
            {
                chartpie.Series[0].Points.Add(data[i]);
                chartpie.Series[0].Points[i].Color = colours[i];
                chartpie.Series[0].BorderColor = Color.Black;
                chartpie.Series[0].Points[i].LegendText = names[i];
                chartpie.Series[0].Points[i].Label = (Math.Round(data[i], 2)).ToString();
            }
        }

        /// <summary>
        /// Отчистка элемента для дальнейшей работы с ним.
        /// </summary>
        /// <param name="chart_to_clear">Элемент, который нужно отчистить</param>
        public void ClearChart(Chart chart_to_clear)
        {
            chart_to_clear.Series.Clear();
            chart_to_clear.Titles.Clear();
        }
    }
}
