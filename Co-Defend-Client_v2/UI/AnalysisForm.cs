using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Co_Defend_Client_v2.UI
{
    public partial class AnalysisForm : Form
    {
        public AnalysisForm(string date, string table_name)
        {
            InitializeComponent();

            analysisChart1.TableName = table_name;
            analysisChart1.Date = date;
        }

        private void AnalysisForm_Load(object sender, EventArgs e)
        {
            #region Set Form To Center
            Rectangle screenBound = Screen.PrimaryScreen.WorkingArea;
            int x = screenBound.Width / 2 - Width / 2;
            int y = screenBound.Height / 2 - Height / 2;

            SetBounds(x, y, Width, Height);
            #endregion
        }
    }
}
