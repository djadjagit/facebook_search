using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace facebook_search
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        [DllImport("user32.dll")]
        public static extern void keybd_event(Keys bVk, byte bScan, UInt32 dwFlags, IntPtr dwExtraInfo);
        public const UInt32 KEYEVENTF_EXTENDEDKEY = 0x01;
        public const UInt32 KEYEVENTF_KEYUP = 0x02;
        StreamReader stream;
        DateTime t1 = DateTime.Now;
        string str = "";
        int scrol = 0;
        int d_x = 0;
        int left_x = 0;
        int ind = 0;
        int ind1 = 0;
        int ind2 = 0;
        string pred = "";
        string pred_pred = "";
        int k = 1;
        int flag = 0;
        int ind_tb = 1;

        public Form1()
        {
            InitializeComponent();
            KeyboardHook.Start();
            KeyboardHook.KeyboardAction += new KeyEventHandler(EventCtrl);
        }

        private void EventCtrl(object sender, KeyEventArgs e)
        {
//            textBox11.Text = e.KeyValue.ToString();
            if (e.KeyCode.ToString()=="Left")
                {
                    ind_tb--;
                    if (ind_tb==0)
                    {
                        ind_tb = 6;
                    }
                }
            if (e.KeyCode.ToString() == "Right")
                {
                    ind_tb++;
                    if (ind_tb == 7)
                    {
                        ind_tb = 1;
                    }
                }
            for (int i = 2; i < 9; i++)
                this.Controls["textBox" + i.ToString()].BackColor = Color.White;
            checkBox1.BackColor = Color.White;
            int coor = 0;
            switch (ind_tb)
            {
                case 1: textBox2.BackColor = Color.Blue; coor = Convert.ToInt32(textBox2.Text); break;
                case 2: textBox3.BackColor = Color.Blue; coor = Convert.ToInt32(textBox3.Text); break;
                case 3: textBox5.BackColor = Color.Blue; coor = Convert.ToInt32(textBox5.Text); break;
                case 4: textBox6.BackColor = Color.Blue; coor = Convert.ToInt32(textBox6.Text); break;
                case 5: textBox8.BackColor = Color.Blue; coor = Convert.ToInt32(textBox8.Text); break;
                case 6: checkBox1.BackColor = Color.Blue; break;
            }
            if (e.KeyCode.ToString() == "Up")
            {
                coor=coor+5;
                if (ind_tb==6)
                {
                    checkBox1.Checked = true;
                    scrol = 0;
                }
            }
            if (e.KeyCode.ToString() == "Down")
            {
                coor=coor-5;
                if (ind_tb == 6)
                {
                    checkBox1.Checked = false;
                }
            }
            switch (ind_tb)
            {
                case 1: textBox2.Text = coor.ToString(); break;
                case 2: textBox3.Text = coor.ToString(); break;
                case 3: textBox5.Text = coor.ToString(); break;
                case 4: textBox6.Text = coor.ToString(); break;
                case 5: textBox8.Text = coor.ToString(); break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            webBrowser1.ProgressChanged += webBrowser1_ProgressChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri(textBox1.Text.ToString());
            webBrowser1.Url = uri;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // перед тем как нажать эту кнопку нужно перейти в обычном браузере по адресу: https://searchisback.com/#people
            // и получить ссылку вида https://www.facebook.com/search/106388046062960/residents/present/intersect/
            // ее скопировать в textBox и нажать кнопку Go
            //scrol = scrol + 10;
            ind = 0;
            label1.Visible = true;
            label2.Visible = false;
            t1 = DateTime.Now;
            SetCursorPos(this.Left + 320, this.Top + 120);
            timer1.Enabled = true;
            //webBrowser1.Navigate("javascript: window.scroll(0,"+scrol.ToString()+")");
            //webBrowser1.Document.Body.ScrollTop = 50;
        }

        private void webBrowser1_StatusTextChanged(object sender, EventArgs e)
        {
            this.Text =(DateTime.Now-t1).TotalSeconds.ToString()+":"+webBrowser1.StatusText;
            if (webBrowser1.StatusText.IndexOf("pages")==-1)
            {
                if (webBrowser1.StatusText.IndexOf("/?ref")==-1)
                {
                    if ((webBrowser1.StatusText.IndexOf("ref=br_rs")>0) && (webBrowser1.StatusText!=pred))
                    {
                        ind++;
                        label1.Text = ind.ToString();
                        File.AppendAllText("log.txt", webBrowser1.StatusText + "\n");
                        pred = webBrowser1.StatusText;
                        t1 = DateTime.Now;
//                        if (ind>20)
//                        {
//                            this.Close();
//                        }
                    }
                    if ((flag==6) && (webBrowser1.StatusText.IndexOf("fref=pb&hc_location=profile_browser") > 0) && (webBrowser1.StatusText != pred_pred))
                    {
                        timer4.Enabled = false;
                        ind1++;
                        label2.Text = ind2.ToString()+":"+ind1.ToString();
                        File.AppendAllText("log_friends.txt", ind2.ToString() + ";" + ind1.ToString()+";"+webBrowser1.StatusText + "\n");
                        pred_pred = webBrowser1.StatusText;
                        t1 = DateTime.Now;
//                        flag = 5;
                        timer4.Enabled = true;
//                        left_x = 520;
//                        Cursor.Position = new Point(this.Left + left_x, this.Top + 280);
//                        Thread.Sleep(100); //https://www.facebook.com/nqt.quangtu?fref=pb&hc_location=profile_browser
                    }
                    if ((flag == 6) && (webBrowser1.StatusText.IndexOf("fref=pb&hc_location=friends_tab") > 0) && (webBrowser1.StatusText != pred_pred))
                    {
                        timer4.Enabled = false;
                        ind1++;
                        label2.Text = ind2.ToString() + ":" + ind1.ToString();
                        File.AppendAllText("log_friends.txt", ind2.ToString() + ";" + ind1.ToString() + ";" + webBrowser1.StatusText + "\n");
                        pred_pred = webBrowser1.StatusText;
                        t1 = DateTime.Now;
                        flag = 5;
                        timer4.Enabled = true;
                        left_x = Convert.ToInt32(textBox8.Text);//520
                        Cursor.Position = new Point(this.Left + left_x, this.Top + Convert.ToInt32(textBox5.Text)); //440
                        //                        Thread.Sleep(100);
                    }
                    if ((flag == 5) && (webBrowser1.StatusText.IndexOf("fref=pb&hc_location=friends_tab") > 0) && (webBrowser1.StatusText != pred_pred))
                    {
//                        timer4.Enabled = false;
                        ind1++;
                        label2.Text = ind2.ToString() + ":" + ind1.ToString();
                        File.AppendAllText("log_friends.txt", ind2.ToString() + ";" + ind1.ToString() + ";" + webBrowser1.StatusText + "\n");
//                        pred = webBrowser1.StatusText;
                        t1 = DateTime.Now;
                        flag = 6;
                        left_x = Convert.ToInt32(textBox6.Text);//140
                        Cursor.Position = new Point(this.Left + left_x, this.Top + Convert.ToInt32(textBox5.Text));//440
                        Thread.Sleep(100);
                        timer4.Enabled = true;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if ((DateTime.Now - t1).TotalSeconds < 300)
            {
                d_x = d_x + 10;
                if (d_x<370)
                {
                    SetCursorPos(this.Left + 320, this.Top + 120+d_x);
                }
                else
                {
                    d_x = 360;
                    k = k * (-1);
                    SetCursorPos(this.Left + 320, this.Top + 120 + d_x+k);
                    scrol = scrol + 10;
                    webBrowser1.Navigate("javascript: window.scroll(0," + scrol.ToString() + ")");
                }
                timer1.Enabled = true;
            }
            else this.Close();
        }

        private void button3_Click(object sender, EventArgs e) 
        {
            ind1 = 0;
            ind2 = 0;
            label1.Visible = false;
            label2.Visible = true;
            stream = new StreamReader("log_out.txt"); //Открываем файл для чтения
            string str = stream.ReadLine();
            webBrowser1.Navigate(str);
            flag = 1;
//            SetCursorPos(this.Left + 640, this.Top + 230);//добавление в друзья
            SetCursorPos(this.Left + 740, this.Top + 230);//отправка сообщения
        }

        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "CurrentProgress", e.CurrentProgress);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", " MaximumProgress", e.MaximumProgress);
            messageBoxCS.AppendLine();
            //            MessageBox.Show(messageBoxCS.ToString(), "ProgressChanged Event");
            textBox1.Text = messageBoxCS.ToString();
            button5.Text = (DateTime.Now - t1).TotalSeconds.ToString();
            if ((flag == 4) && (e.MaximumProgress==0))
            {
                timer3.Enabled = false;
                timer3.Interval = 50;
                left_x = Convert.ToInt32(textBox6.Text);//140
                flag = 6;
                t1 = DateTime.Now;
                label2.Text = ind2.ToString() + ":" + ind1.ToString();
//                timer4.Interval = 2000;
                timer4.Enabled = true;
            }
            if ((flag == 3) && (e.MaximumProgress==0))
            {
//                webBrowser1.Navigate("javascript: window.scroll(0, 0)");
                Thread.Sleep(200);
                left_x = Convert.ToInt32(textBox2.Text); //460
                Cursor.Position = new Point(this.Left + left_x, this.Top + Convert.ToInt32(textBox3.Text)); //280
                Thread.Sleep(200);
                //                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                if (webBrowser1.StatusText.IndexOf("pb_friends_tl") > 0)
                {
                    //                    Cursor.Position = new Point(this.Left + 460, this.Top + 279);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                    Thread.Sleep(200);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                }
                else
                {
                    Cursor.Position = new Point(this.Left + left_x, this.Top + Convert.ToInt32(textBox5.Text)); //440
                    Thread.Sleep(200);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                    Thread.Sleep(200);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                }
                //                Cursor.Position = new Point(this.Left + 460, this.Top + 279);
                //                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                //                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                flag = 4;
                timer3.Interval = 30000;
                timer3.Enabled = true;
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (flag == 1)//нажимает кнопку message
            {
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                Cursor.Position = new Point(this.Left + 740, this.Top + 230);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                flag = 0;
                timer2.Enabled = true;
            }
            if (flag == 2)// нажимает кнопку добавить в друзья
            {
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                Cursor.Position = new Point(this.Left + 640, this.Top + 230);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                flag = 0;
                ind1++;
                label2.Text = ind1.ToString();
                Thread.Sleep(100);
                timer3.Enabled = true;
            }
//            if (flag==3)
//            {
//                int q = 0;
////                while (webBrowser1.StatusText.IndexOf("Готово") ==-1)
//                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
//                {
//                    q++;
//                    t1 = DateTime.Now;
////                    Application.DoEvents();
//                }
//                textBox1.Text = q.ToString();
//                Cursor.Position = new Point(this.Left + 460, this.Top + 280);
////                Thread.Sleep(200);
////                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                Thread.Sleep(200);
//                if (webBrowser1.StatusText.IndexOf("pb_friends_tl")>0)
//                {
////                    Cursor.Position = new Point(this.Left + 460, this.Top + 279);
//                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                }
//                else
//                {
//                    Cursor.Position = new Point(this.Left + 460, this.Top + 440);
//                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                }
//                //                Cursor.Position = new Point(this.Left + 460, this.Top + 279);
//                //                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                //                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
//                flag = 4;
//                ind2++;
//                t1 = DateTime.Now;
//                label2.Text =ind2.ToString()+":"+ind1.ToString();
////                timer4.Enabled = true;
//            }
        }

        private void timer2_Tick(object sender, EventArgs e) //отправляет сообщения из буфера обмена
        {
            timer2.Enabled = false;
            keybd_event(Keys.ControlKey, 0, 0, IntPtr.Zero);
            keybd_event(Keys.V, 0, 0, IntPtr.Zero);
            Thread.Sleep(100);
            keybd_event(Keys.V, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(Keys.ControlKey, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(Keys.Tab, 0, 0, IntPtr.Zero);
            Thread.Sleep(100);
            keybd_event(Keys.Tab, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            Thread.Sleep(100);
            keybd_event(Keys.Return, 0, 0, IntPtr.Zero);
            Thread.Sleep(100);
            keybd_event(Keys.Return, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            ind1++;
            label2.Text = ind1.ToString();
            Thread.Sleep(100);
            timer3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ind1 = 0;
            label1.Visible = false;
            label2.Visible = true;
            stream = new StreamReader("log_out.txt"); //Открываем файл для чтения
            flag = 2;
            SetCursorPos(this.Left + 640, this.Top + 230);//добавление в друзья
        }

        private void timer3_Tick(object sender, EventArgs e)//управляющий таймер для отправки сообщений и добавления в друзья
        {
            timer3.Enabled = false;
            try
            {
                str = stream.ReadLine();
                ind2++;
                if ((str.Length >11) && (str.IndexOf(";")==-1))
                {
                    scrol = 0;
                    webBrowser1.Navigate(str);
                }
                else
                {
                    timer3.Enabled = true;
                }
            }
            catch 
            {
                textBox1.Text = "Ошибка чтения файла. Завершено";
            }
        }

        private void button5_Click(object sender, EventArgs e) // добавление к списку друзей 
        {
            ind1 = 0;
            ind2 = 0;
            label1.Visible = false;
            label2.Visible = true;
            stream = new StreamReader("log_out.txt"); //Открываем файл для чтения
            flag = 3;
//            SetCursorPos(this.Left + 640, this.Top + 230);//добавление в друзья
            timer3.Enabled = true;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            timer4.Enabled = false;
//            timer4.Interval = 150;
            if ((flag==6) || (flag==5) || (flag == 4))
            {
                if ((DateTime.Now - t1).TotalSeconds < 50)
                {
                    if (checkBox1.Checked == true)
                    {
                        scrol = 0;
                    }
                    else
                    {
                        scrol = scrol + 10;
                    }
                    webBrowser1.Navigate("javascript: window.scroll(0," + scrol.ToString() + ")");
                    k = k * (-1);
                    Cursor.Position = new Point(this.Left + left_x, this.Top + Convert.ToInt32(textBox5.Text) + k);//440
                    timer4.Enabled = true;
                }
                else
                {
                    timer3.Enabled = true;
                    flag = 3;
                }
            }
            button5.Text = (DateTime.Now - t1).TotalSeconds.ToString();
        }

        private void timer5_Tick(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox7.Text = textBox5.Text;
            textBox9.Text = textBox5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetCursorPos(Left + Convert.ToInt32(textBox10.Text), Top + Convert.ToInt32(textBox11.Text));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = textBox2.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (flag==6)
            {
                left_x = Convert.ToInt32(textBox6.Text);//140
            }
        }
    }

    public static class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static event KeyEventHandler KeyboardAction = delegate { };

        public static void Start()
        {
            _hookID = SetHook(_proc);
        }
        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (wParam == (IntPtr)WM_KEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((vkCode > 36) && (vkCode < 41))
                {
                    KeyEventArgs key = new KeyEventArgs((Keys)vkCode);
                    KeyboardAction(null, key);
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

}
