using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZavrsniRadRojnic
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            CultureInfo culture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path;
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                knn knn = new knn();
                string selected = comboBox1.Text;
                IOverSamplingAlgorithm algorithm = randomOversampling.GetInstance();

                switch (selected)
                {
                    case "bez preuzorkovanja":
                        algorithm = new NoOverSampling();
                        break;
                    case "nasumično preuzorkovanje":
                        algorithm = new randomOversampling();
                        break;
                    case "SMOTE":
                        algorithm = new SMOTE(knn);
                        break;
                    case "Bordeline-SMOTE":
                        algorithm = new Borderline_SMOTE(knn);
                        break;
                    case "Safe-Level-SMOTE":
                        algorithm = new Safe_Level_SMOTE(knn);
                        break;
                    default:
                        break;
                }

                Dataset dataset = new Dataset(path);
                datasetManipulator datasetManipulator;
                Test test = new Test();

                double sum = 0;
                int n = Convert.ToInt32(textBox4.Text);
                for (int i = 0; i < n; i++)
                {
                    datasetManipulator = new datasetManipulator(dataset, algorithm, knn, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox1.Text));
                    sum += Math.Round(test.calculate_FMeasure(datasetManipulator), 6);
                }
                textBox3.Text = Convert.ToString(Math.Round((sum / n), 6));
            }
        }
    }
}
