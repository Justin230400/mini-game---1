using System;
using System.Drawing;

namespace Connect0607
{
    partial class Form1
    {
        int N = 10;
        int NH = 10, NH1, NH2;//格線的高 偶數.
        int NW = 10, NW1, NW2;//格線的寬.偶數.

        int N1 = 11;  // N + 1;
        int N2 = 12; // N + 2;
        int WW = 60;//方塊寬度.
        int WW2; 
        int[,] map; // =new char[N2, N2];
        Bitmap source;
        Bitmap[] bmp42 = new Bitmap[43];

        char[] a62 = new char[62];
        int[] a100; // = new char[100];  //N*N 

        int score = 0; //目前得分
        int count = 100; //方塊數
        int sos = 0; //求救次數
        bool select = false;
        int a1, b1, a2, b2; //記錄點擊的座標..(a1,b1), (a2,b2) 

        Font drawFont = new Font("Arial", 30);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
        Color bgColor;
        SolidBrush bgBrush;
        Random rand = new Random();
        Pen pen1, pen2, pen3; 

        void Init()
        {
            N = 10; N1 = N + 1; N2 = N + 2;
            NH = 4; NH1 = NH + 1; NH2 = NH + 2;
            NW = 6; NW1 = NW + 1; NW2 = NW + 2; 
            map = new int[NH2, NW2];
            WW2 = WW / 2; 
            //把整張地圖 清空為 0 空白字元. 
            for (int h = 0; h < NH2; h++) for (int w = 0; w < NW2; w++)
                    map[h, w] = 0; 

            a100 = new int[NH * NW];//10x10=100
            SetA62();//只做一次
            SetA100(); //只做一次
            MapReset(); //會做很多次, 重玩呼叫它.
            SetMap();
        }
        void SetMap()
        {
            source = Resource1.animal;
            for(int i = 1;i <= 42;i++)
            {
                bmp42[i] = CreateImage(i - 1);
            }
        }
        Bitmap CreateImage(int n)
        {
            Bitmap bit = new Bitmap(WW, WW);
            Graphics g = Graphics.FromImage(bit);
            Rectangle a = new Rectangle(0, 0, WW, WW);
            Rectangle b = new Rectangle(0, n * 39, 39, 39);
            g.DrawImage(source, a, b, GraphicsUnit.Pixel);
            return bit;
        }

        void Init1()
        {
            pen1 = new Pen(Color.Green, 3); //畫格線
            pen2 = new Pen(Color.Red, 3); //畫第一次 點選 框出. 
            pen3 = new Pen(Color.Yellow, 3);

            bgColor = this.BackColor;
            bgBrush = new SolidBrush(bgColor); 

        }
        void SetA62()   //a62[] 
        {
            char c;
            int i = 0;
            string str = " ";
            for (c = '0'; c <= '9'; c++) a62[i++] = c; //10
            for (c = 'A'; c <= 'Z'; c++) a62[i++] = c; //26
            for (c = 'a'; c <= 'z'; c++) a62[i++] = c; //26  62個字元.
            // NH=10, NW=10,  10*10=100格..  一字元出現4次..
            // 100/4=25..   25個字元.
            // 20*20=400,  400/4=100...100個字元 超過範圍..
            // 62*4=248,  14*14= < 248.. 此遊戲的寬高不能超過14x14....

            for (i = 0; i < 62; i++)
            {
                str = str + a62[i];
            }
            //MessageBox.Show(str);
        }
        void SetA100()
        {
            int i, j;
            for (j = 0, i = 1; i <= NH * NW / 4; i++)
            {
                a100[j++] = i;
                a100[j++] = i;
                a100[j++] = i;
                a100[j++] = i;//
            }

            //地圖周圍設為空白.


        }
        void MapReset()
        {
            int i, j , k, x,y;
            int tmp;
            count = NH*NW; score = 0; sos = 0;
            this.label3.Text = "方塊數:" + count;
            this.label2.Text = "求救" + sos + "次";
            this.label1.Text = "分數" + score;

            for (i = 0; i < NH*NW; i++)
            {
                j = rand.Next(count);//對調..
                tmp = a100[j]; a100[j] = a100[i]; a100[i] = tmp;
            }
            k = 0; 
            for (y = 1; y <= NH; y++) for (x = 1; x <= NW; x++)
                {
                    map[y, x] = a100[k++];
                }
        }

        //水平check..  map[h,w] map[y,x] 
        bool IsXLink(int x11, int y11, int x22, int y22)
        {
            int x, x1, x2;

            if (y11 != y22) return false;

            if (x11 > x22) { x1 = x22; x2 = x11; }
            else { x1 = x11; x2 = x22; }

            //[y11, x1]在左邊, [y11, x2] 在右邊
            //補程式碼 check.. x1+1 到x2是否是否全都空格; 若有非空白 return false
            for(x=x1+1; x<x2; x++)
            {
                if (map[y11, x] != 0) return false; 
            }

            return true;
        }

        //垂直check..  map[h,w] map[y,x]
        bool IsYLink(int x11, int y11, int x22, int y22)
        {
            if (x11 != x22) return false;

            int y, y1, y2;

            if (y11 > y22) { y1 = y22; y2 = y11; }
            else { y1 = y11; y2 = y22; }

            // [y1, x11], 在上面
            // [y2, x11]  在下面
            //補程式碼 check y1+1到 y2之間是否全部空白; 若有非空白 return false 
            for(y=y1+1; y<y2; y++)
            {
                if (map[y, x11] != 0) return false; 
            }
            return true;
        }

        bool Is1Corner(int x11, int y11, int x22, int y22)
        {
            int x1 = x11, x2 = x22, y1 = y11, y2 = y22;
            if (x11 > x22) { x1 = x22; x2 = x11; y1 = y22; y2 = y11; }
            //x1 <= x2  (y1, x1)在左邊,  (y2, x2)在右邊.

            // 3.[y2,x1]*            2.[y2, x2]      <---XLink
            // 空白
            // 1.[y1, x1]          
            // 空白
            // 3.[y2, x1] *           2.[y2, x2]
            //  YLink 
            if (map[y2, x1] == 0)   
            {
                //y1--y2 都要空白..(1,3之間YLink) 且 (2,3之間XLink) 
                //補程式碼  填上座標, 呼叫XLink, YLink,
                if ( IsYLink(x1,y1, x1,y2)     &&   IsXLink(x2,y2, x1,y2)  ) return true; 
            }

            //                       2.[y2, x2]     
            //                         空白
            // 1.[y1, x1]  空白      3.[y1, x2]      <---XLink
            //                          空白 
            //                       2.[y2, x2]
            //                          YLink

            if (map[y1, x2] == 0)   
            {
                // (1,3點間XLink) 且  (2,3點間YLink )
                //補程式碼, 填上座標, 呼叫XLink, YLink,
               if ( IsXLink(x1,y1, x2,y1)    &&   IsYLink(x2,y2, x2,y1)  )  return true; 

            }

            //上面case都沒發生..所以無1轉折點, return  false; 
            return false;
        }

        bool Is2Corner(int x11, int y11, int x22, int y22)
        {
            int x, y;
            int x1 = x11, x2 = x22, y1 = y11, y2 = y22;
            if (x11 > x22) { x1 = x22; x2 = x11; y1 = y22; y2 = y11; }

            //x1 <= x2  (x1, y1), (x2, y2)在右邊.
            //                          [y2, x2]
            //  [y1, x1] ---> *[y1,x]              往右check
            //                          [y2, x2]
            for (x = x1 + 1; x < NW2; x++) //[y1, x1]固定y1. 往右check 空白.
            {
                if (map[y1, x] != 0) break;//
                // [y1, x]這點是空白, 則檢查與 [y2,x2]是否1Corner
                if (Is1Corner(x, y1, x2, y2)) return true;
            }

            //x1 <= x2  (x1, y1), (x2, y2)在右邊.
            //                           [y2, x2]
            //  *[y1,x] <---  [y1, x1]               往左check
            //                           [y2, x2]

            for (x = x1 - 1; x >= 0; x--)//[y1, x1]固定y1. 往左check 空白.
            {
                if (map[y1, x] != 0) break;
                // [y1, x]這點是空白, 則檢查與 [y2,x2]是否1Corner
                if (Is1Corner(x, y1, x2, y2)) return true;
            }

            //x1 <= x2  (x1, y1), (x2, y2)在右邊.
            //                    [y2, x2]
            //
            //    [y1, x1]               往下check
            //       |
            //       *[y,x1]
            //                     [y2, x2]
            for (y = y1 + 1; y < NH2; y++) //[y1, x1]固定x1. 往下
            {
                if (map[y, x1] != 0) break;

                // [y,x1]空白
                if (Is1Corner(x1, y, x2, y2)) return true;
            }

            //x1 <= x2  (x1, y1), (x2, y2)在右邊.
            //                    [y2, x2]
            //       *[y,x1]
            //       |
            //       |
            //    [y1, x1]               往上check
            //       
            //                     [y2, x2]

            for (y = y1 - 1; y >= 0; y--)//[y1, x1]固定x1. 往上
            {
                if (map[y, x1] != 0) break;

                // [y,x1]空白
                if (Is1Corner(x1, y, x2, y2)) return true;
            }
            return false;
        }

        bool IsLink(int x1, int y1, int x2, int y2)
        {
            //先check 兩字元是否相同
            if (map[y1, x1] != map[y2, x2]) return false;

            if (IsXLink(x1, y1, x2, y2)) return true;
            if (IsYLink(x1, y1, x2, y2)) return true;
            if (Is1Corner(x1, y1, x2, y2)) return true;
            if (Is2Corner(x1, y1, x2, y2)) return true;

            return false;
        }
        bool FindBlock()
        {
            int x1, y1, x2, y2;
            for (x1 = 1; x1 <= NW; x1++) for (y1 = 1; y1 <= NH; y1++)
                {
                    if (map[y1, x1] == 0) continue;
                    for (x2 = x1 + 1; x2 <= NW; x2++) for (y2 = 1; y2 <= NH; y2++)
                        {
                            if (map[y2, x2] == 0) continue;
                            if (map[y1, x1] != map[y2, x2]) continue;

                            //map[x2,y2] 與 map[x1, y1] 字元相同...
                            if (IsLink(x1, y1, x2, y2))
                            {
                                this.CreateGraphics().DrawRectangle(pen1, x1 * 50, y1 * 50, 50, 50);
                                a1 = x1; b1 = y1;
                                select = true;
                                return true;
                            }
                        }
                }
            return false;
        }
    }
}