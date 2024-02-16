using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sortowanie
{
    public partial class Form1 : Form
    {
        int n;
        int[] array;

        public Form1()
        {
            InitializeComponent();
            Graphic();
        }

        int generateCount;
        int quickSortCounter;

        #region Buttons
        private void button1_Click(object sender, EventArgs e) // Button for generating an array
        {
            if (generateCount > 0)
                AllClear();
            generateCount++;
            ArrayLength();
            Random rand = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                if (i == array.Length - 1)
                {
                    array[i] = rand.Next(1, 1000);
                    textBox1.Text += array[i];
                }
                else
                {
                    array[i] = rand.Next(1, 1000);
                    textBox1.Text += array[i] + "; ";
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e) // Button to start all sorting in a thread
        {
            int k = (int)numericUpDown2.Value;
            int op = 0;
            if ((generateCount == 0) || (array.Length == 0))
                _ = MessageBox.Show("No elements to sort!");
            else
            {
                for (int subtableSize = 3; subtableSize <= array.Length; subtableSize += k)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[op].Cells[0].Value = subtableSize;

                    await Task.Run(() => BubbleSort(ArrayCopy(subtableSize), op));
                    await Task.Run(() => SelectionSort(ArrayCopy(subtableSize), op));
                    await Task.Run(() => InsertionSort(ArrayCopy(subtableSize), op));
                    await Task.Run(() => ShellSort(ArrayCopy(subtableSize), op));

                    int[] subtable = ArrayCopy(subtableSize);
                    await Task.Run(() => QuickSort(subtable, 0, subtable.Length - 1));
                    dataGridView1.Rows[op].Cells[5].Value = quickSortCounter;
                    Graphic(4, subtableSize, quickSortCounter);
                    quickSortCounter = 0;

                    if ((array.Length - subtableSize < k) && (array.Length != subtableSize))
                        k = array.Length - subtableSize;
                    UpdateProgressBar(subtableSize, array.Length);
                    ControlsActive();
                    op++;
                }
            }
        }

        #region Expansion buttons
        private void buttonExpand1_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(textBox1.Text);
            form.Size = new Size(this.Width, this.Height);
            form.ShowDialog();
        }
        private void buttonExpand2_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(textBox2.Text);
            form.Size = new Size(this.Width, this.Height);
            form.ShowDialog();
        }
        private void buttonExpand3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(textBox3.Text);
            form.Size = new Size(this.Width, this.Height);
            form.ShowDialog();
        }
        private void buttonExpand4_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(textBox4.Text);
            form.Size = new Size(this.Width, this.Height);
            form.ShowDialog();
        }
        private void buttonExpand5_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(textBox5.Text);
            form.Size = new Size(this.Width, this.Height);
            form.ShowDialog();
        }
        private void buttonExpand6_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(textBox6.Text);
            form.Size = new Size(this.Width, this.Height);
            form.ShowDialog();
        }
        #endregion

        #endregion

        #region Algorithms
        private void BubbleSort(int[] array, int op)
        {
            int arrLength = array.Length;
            bool swapped;
            int counter = 0; // operation counter

            do
            {
                swapped = false;
                for (int i = 1; i < arrLength; i++)
                {
                    if (array[i - 1] > array[i])
                    {
                        int temp = array[i - 1];
                        array[i - 1] = array[i];
                        array[i] = temp;
                        swapped = true;
                        counter += 3; // increment counter when elements are swapped
                    }
                    counter += 2; // increment counter for each loop iteration
                }
                arrLength--;
            } while (swapped);
            dataGridView1.Rows[op].Cells[1].Value = counter;
            Graphic(0, array.Length, counter);
            if (array.Length == n)
                Invoke(new Action(() => textBox2.Text = string.Join("; ", array)));
        }

        private void SelectionSort(int[] array, int op)
        {
            int arrLength = array.Length;
            int counter = 0;
            for (int i = 0; i < arrLength - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < arrLength; j++)
                {
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                    counter++;
                }
                if (minIndex != i)
                {
                    int temp = array[i];
                    array[i] = array[minIndex];
                    array[minIndex] = temp;
                    counter += 3;
                }
            }
            dataGridView1.Rows[op].Cells[2].Value = counter;
            Graphic(1, array.Length, counter);
            if (array.Length == n)
                Invoke(new Action(() => textBox3.Text = string.Join("; ", array)));
        }

        private void InsertionSort(int[] array, int op)
        {
            int arrLength = array.Length;
            int counter = 0;
            for (int i = 1; i < arrLength; i++)
            {
                int key = array[i];
                int j = i - 1;
                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                    counter += 2;
                }
                array[j + 1] = key;
                counter++;
            }
            dataGridView1.Rows[op].Cells[3].Value = counter;
            Graphic(2, array.Length, counter);
            if (array.Length == n)
                Invoke(new Action(() => textBox4.Text = string.Join("; ", array)));
        }

        private void ShellSort(int[] array, int op)
        {
            int arrLength = array.Length;
            int g = arrLength / 2;
            int counter = 0;
            while (g > 0)
            {
                for (int i = g; i < arrLength; i++)
                {
                    int temp = array[i];
                    int j = i;
                    while (j >= g && array[j - g] > temp)
                    {
                        array[j] = array[j - g];
                        j -= g;
                        counter += 2;
                    }
                    array[j] = temp;
                    counter++;
                }
                g /= 2;
            }
            dataGridView1.Rows[op].Cells[4].Value = counter;
            Graphic(3, array.Length, counter);
            if (array.Length == n)
                Invoke(new Action(() => textBox5.Text = string.Join("; ", array)));
        }

        #region QuickSort
        public void QuickSort(int[] array, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = Partition(array, left, right);
                quickSortCounter += right - left;
                QuickSort(array, left, pivotIndex - 1);
                QuickSort(array, pivotIndex + 1, right);
            }

            if (array.Length == n)
                Invoke(new Action(() => textBox6.Text = string.Join("; ", array)));
        }
        int Partition(int[] array, int left, int right)
        {
            int pivot = array[right];
            int i = left - 1;
            for (int j = left; j < right; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }
            Swap(array, i + 1, right);

            return i + 1;
        }
        void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        #endregion

        #endregion

        #region Interface
        public void Graphic(int sortNumber, int dataSize, int counter)
        {
            double x = dataSize;
            double y = counter;
            chart1.Series[sortNumber].Points.AddXY(x, y);
        }

        public void ArrayLength()
        {
            n = (int)numericUpDown1.Value;
            array = new int[n];
            this.Text = $"Maximum size = {n}";
        }

        public void UpdateProgressBar(int value1, int value2)
        {
            int procent = 100 * value1 / value2;
            progressBar1.Value = procent;
        }

        public void AllClear()
        {
            TableClear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            progressBar1.Value = 0;
        }

        public void TableClear()
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            int rowCount = dataGridView1.Rows.Count;
            for (int i = rowCount - 2; i >= 0; i--)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
            dataGridView1.Refresh();
        }

        public void Graphic()
        {
            Axis ax = new Axis();
            ax.Title = "Table size";
            ax.TitleFont = new Font("Arial", 8, FontStyle.Bold);
            chart1.ChartAreas[0].AxisX = ax;
            Axis ay = new Axis();
            ay.Title = "Number of operations";
            ay.TitleFont = new Font("Arial", 8, FontStyle.Bold);
            chart1.ChartAreas[0].AxisY = ay;
        }

        int[] ArrayCopy(int subtableSize)
        {
            int[] subtable = new int[subtableSize];
            Array.Copy(array, 0, subtable, 0, subtableSize);
            return subtable;
        }

        public void ControlsActive()
        {
            if (progressBar1.Value == progressBar1.Maximum)
            {
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                }
            }
            else
            {
                // Lock all controls on the form
                foreach (Control control in this.Controls)
                {
                    control.Enabled = false;
                }
            }
        }
        #endregion
    }
}
