using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVN
{
    // thuật toán tìm các cột lận cận
    public class TimCotLanCan
    {
        List<string> maDoiTuong;
        List<string> maLienKet;
        List<ToaDo> toaDoDaiDien;
        List<string> soThuTu;
        List<string> soHieu;
        List<string> loCapDien;
        private const double maxDistance = 500;//500 mét
        public List<bool> daXet = new List<bool>();
        public List<bool> loi = new List<bool>();
        public List<List<int>> cotLienKet = new List<List<int>>();
        public List<List<string>> maCotLienKet = new List<List<string>>();
        public int soCot;
        private List<int> cotGoc = new List<int>();
        public TimCotLanCan(List<string> maDoiTuong, List<string> maLienKet, List<ToaDo> toaDoDaiDien, List<string> soThuTu, List<string> soHieu, List<string> loCapDien)
        {

            this.maDoiTuong = maDoiTuong;
            this.maLienKet = maLienKet;
            this.toaDoDaiDien = toaDoDaiDien;
            this.soThuTu = soThuTu;
            this.soHieu = soHieu;
            this.loCapDien = loCapDien;
            soCot = soHieu.Count;
            for (int i = 0; i < this.soCot; i++)
            {
                this.daXet.Add(false);
                this.cotLienKet.Add(new List<int>());
                if (this.soHieu[i].Length < 3 || this.soHieu[i].Substring(0, 3) != "COT")
                {
                    this.loi.Add(true);
                }
                else
                {
                    this.loi.Add(false);
                }
                List<string> temp = new List<string>();
                temp.Add(this.maDoiTuong[i]);
                this.maCotLienKet.Add(temp);
                if (this.soThuTu[i] == "1" || this.soThuTu[i] == "1XT")
                {
                    this.cotGoc.Add(i);
                }
            }
        }
        private double d(ToaDo a, ToaDo b)
        {
            Coordinates toaDo1 = new Coordinates(a);
            Coordinates toaDo2 = new Coordinates(b);
            double doDai = CoordinatesDistanceExtensions.DistanceTo(toaDo1, toaDo2) * 1000;// đơn vị là mét.
            return doDai;
        }
        private bool nhanhCon(int i, int j)
        {
            if (!this.loi[j] && !this.loi[i] && cap(this.soHieu[i]) == cap(this.soHieu[j]) - 1)
            {
                if (this.soHieu[i].Substring(3) == this.loCapDien[j])
                {
                    //Log(soHieu[i] + ',' + soHieu[j]);
                    return true;
                }
                if (this.loCapDien[j].IndexOf(this.soHieu[i].Substring(3)) == 0 && this.loCapDien[j][this.soHieu[i].Length - 3] == '_')
                {
                    //Log(soHieu[i] + ',' + soHieu[j]);
                    return true;
                }
            }
            return false;
        }
        private int cap(string ten)
        {
            int count = 0;
            for (int i = 0; i < ten.Length; i++)
            {
                if (ten[i] == '-')
                {
                    count += 1;
                }
            }
            return count;
        }
        public void chuyenDoi()
        {
            foreach (int i in this.cotGoc)
            {
                if (!this.loi[i] && !this.daXet[i])
                {
                    List<int> Q = new List<int>();
                    Q.Add(i);
                    Program.Log("\n" +soHieu[i]);
                    while (Q.Count > 0)
                    {
                        //Log(Q[0]);
                        int u = Q[Q.Count - 1];
                        this.daXet[u] = true;
                        //Console.Write("Truoc :" + Q.Count.ToString());
                        Q.RemoveAt(Q.Count - 1);
                        //Console.Write("Sau :" + Q.Count.ToString());
                        //Log(Q.Count.ToString()+"|||");
                        //Log(this.soHieu[u]);
                        for (int j = 0; j < this.soCot; j++)
                        {
                            if (!this.loi[j] && !this.daXet[j] && u != j)
                            {
                                if (this.loCapDien[u] == this.loCapDien[j] || this.nhanhCon(u, j))
                                {
                                    if (0 < this.d(this.toaDoDaiDien[u], this.toaDoDaiDien[j]) && this.d(this.toaDoDaiDien[u], this.toaDoDaiDien[j]) <= maxDistance)
                                    {
                                        bool da_co = false;
                                        int count = this.cotLienKet[u].Count;
                                        for (int t = 0; t < count; t++)
                                        {
                                            int k = cotLienKet[u][t];
                                            if (this.loCapDien[k] == this.loCapDien[j])
                                            {
                                                //Log(soHieu[k] + "|" + soHieu[j] + "|" + loCapDien[j]);
                                                da_co = true;
                                                if (this.d(this.toaDoDaiDien[u], this.toaDoDaiDien[j]) < this.d(this.toaDoDaiDien[u], this.toaDoDaiDien[k]))
                                                {
                                                    this.cotLienKet[u][t] = j;
                                                    Q.Add(j);
                                                }
                                            }
                                        }
                                        if (!da_co)
                                        {
                                            this.cotLienKet[u].Add(j);
                                            Q.Add(j);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < this.soCot; i++)
            {
                foreach (int j in this.cotLienKet[i])
                {
                    if (!this.cotLienKet[j].Contains(i))
                    {
                        this.cotLienKet[j].Add(i);
                    }
                }
            }
            for (int i = 0; i < this.soCot; i++)
            {
                foreach (int j in this.cotLienKet[i])
                {
                    this.maCotLienKet[i].Add(this.maDoiTuong[j]);
                }
            }
        }
        public List<List<int>> layCotLienKet()
        {
            return this.cotLienKet;
        }
        public List<int> layCotGoc()
        {
            return this.cotGoc;
        }
    }
}
