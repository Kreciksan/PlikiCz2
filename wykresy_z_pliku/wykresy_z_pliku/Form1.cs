using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wykresy_z_pliku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double max = 0;
        int xmax = 0;

        double yx1 = 0;
        double yx2 = 0;

        private void loadBtn_Click(object sender, EventArgs e)
        {
            chart1.Series["punkty"].Points.Clear();
            chart1.Series["wysokosc"].Points.Clear();

            max = 0;
            yx1 = 0;
            yx2 = 0;

            StreamReader sr;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                sr = File.OpenText(openFileDialog1.FileName);

                Char[] separators = { ',', ' ', '\n'};
                String[] nums = sr.ReadToEnd().Split(separators);
                Double[] values = new Double[nums.Length];

                int i = 0;
                foreach (var num in nums)
                {
                    double.TryParse(num.Replace('.', ',').Trim(), out double parsed);
                    values[i] = parsed;
                    i++;
                }

                for (i = 0; i < values.Length; i++)
                {
                    if(i % 2 == 1)
                    {
                        chart1.Series["punkty"].Points.AddXY(values[i - 1], values[i]);

                        if (max < values[i])
                        { 
                            max = values[i];
                            xmax = i-1;
                            
                        }

                    }
                }

                for (i = xmax; i > 0; i--)
                {
                    if (i % 2 == 1)
                    {
                        if (values[i] > values[i - 2])
                        {
                            yx1 = values[i - 2];
                            break;
                        }
                    }
                }

                for (i = xmax; i < values.Length/2; i++)
                {
                    if (i % 2 == 1)
                    {
                        if (values[i] < values[i - 2])
                        {
                            yx2 = values[i - 2];
                            break;
                        }
                    }
                }


                for (i = 0; i < values.Length; i++)
                {
                    if (i % 2 != 0)
                    {

                        if(yx1 < yx2)
                        {
                            chart1.Series["wysokosc"].Points.AddXY(values[i - 1], Math.Round((max + yx1)/2).ToString());
                        }
                        else
                        {
                            chart1.Series["wysokosc"].Points.AddXY(values[i - 1], Math.Round((max + yx2) / 2).ToString());

                        }
                    }
                }

                sr.Close();

            }
        }
    }
}
