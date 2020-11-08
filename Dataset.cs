using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZavrsniRadRojnic
{
    class Dataset
    {
        private List<List<double>> data;

        public Dataset()
        {
            this.data = new List<List<double>>();
        }

        public Dataset(String filePath) : this()
        {
            this.LoadDataFromCSV(filePath);
        }

        public void LoadDataFromCSV(String filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                List<double> row = new List<double>(Array.ConvertAll(line.Split(','), double.Parse));
                this.data.Add(row);
            }
        }

        public IList<List<double>> GetData()
        {
            return new System.Collections.ObjectModel.ReadOnlyCollection<List<double>>(this.data);
        }
    }
}
