using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StreameReader_read_ini
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string FilePath;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button_ReadFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;           
            }
            GetValue(FilePath, textBox_Title.Text, textBox_Item.Text);
        }

        private void GetValue(string filepath, string Title, string Item)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StreamReader sr = new StreamReader(FilePath, Encoding.UTF8);
                bool Title_Check = false;
                //avoid while loops
                int loop_flag = 0;
                while (true)
                {
                    string sr_buffer = sr.ReadLine();
                    //skip start at beginning of the string (null)
                    if (sr_buffer == null || sr_buffer == "")
                    {
                        MessageBox.Show("no match title or item");
                        break;
                    }
                    //skip start at beginning of the string (// and #)
                    if (Regex.Match(sr_buffer, @"^(//|#).*$").Success)
                    {
                        continue;
                    }
                    //check "[" and "]"
                    if (Regex.Match(sr_buffer, @"^\[.*\]").Success)
                    {
                        //check Title is right
                        if (Regex.Match(sr_buffer, @"\b" + Title + @"\b").Success)
                        {
                            Title_Check = true;
                        }
                        //if(Regex.Match(sr_buffer, Title).Success)
                        //{
                        //    Title_Check = true;
                        //}
                        //if(sr_buffer.Equals(Title))
                        //{
                        //    Title_Check = true;
                        //}else
                        //{
                        //    Title_Check = false;
                        //}
                    }
                    if (Title_Check)
                    {
                        string[] GetValue_buffer = sr_buffer.Split('=');
                        if (Regex.Match(GetValue_buffer[0].Trim(), Item).Success)
                        {
                            sb.Append(GetValue_buffer[1].Trim());
                            break;
                        }
                        loop_flag += 1;
                        if (loop_flag >= 5)
                        {
                            MessageBox.Show("Break");
                            break;
                        }
                    }
                }
                sr.Close();

                if (CheckIsDigitsOnly(sb.ToString()))
                {
                    label_Result.Text = sb.ToString();
                    Console.WriteLine(sb.ToString());
                }
                else
                {
                    label_Result.Text = "";
                    MessageBox.Show("Not Digits, Please Check your ini File");
                    Console.WriteLine("not");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
            
        

        bool CheckIsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

    }
}
