using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVN
{
    // đưa dữ liệu trên vào để chuyển thành dữ liệu phù hợp để vào tìm vị trí đặt
    public class ChuyenDoiDuLieu
    {
        private List<List<int>> cotLienKet;
        private List<int> cotGoc;
        private List<ToaDo> toaDoCot;
        private int soCot;
        public List<List<List<int>>> cay = new List<List<List<int>>>();
        public List<List<List<string>>> cay_maCot = new List<List<List<string>>>();
        public List<List<List<string>>> cay_tenCot = new List<List<List<string>>>();
        public List<List<List<double[]>>> cay_toaDo = new List<List<List<double[]>>>();
        public List<List<int>> parent = new List<List<int>>();
        private List<bool> daxet = new List<bool>();
        public List<List<double>> d = new List<List<double>>();
        public List<List<double>> w = new List<List<double>>();
        public List<List<int>> l = new List<List<int>>();

        public ChuyenDoiDuLieu(List<List<int>> cotLienKet, List<int> cotGoc, int soCot, List<string> maCot, List<string> tenCot, List<ToaDo> toaDoCot)
        {
            this.cotLienKet = cotLienKet;
            this.cotGoc = cotGoc;
            this.soCot = soCot;
            this.toaDoCot = toaDoCot;
            for (int i = 0; i < soCot; i++)
            {
                daxet.Add(false);
            }
            chuyen();
            this.w = this.d;
            for (int i = 0; i < this.cay.Count; i++)
            {
                this.cay_maCot.Add(new List<List<string>>());
                this.cay_tenCot.Add(new List<List<string>>());
                this.cay_toaDo.Add(new List<List<double[]>>());
                for (int j = 0; j < this.cay[i].Count; j++)
                {
                    this.cay_maCot[i].Add(new List<string>());
                    this.cay_tenCot[i].Add(new List<string>());
                    this.cay_toaDo[i].Add(new List<double[]>());
                    for (int k = 0; k < this.cay[i][j].Count; k++)
                    {
                        this.cay_maCot[i][j].Add(maCot[this.cay[i][j][k]]);
                        this.cay_tenCot[i][j].Add(tenCot[this.cay[i][j][k]]);
                        this.cay_toaDo[i][j].Add(new double[2] {toaDoCot[this.cay[i][j][k]].getX(), toaDoCot[this.cay[i][j][k]].getY()});
                    }
                }
            }

        }
        private void chuyen()
        {
            for (int i = 0; i < this.cotGoc.Count; i++)
            {
                this.cay.Add(new List<List<int>>());
                this.parent.Add(new List<int>());
                this.d.Add(new List<double>());
                this.l.Add(new List<int>());
                List<int> Queue_cot = new List<int>();
                Queue_cot.Add(this.cotGoc[i]);
                List<int> Queue_parent = new List<int>();
                Queue_parent.Add(-1);
                List<int> Queue_parent_2 = new List<int>();
                Queue_parent_2.Add(-222);
                while (Queue_cot.Count > 0)
                {
                    int x = Pop(Queue_cot);
                    //Log(x);
                    int y = Pop(Queue_parent);
                    int z = Pop(Queue_parent_2);
                    List<int> doan = layDoan(x);
                    if (z != -222)
                    {
                        doan.Insert(0, z);
                    }
                    this.cay[i].Add(doan);
                    this.parent[i].Add(y);
                    this.d[i].Add(doDaiDoan(doan));
                    this.l[i].Add(1);
                    int cot_cuoi = doan[doan.Count - 1];
                    List<int> cot_chua_xet = new List<int>();
                    foreach (int j in this.cotLienKet[cot_cuoi])
                    {
                        if (!this.daxet[j])
                        {
                            cot_chua_xet.Add(j);
                        }
                    }
                    foreach (int k in cot_chua_xet)
                    {
                        Queue_cot.Add(k);
                        Queue_parent.Add(this.cay[i].Count - 1);
                        Queue_parent_2.Add(cot_cuoi);
                    }
                }
            }
        }
        private int Pop(List<int> a)
        {
            //Log(a.Count.ToString()+'i');
            int temp = a[a.Count - 1];
            a.RemoveAt(a.Count - 1);
            return temp;
        }
        private List<int> layDoan(int x)
        {
            this.daxet[x] = true;
            List<int> cot_chua_xet = new List<int>();
            foreach (int i in this.cotLienKet[x])
            {
                if (!this.daxet[i])
                {
                    cot_chua_xet.Add(i);
                }
            }
            if (cot_chua_xet.Count == 1)
            {
                List<int> temp = layDoan(cot_chua_xet[0]);
                temp.Insert(0, x);
                return temp;
            }
            else
            {
                List<int> temp = new List<int>();
                temp.Add(x);
                return temp;
            }
        }
        private double khoangCach(ToaDo a, ToaDo b)
        {
            Coordinates toaDo1 = new Coordinates(a);
            Coordinates toaDo2 = new Coordinates(b);
            double doDai = CoordinatesDistanceExtensions.DistanceTo(toaDo1, toaDo2) * 1000;// đơn vị là mét.
            return doDai;
        }
        private double doDaiDoan(List<int> doan)
        {
            double do_dai = 0;
            for (int i = 0; i < doan.Count - 1; i++)
            {
                do_dai += khoangCach(toaDoCot[doan[i]], toaDoCot[doan[i + 1]]);
            }
            return do_dai;
        }
    }
}
