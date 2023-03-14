using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PicturePuzzle
{
    

    public partial class Form1 : Form
    {
        int[,] mat = new int[5, 5];
        myPictureBox[,] myPictureBoxArray = new myPictureBox[5, 5];
        int x = 0, y = 4;
        myPictureBox whitePictureBox;
        Image image;
        int width, height;
        Panel p;
        Bitmap bitMap;

        string status = "user";//system-user

        stackFunctions stack;


        public Form1()
        {
            InitializeComponent();

            stack = new stackFunctions();

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (i == 0 && j == 4)  mat[i, j] = 0;
                    else
                        mat[i, j] = 1;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            image = ((PictureBox)sender).Image;

            CreatePuzzle(image);
 
        }

        private void picturebox_Click(object sender, EventArgs e)
        {
            //if (stack.isEmpty())
            //{
            //    you finished the game
            //    MessageBox.Show("you finished th game");
            //}
            moveControl(((myPictureBox)sender));
        }

        private void CreatePuzzle(Image image)
        {
            bitMap = new Bitmap(image);
            width = bitMap.Width;
            height = bitMap.Height;

            createPanel();

            seperateImage();

            mixPicture();

        }

        private void mixPicture()
        {
            Random rnd = new Random();
            for (int i = 0; i < 150; i++)
            {
                int direction = rnd.Next(4);

                switch (direction)
                {
                    case 0: if (whitePictureBox.Y > 0) moveControl(myPictureBoxArray[whitePictureBox.X, whitePictureBox.Y - 1]); break;//up
                    case 1: if (whitePictureBox.Y < 4) moveControl(myPictureBoxArray[whitePictureBox.X, whitePictureBox.Y + 1]); break;//down
                    case 2: if (whitePictureBox.X > 0) moveControl(myPictureBoxArray[whitePictureBox.X - 1, whitePictureBox.Y]); break;//right
                    case 3: if (whitePictureBox.X < 4) moveControl(myPictureBoxArray[whitePictureBox.X + 1, whitePictureBox.Y]); break;//left
                    default:
                        break;
                }
            }


            
        }

        private void moveControl(myPictureBox pictureBox)
        {
            int Xcolor = pictureBox.X;
            int Ycolor = pictureBox.Y;
            int Xwhite = whitePictureBox.X;
            int Ywhite = whitePictureBox.Y;

            if (status == "user")
            {
                if (Xcolor == Xwhite && Ycolor < Ywhite)
                    stack.push(Direction.down);
                if (Xcolor == Xwhite && Ycolor > Ywhite)
                    stack.push(Direction.up);
                if (Xcolor > Xwhite && Ycolor == Ywhite)
                    stack.push(Direction.left);
                if (Xcolor < Xwhite && Ycolor == Ywhite)
                    stack.push(Direction.right);
            }
            


            int temp;
            myPictureBox tempPicturebox;
            Point location = pictureBox.Location;
            Size size = pictureBox.Size;
            int i = location.X / 120;
            int j = location.Y / 120;

            if ((i + 1 == x && j == y) || (i - 1 == x && j == y) || (i == x && j == y + 1) || (i == x && j == y - 1))
            {
                mat[i, j] = 0;
                mat[x, y] = 1;


                p.Controls.Remove(pictureBox);
                p.Controls.Remove(whitePictureBox);


                temp = Xcolor;
                pictureBox.X = Xwhite;
                whitePictureBox.X = temp;

                temp = Ycolor;
                pictureBox.Y = Ywhite;
                whitePictureBox.Y = temp;


                tempPicturebox = new myPictureBox(p.Width, p.Height);
                tempPicturebox.X = whitePictureBox.X;
                tempPicturebox.Y = whitePictureBox.Y;
               
                tempPicturebox.Location = location;
                tempPicturebox.BackColor = Color.White;
                p.Controls.Add(tempPicturebox);
                whitePictureBox = tempPicturebox;


                tempPicturebox = new myPictureBox(p.Width, p.Height, x, y);
                tempPicturebox.X = pictureBox.X;
                tempPicturebox.Y = pictureBox.Y;
                
                tempPicturebox.Click += picturebox_Click;
               
                tempPicturebox.Image = pictureBox.Image;
                p.Controls.Add(tempPicturebox);
                pictureBox = tempPicturebox;


                x = i;
                y = j;


                myPictureBoxArray[Xcolor, Ycolor] = whitePictureBox;
                myPictureBoxArray[Xwhite, Ywhite] = pictureBox;
            }

           
        }

        private void seperateImage()
        {
            myPictureBox picturebox;
            Bitmap map;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    myPictureBoxArray[i, j] = new myPictureBox();
                    if (i == 0 && j == 4)
                    {
                        picturebox = new myPictureBox(p.Width, p.Height, 0, 4);
                        picturebox.BackColor = Color.White;
                        p.Controls.Add(picturebox);

                        picturebox.X = i;
                        picturebox.Y = j;

                        whitePictureBox = picturebox;
                        myPictureBoxArray[i, j] = picturebox;
                    }
                    else
                    {
                        picturebox = new myPictureBox(p.Width, p.Height, i, j);
                        picturebox.Click += picturebox_Click;
                        p.Controls.Add(picturebox);


                        map = new Bitmap(bitMap.Width / 5, bitMap.Height / 5);
                        for (int k = 0; k < map.Width; k++)
                            for (int l = 0; l < map.Height; l++)
                            {
                                map.SetPixel(k, l, bitMap.GetPixel(i * (bitMap.Width / 5) + k, j * (bitMap.Height / 5) + l));
                            }
                        picturebox.Image = map;
                        picturebox.X = i;
                        picturebox.Y = j;

                        if (i > 0) picturebox.validFunctions.Add(Direction.right);
                        if (i < 4) picturebox.validFunctions.Add(Direction.left);
                        if (j > 0) picturebox.validFunctions.Add(Direction.up);
                        if (j < 4) picturebox.validFunctions.Add(Direction.down);

                        myPictureBoxArray[i, j] = picturebox;

                    }

                }
        }

        private void createPanel()
        {
            p = new Panel();
            p.Location = new Point(20, 20);
            p.Size = new System.Drawing.Size(600, 600);
            p.BorderStyle = BorderStyle.FixedSingle;


            Controls.Add(p);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();

            
            pictureBox2.Image = new Bitmap(file.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(stack.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            numericUpDown1.Visible = true;
            button4.Visible = true;

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = stack.Count;

            label1.Text = "there's " + stack.Count.ToString() + " last functions. how many functions do you want to undo?";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            status = "system";
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                Direction? direction = stack.pop();
                switch (direction)
                {
                    case Direction.up: moveControl(myPictureBoxArray[whitePictureBox.X, whitePictureBox.Y - 1]) ; break;
                    case Direction.down: moveControl(myPictureBoxArray[whitePictureBox.X, whitePictureBox.Y + 1]);break;
                    case Direction.right: moveControl(myPictureBoxArray[whitePictureBox.X + 1, whitePictureBox.Y]);break;
                    case Direction.left: moveControl(myPictureBoxArray[whitePictureBox.X - 1, whitePictureBox.Y]);break;
                    default:
                        break;
                }
            }
            status = "user";
            label1.Text = "there's " + stack.Count.ToString() + " last functions. how many functions do you want to undo?";
        }


    }
}


class myPictureBox:PictureBox
{
    public int X { get; set; }
    public int Y { get; set; }

  
    

    public List<Direction> validFunctions { get; set; }

    public myPictureBox(int Width, int Height,int i,int j)
        : base()
    {

        validFunctions = new List<Direction>();


        SizeMode = PictureBoxSizeMode.StretchImage;
        BorderStyle = BorderStyle.FixedSingle;
        Size = new Size(Width / 5, Height / 5);
        Location = new Point(i * (Width / 5), j * (Height / 5));
    }


    public myPictureBox(int Width, int Height)
        : base()
    {

        validFunctions = new List<Direction>();

        SizeMode = PictureBoxSizeMode.StretchImage;
        BorderStyle = BorderStyle.FixedSingle;
        Size = new Size(Width / 5, Height / 5);
    }

    public myPictureBox():base()
    {
        validFunctions = new List<Direction>();
    }
}


enum Direction
{
    up, down, right, left
}


class stackFunctions 
{
    private List<Direction> list;

    public int Count
    {
        get { return list.Count ; }
    }

    public override string ToString()
    {
        string s = string.Empty;
        for (int i = 0; i < list.Count; i++)
        {
            s += list[i].ToString() + "\t";
        }
        return s;
    }

    public void push(Direction direction)
    {
        Direction? direct = top();
        if (direct != null)
        {
            if ((direction == Direction.up && direct == Direction.down) ||
                (direction == Direction.down && direct == Direction.up) ||
                (direction == Direction.right && direct == Direction.left) ||
                (direction == Direction.left && direct == Direction.right))
                pop();
            else
                list.Add(direction);
        }
        else list.Add(direction);

    }

    public bool isEmpty()
    {
        return Count == 0;
    }

    public Direction? top()
    {
        if (list.Count != 0) return list.Last();
        return null;
    }

    public Direction? pop()
    {
        Direction? direction = top();
        if (list.Count != 0)  
            list.RemoveAt(list.Count() - 1);
        return direction;
    }

    public stackFunctions()
    {
        list = new List<Direction>();
    }
}
