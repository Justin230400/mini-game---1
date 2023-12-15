using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect0607
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
            Init1(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MapReset();
            this.Invalidate(); 
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int i, j;
            Graphics g = e.Graphics;
            //水平隔線..以pen1畫隔線      //NH1 NH+1, NW1 NW+1,   WW2=WW/2; 
            for (i = 1; i <= NH1; i++)     // 座標1=1*WW,  y1座標 i*WW, x2=NW1*WW, y2 i*WW  矩形寬度 WW  矩形高度WW
                g.DrawLine(pen1, WW,i*WW,NW1*WW,i*WW ); //(x1座標, y1座標, x2, y2, 矩形寬度, 矩形高度)
            
            //畫垂直線
            for (j = 1; j <= NW1; j++)   // 座標1=j*WW,  y1座標 1*WW, x2=j*WW, y2 NH1*WW
                g.DrawLine(pen1, j*WW, WW, j*WW, NH1*WW);

            //畫map 字元 
            //畫map字元.   //WW2=WW/2; 
            int x,y;
            Bitmap bmp;
            for (y = 1; y <= NH; y++) for (x = 1; x <= NW; x++)
                {
                    //g.DrawString(map[y, x].ToString(), drawFont, drawBrush,
                    //     x * WW , y * WW, format1);  //WW2=WW/2 
                    if (map[y, x] == 0) continue;
                    bmp = bmp42[map[y, x]];
                    g.DrawImage(bmp, x * WW, y * WW, WW, WW);
                }

        }
        //bool select = false;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int x1, y1, x2, y2,a,b,x,y;
            Graphics g = this.CreateGraphics();

            if ( e.Button == MouseButtons.Right)
            {
                if (FindBlock()) //若回傳true..表有找到可消掉的兩字元..
                {
                    g.DrawRectangle(pen2, a1 * WW, b1 * WW, WW, WW);
                    select = true;
                    sos++; //求救一次..
                    this.label2.Text = "求救:" + sos + "次";
                }
                else
                {
                    MessageBox.Show("GameOver");
                    MapReset();
                }
                return;
            }


            
            x= e.X; y = e.Y;
            a = x / WW; b = y / WW;
            //點擊..
            //若未點擊 則表第一次點擊..
            if (select == false)
            {              
                 
                a1 = a; b1 = b;//轉成格子座標.
                if (a1 < 1 || b1 < 1 || a1 > NW || b1 > NH) return;
             
                select = true; //設定成點擊過了..
                //在(x1,y1) 位置畫出紅色框框.
                
                g.DrawRectangle(pen2, a1 * WW, b1 * WW, WW, WW); 
            }
            else  //select為=true, 第2次點擊..
            {
                select = false;
                
                a2 = a; b2 = b;//轉成格子座標.
                if (a2 < 1 || b2 < 1 || a2 > NW || b2 > NH)
                { //第2次點擊失敗..所以取消第一次點擊 重新點擊..
                    //將第一次點擊的框框 去掉..
                    g.DrawRectangle(pen1, a1 * WW, b1 * WW, WW, WW);
                    
                    return;
                }
                if( map[b2, a2]  !=map[b1, a1])
                {
                    g.DrawRectangle(pen1, a1 * WW, b1 * WW, WW, WW);
                    
                    return; 
                } 

                g.DrawRectangle(pen3, a2 * WW, b2 * WW, WW, WW);
                //檢查 (a1, b1), (a2,b2)之間是否有通道可連..
                if (IsLink(a1,b1, a2, b2))
                {
                    map[b1, a1] = 0; map[b2, a2] = 0;
                    //g.DrawRectangle(pen1, a1 * WW, b1 * WW, WW, WW);
                    score += 2; this.label1.Text = "分數:" + score;
                    count = count - 2;
                    this.label3.Text = "方塊數:" + count;
                    this.Invalidate();
                    if(count <= 0)
                    {
                        MessageBox.Show("You Won!");
                        MapReset();
                    }
                    //g.FillRectangle(bgBrush, a1 * WW, b1 * WW, WW, WW);
                    //g.FillRectangle(bgBrush, a2 * WW, b2 * WW, WW, WW);
                }
                else
                {
                    g.DrawRectangle(pen1, a1 * WW, b1 * WW, WW, WW);
                }
                
            }

        }
    }
}
