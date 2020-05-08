using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace ChatBox_v3
{
    public partial class Client : Form
    {

        public Client()
        {
            InitializeComponent();
        }

        Socket socket;
        string userName;

        string saveImage = "D:\\Thư mục X\\";
        string saveFileName = "D:\\Thư mục X\\";
        string openFileName = "";

        List<LinkLabel> linkLabelList = new List<LinkLabel>();
        LinkLabel link;

        /// <summary>
        /// Kết nối tới server
        /// </summary>
        void Connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                try
                {
                    socket.Connect("127.0.0.1", 6789);
                    socket.Send(Encoding.UTF8.GetBytes(userName));
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể kết nối đến server", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Thread receive = new Thread(Receive);

                receive.IsBackground = true;
                receive.Start();
            }

            catch (Exception)
            {
                MessageBox.Show("Không thể kết nối tới Server. Vui lòng thử lại", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Ngắt kết nối
        /// </summary>
        void Disconnect()
        {
            if (socket != null)
                socket.Close();
        }

        /// <summary>
        /// Gửi tin nhắn
        /// </summary>
        void SendMessage()
        {
            if (khungNoiDung.Text != "")
            {
                //  Gửi header
                socket.Send(Encoding.UTF8.GetBytes("message"));

                string message = khungNoiDung.Text;
                socket.Send(Encoding.UTF8.GetBytes(message));

                khungHienThi.SelectionAlignment = HorizontalAlignment.Right;
                khungHienThi.AppendText("Bạn :");
                khungHienThi.AppendText(Environment.NewLine);

                MessageWithEmoji(message);

                khungNoiDung.Clear();
            }
        }

        /// <summary>
        /// Gửi file
        /// </summary>
        void SendFile(string fullName)
        {
            try
            {
                loadingBar.Visible = true;

                string[] arrayName = fullName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                string fileName = arrayName[arrayName.Length - 1];

                string fileType = fileName.Substring(fileName.IndexOf("."));

                FileStream fileStream = new FileStream(fullName, FileMode.Open);
                long fileSize = fileStream.Length;

                socket.Send(Encoding.UTF8.GetBytes("file=" + fileName + ";size=" + fileSize));

                int readByte = 0;

                do
                {
                    byte[] buffer = new byte[2048];

                    readByte = fileStream.Read(buffer, 0, buffer.Length);

                    int percent =(int) ((readByte / fileSize) * 100);

                    loadingBar.Increment(percent);

                    if (readByte > 0)
                    {
                        int sendByte = 0;
                        do
                        {
                            sendByte = socket.Send(buffer, readByte, SocketFlags.None);
                            readByte = readByte - sendByte;

                        } while (readByte > 0);

                        readByte = sendByte;
                    }

                    fileSize = fileSize - readByte;

                } while (fileSize > 0);

                fileStream.Close();

                khungHienThi.SelectionAlignment = HorizontalAlignment.Right;
                khungHienThi.AppendText("Bạn :");
                khungHienThi.AppendText(Environment.NewLine);

                //  Hiển thị trực tiếp nếu là file ảnh
                if (fileType == ".jpg" || fileType == ".png")
                    DisplayImage(fullName);
                else
                {
                    khungHienThi.AppendText("Bạn đã gửi một file : ");
                    khungHienThi.AppendText(Environment.NewLine);
                    khungHienThi.AppendText(fileName);
                }

                khungHienThi.AppendText(Environment.NewLine);

                loadingBar.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi gửi tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Gửi header
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        string ReceiveHeader()
        {
            string strHeader = "";

            while (true)
            {
                if (socket.Available > 0)
                {
                    byte[] buffer = new byte[socket.Available];
                    socket.Receive(buffer);

                    strHeader = Encoding.UTF8.GetString(buffer);
                    return strHeader;
                }
            }
        }

        /// <summary>
        /// Nhận filename từ server
        /// </summary>
        void ReceiveFileName()
        {
            byte[] headerByte = new byte[1024];
            socket.Receive(headerByte);
            string header = Encoding.UTF8.GetString(headerByte);

            string[] arrayName = header.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string sender = arrayName[0];
            string fileName = arrayName[1];

            khungHienThi.SelectionAlignment = HorizontalAlignment.Left;
            khungHienThi.AppendText(sender + " đã gửi một file: ");

            link = new LinkLabel();

            linkLabelList.Add(link);

            link.Text = fileName;
            link.AutoSize = true;
            link.Location = khungHienThi.GetPositionFromCharIndex(khungHienThi.TextLength);

            //  Truy cập luồng UI an toàn từ luồng khác
            BeginInvoke((Action)(() =>
            {
                khungHienThi.Controls.Add(link);
            }));

            khungHienThi.SelectionStart = khungHienThi.TextLength;
            khungHienThi.AppendText(Environment.NewLine);

            link.LinkClicked += new LinkLabelLinkClickedEventHandler(LinkedLabelClicked);
        }

        /// <summary>
        /// Nhận file từ server
        /// </summary>
        /// <param name="client"></param>
        void ReceiveFile()
        {
            try
            {
                string strHeader = ReceiveHeader();

                string[] arrayName = strHeader.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                //  Tên file
                string fileName = arrayName[0].Substring(arrayName[0].IndexOf("=") + 1);

                //  Đuôi file
                string fileType = fileName.Substring(fileName.IndexOf("."));

                //  Kích thước file
                string length = arrayName[1].Substring(arrayName[1].IndexOf("=") + 1);
                long fileSize = Convert.ToInt64(arrayName[1].Substring(arrayName[1].IndexOf("=") + 1));


                FileStream fileStream;

                if (fileType == ".jpg" || fileType == ".png")
                    fileStream = new FileStream(saveImage + fileName, FileMode.OpenOrCreate);
                else
                    fileStream = new FileStream(saveFileName + fileName, FileMode.OpenOrCreate);

                khungHienThi.SelectionAlignment = HorizontalAlignment.Left;
                khungHienThi.AppendText("Đang tải file");
                khungHienThi.AppendText(Environment.NewLine);

                int receiveByte = 0;

                do
                {
                    receiveByte = socket.Available;

                    if (receiveByte > 0)
                    {
                        byte[] buffer = new byte[receiveByte];
                        socket.Receive(buffer);
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                    fileSize = fileSize - receiveByte;

                } while (fileSize > 0);

                fileStream.Close();

                //  Hiển thị trực tiếp nếu là file ảnh
                if (fileType == ".jpg" || fileType == ".png")
                    DisplayImageReceive(fileName);

                else
                {
                    khungHienThi.AppendText("Tải file " + fileName + " thành công");
                    khungHienThi.AppendText(Environment.NewLine);

                    DialogResult result = MessageBox.Show("Bạn có muốn mở file", "Đã tải file xong", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                        System.Diagnostics.Process.Start(saveFileName + fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nhận tin nhắn
        /// </summary>
        /// <param name="client"></param>
        void ReceiveMessage()
        {
            byte[] data = new byte[1024];

            socket.Receive(data);
            string message = Encoding.UTF8.GetString(data);

            khungHienThi.SelectionAlignment = HorizontalAlignment.Left;
            MessageWithEmojiReceive(message);

            khungHienThi.AppendText("\n");
        }

        /// <summary>
        /// Nhận dữ liệu từ server
        /// </summary>
        /// <param name="obj"></param>
        void Receive()
        {
            try
            {
                while (true)
                {
                    //  Nhận header từ server
                    string strHeader = ReceiveHeader();

                    //  Xác định loại dữ liệu sẽ nhận được
                    if (strHeader == "message")
                        ReceiveMessage();

                    else if (strHeader == "fileName")
                        ReceiveFileName();

                    else if (strHeader == "file")
                    {
                        ReceiveFile();
                        khungHienThi.AppendText(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhận tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Sự kiện click vào đường dẫn tải file về máy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkedLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //  Đuôi file
            string fileType = link.Text.Substring(link.Text.IndexOf("."));

            if (fileType != ".jpg" || fileType != ".png")
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();

                if (result == DialogResult.OK)
                    saveFileName = folderBrowserDialog1.SelectedPath + "\\";
            }

            socket.Send(Encoding.UTF8.GetBytes("getfile;" + link.Text));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sự kiện click vào nút gửi file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Chọn file để gửi";
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileName = openFileDialog1.FileName;
                SendFile(openFileName);
            }
        }

        /// <summary>
        /// Sự kiện click vào nút gửi tin nhắn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// Đóng chatbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socket != null)
            {
                socket.Send(Encoding.UTF8.GetBytes("close"));
                Disconnect();
            }
        }

        /// <summary>
        /// Sự kiện click vào nút gửi yêu cầu kết nối
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            if (khungUserName.Text != "")
            {
                userName = khungUserName.Text;
                Connect();
            }
        }

        /// <summary>
        /// Hiển thị ảnh
        /// </summary>
        /// <param name="fullName"></param>
        private void DisplayImage(string fullName)
        {
            Image myImage = Image.FromFile(fullName);
            myImage = ResizeImage(myImage);

            System.Drawing.Image tempImage = myImage;
            Clipboard.SetDataObject(tempImage);
            DataFormats.Format image = DataFormats.GetFormat(DataFormats.Bitmap);
            khungHienThi.Paste(image);
        }

        /// <summary>
        /// Hiển thị ảnh với tỉ lệ thu nhỏ
        /// </summary>
        /// <param name="fullName"></param>
        private void DisplayImage(string fullName, double percent)
        {
            Image myImage = Image.FromFile(fullName);
            myImage = ResizeImage(myImage, percent);

            System.Drawing.Image tempImage = myImage;
            Clipboard.SetDataObject(tempImage);
            DataFormats.Format image = DataFormats.GetFormat(DataFormats.Bitmap);
            khungHienThi.Paste(image);
        }

        /// <summary>
        /// Thu nhỏ kích thước ảnh
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private Image ResizeImage(Image img)
        {
            //lấy kích thước ban đầu của bức ảnh
            int originalW = img.Width;
            int originalH = img.Height;
            int resizedW, resizedH;
            int size = 250;


            //tính kích thước cho ảnh mới 
            if (originalW < originalH)
                size = (int)(size * 0.85);

            resizedW = size;
            resizedH = (int)(originalH / (originalW / size));


            //tạo 1 ảnh Bitmap mới theo kích thước trên
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //tạo 1 graphic mới từ Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            //vẽ lại ảnh ban đầu lên bmp theo kích thước mới
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //giải phóng tài nguyên mà graphic đang giữ
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }

        /// <summary>
        /// Thu nhỏ kích thước ảnh theo tỉ lệ
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private Image ResizeImage(Image img, double percent)
        {
            //lấy kích thước ban đầu của bức ảnh
            int originalW = img.Width;
            int originalH = img.Height;


            //tính kích thước cho ảnh mới 
            int resizedW = (int)(originalW * percent);
            int resizedH = (int)(originalH * percent);


            //tạo 1 ảnh Bitmap mới theo kích thước trên
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //tạo 1 graphic mới từ Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            //vẽ lại ảnh ban đầu lên bmp theo kích thước mới
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //giải phóng tài nguyên mà graphic đang giữ
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }

        private void MessageWithEmoji(string message)
        {
            string characters = "";

            for (int i = 0; i < message.Length - 1; i++)
            {
                characters = message.Substring(i, 2);

                switch (characters)
                {
                    case ":/":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Confuse.png", 0.22);
                        i++;
                        break;

                    case ":|":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\BlaBla.png", 0.22);
                        i++;
                        break;

                    case ";)":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Blink.png", 0.22);
                        i++;
                        break;

                    case "=(":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Cry.png", 0.22);
                        i++;
                        break;

                    case "DX":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Dead.png", 0.22);
                        i++;
                        break;

                    case ":D":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Hah_3.png", 0.22);
                        i++;
                        break;

                    case ":(":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Sad.png", 0.22);
                        i++;
                        break;

                    case ":)":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Haha_2.png", 0.22);
                        i++;
                        break;

                    case ":x":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Love.png", 0.22);
                        i++;
                        break;

                    case ":o":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Wow.png", 0.22);
                        i++;
                        break;

                    case "=)":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Smile_2.png", 0.22);
                        i++;
                        break;

                    case ":*":
                        DisplayImage("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Huh.png", 0.22);
                        i++;
                        break;

                    default:
                        khungHienThi.AppendText(message.Substring(i,1));
                        break;
                }

                if (i == message.Length - 2)
                    khungHienThi.AppendText(message.Substring(i + 1, 1));
            }

            khungHienThi.AppendText(Environment.NewLine);
        }

        private void MessageWithEmojiReceive(string message)
        {
            string characters = "";

            for (int i = 0; i < message.Length - 1; i++)
            {
                characters = message.Substring(i, 2);

                switch (characters)
                {
                    case ":/":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Confuse.png");
                        i++;
                        break;

                    case ":|":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\BlaBla.png");
                        i++;
                        break;

                    case ";)":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Blink.png");
                        i++;
                        break;

                    case "=(":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Cry.png");
                        i++;
                        break;

                    case "DX":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Dead.png");
                        i++;
                        break;

                    case ":D":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Hah_3.png");
                        i++;
                        break;

                    case ":(":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Sad.png");
                        i++;
                        break;

                    case ":)":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Haha_2.png");
                        i++;
                        break;

                    case ":x":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Love.png");
                        i++;
                        break;

                    case ":o":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Wow.png");
                        i++;
                        break;

                    case "=)":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Smile_2.png");
                        i++;
                        break;

                    case ":*":
                        DisplayEmoji("D:\\Visual Sudio 2019\\ChatBox_v3\\Icon\\Emoji\\Huh.png");
                        i++;
                        break;

                    default:
                        khungHienThi.AppendText(message.Substring(i, 1));
                        break;
                }

                if (i == message.Length - 2)
                    khungHienThi.AppendText(message.Substring(i + 1, 1));
            }

            khungHienThi.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Hiển thị hình ảnh khi nhận được tin nhắn
        /// </summary>
        /// <param name="fileName"></param>
        private void DisplayImageReceive(string fileName)
        {
            Thread t = new Thread(() =>
            {
                DisplayImage(saveImage + fileName);
            });

            t.SetApartmentState(ApartmentState.STA);
            //t.IsBackground = true;
            t.Start();
            t.Join();
        }

        /// <summary>
        /// Hiển thị emoji khi nhận được tin nhắn
        /// </summary>
        /// <param name="fileName"></param>
        private void DisplayEmoji(string fullName)
        {
            Thread t = new Thread(() =>
            {
                DisplayImage(fullName, 0.22);
            });

            t.SetApartmentState(ApartmentState.STA);
            //t.IsBackground = true;
            t.Start();
            t.Join();
        }
    }
}
