using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }



        private void SubjectEntryButton_Click(object sender, EventArgs e)
        {
            SubjectEntryForm subjectEntryForm = new SubjectEntryForm();
            subjectEntryForm.Show();
            this.Hide();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void SubjectScheduleButton_Click(object sender, EventArgs e)
        {
            SubjectScheduleEntryForm subjectScheduleEntryForm = new SubjectScheduleEntryForm();
            subjectScheduleEntryForm.Show();
            this.Hide();
        }
    }
}
