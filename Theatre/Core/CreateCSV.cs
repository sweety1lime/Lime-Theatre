using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Theatre.Core
{
    class CreateCSV
    {
        public static void WriteCSV(List<string> tableList, string tableName)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists($"{folder.SelectedPath}\\{tableName}.csv"))
                    File.Delete($"{folder.SelectedPath}\\{tableName}.csv");

                using (StreamWriter sw = File.CreateText($"{folder.SelectedPath}\\{tableName}.csv"))
                {
                    foreach (string item in tableList)
                        sw.WriteLine(item);
                }

                System.Windows.MessageBox.Show("Экспорт таблицы завершен!");
            }
        }
    }
}
