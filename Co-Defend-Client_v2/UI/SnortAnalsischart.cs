using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Co_Defend_Client_v2.UI
{
    public partial class SnortAnalsischart : Form
    {
        public SnortAnalsischart()
        {
            InitializeComponent();
            this.Load += new EventHandler(SnortAnalsischart_Load);
            

        }
        private void SnortAnalsischart_Load(object sender, EventArgs e)
        {
            #region Set Form To Center
            Rectangle screenBound = Screen.PrimaryScreen.WorkingArea;
            int x = screenBound.Width / 2 - Width / 2;
            int y = screenBound.Height / 2 - Height / 2;

            int datalimit = 0;
            string Attack_name = string.Empty;
            string Source_IP = string.Empty;
            String Protocol = string.Empty;
            string Destination_Port = string.Empty;
            String Source_Port = string.Empty;
            string Destination_IP = string.Empty;
            string Source_Mac = string.Empty;
            string Destination_Mac = string.Empty;
            int j = 0;
            int total = 0;

            DataGridViewRowCollection rows = dataGridView1.Rows;
            SetBounds(x, y, Width, Height);
            #endregion

            StreamReader sr = new StreamReader(@"C:\Users\Administrator\Downloads\新增資料夾 (2)\co-defend-client_v2\Co-Defend-Client_v2\bin\Debug\SnortLog\alert.csv");


                
                string Line;
                while ((Line = sr.ReadLine()) != null)
                {
                    string[] ReadLine_Array = Line.Split(',');

                //這邊可以自行發揮
                  foreach (string i in ReadLine_Array)
                  {
                    datalimit++;
                    if (datalimit == 26)
                        rows.Add(new Object[] { Attack_name, Protocol, Source_IP, Source_Port, Destination_IP, Destination_Port, Source_Mac, Destination_Mac, total / 27 });
                    if (datalimit == 27)
                     {
                        
                        datalimit = 0;
                     }
                     
                    if (datalimit == 4)
                     {
                        Attack_name = i;
                        

                     }

                     if (datalimit == 5)
                         Protocol = i;
                     if (datalimit == 6)
                         Source_IP = i;
                     if (datalimit == 7)
                         Source_Port = i;
                     if (datalimit == 8)
                         Destination_IP = i;
                     if (datalimit == 9)
                         Destination_Port = i;
                     if (datalimit == 10)
                         Source_Mac = i;
                     if (datalimit == 11)
                         Destination_Mac = i;

                    
                    total++;
                    






                    
                   }
                

            }
            
            sr.Close();




        }



    }
}

