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
    public partial class ComparativeAnalysisForm : Form
    {
        public ComparativeAnalysisForm(string date1, string table_name1, string date2, string table_name2)
        {
            InitializeComponent();

            analysisChart1.TableName = table_name1;
            analysisChart1.Date = date1;

            analysisChart2.TableName = table_name2;
            analysisChart2.Date = date2;
        }

        private void ComparativeAnalysisForm_Load(object sender, EventArgs e)
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
