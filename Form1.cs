using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImperialCalendar
{
    public partial class Form1 : Form
    {
        // Обновление названий дней недели
        private static string[] daysOfWeek = { "Веллентаг", "Аубентаг", "Марктаг", "Баккентаг", "Бецальтаг", "Конистаг", "Ангестатг", "Фестаг" };
        private int[] startDayOfMonth = new int[] { 0, 4, 4, 5, 5, 6, 7, 0, 0, 1, 1, 1, 1, 2, 3, 4, 0, 5 };
        private int baseYear = 2522; // Базовый год, для которого известны начальные дни недели

        // Функция для получения начального дня недели для месяца в заданном году
        private int GetStartDayOfWeekForMonth(int year, int month)
        {
            int yearDifference = year - baseYear; // Разница между текущим годом и базовым годом
            int startDayIndex = (startDayOfMonth[month - 1] + yearDifference) % 8; // Вычисляем индекс начального дня недели с учетом смещения
            if (startDayIndex < 0) startDayIndex += 8; // Если индекс отрицательный, корректируем его
            return startDayIndex;
        }
        private string[] monthNames = {
    "Хексенстаг", "Нахексен", "Ярдрунг", "Миттерфрул", "Пфлугцайт",
    "Зигмарцайт", "Зоммерцайт", "Зоннстил", "Форгехайм", "Гехаймнистаг",
    "Нахгехайм", "Эрнтецайт", "Миттербст", "Брауцайт", "Кальдецайт",
    "Ульрикцайт", "Мондштилле", "Форексен"
};
        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.BG;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            InitializeCalendarControls();
        }

        private void InitializeCalendarControls()
        {
            // Заполнение comboBox1 названиями месяцев
            comboBox1.Items.Clear(); // Очищаем существующие элементы
            foreach (string monthName in monthNames)
            {
                comboBox1.Items.Add(monthName);
            }
            comboBox1.SelectedIndex = 0; // Выбор первого месяца по умолчанию

            numericUpDown1.Minimum = 2000; // Минимальное значение
            numericUpDown1.Maximum = 3000; // Пример максимального значения
            numericUpDown1.Value = 2522; // Начальное значение

            InitializeDaysOfWeekHeaders();
        }


        private void InitializeDaysOfWeekHeaders()
        {
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
                Label lbl = new Label
                {
                    Text = daysOfWeek[i],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(lbl, i, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCalendar((int)numericUpDown1.Value, comboBox1.SelectedIndex + 1);
        }

        private void LoadCalendar(int year, int month)
        {

            // Очистка предыдущих чисел месяца, кроме заголовков дней недели
            while (tableLayoutPanel1.RowCount > 1)
            {
                int row = tableLayoutPanel1.RowCount - 1;
                for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(i, row);
                    if (control != null)
                    {
                        tableLayoutPanel1.Controls.Remove(control);
                    }
                }
                tableLayoutPanel1.RowStyles.RemoveAt(row);
                tableLayoutPanel1.RowCount--;
            }

            int startDayOfWeek = GetStartDayOfWeekForMonth(year, month);

            int daysInMonth = GetDaysInMonth(month);

            // Добавляем пустые ячейки до начального дня недели месяца, если это необходимо
            for (int i = 0; i < startDayOfWeek; i++)
            {
                Label lblEmpty = new Label { Text = "", Dock = DockStyle.Fill };
                tableLayoutPanel1.Controls.Add(lblEmpty, i, 1); // Предполагаем, что заголовки дней недели находятся в первой строке
            }

            // Отображение чисел месяца начиная с правильного дня недели
            for (int day = 1; day <= daysInMonth; day++)
            {
                Label lblDay = new Label
                {
                    Text = day.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                int position = (day + startDayOfWeek - 1) % 8; // Вычисляем позицию дня в строке
                int row = (day + startDayOfWeek - 1) / 8 + 1;  // Вычисляем номер строки
                if (tableLayoutPanel1.RowCount < row + 1)
                {
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
                tableLayoutPanel1.Controls.Add(lblDay, position, row);
            }

        }
        private int GetDaysInMonth(int month)
        {
            if (new int[] { 1, 4, 8, 10, 13, 17 }.Contains(month))
            {
                return 0;
            }
            else if (new int[] { 2, 11 }.Contains(month))
            {
                return 32;
            }
            else
            {
                return 33;
            }
        }
        // Остальные обработчики событий можно оставить пустыми, если они не используются
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}