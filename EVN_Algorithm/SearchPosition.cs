using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVN
{
    //tìm điểm đặt của 1 cây
    public class SearchPosition
    {
        public List<int> parent;
        public List<double> d;
        public List<double> w;
        public List<int> l;
        public int numNode;
        private List<List<int>> child = new List<List<int>>();
        public List<bool> mc = new List<bool>();
        private List<bool> last_mc = new List<bool>();
        public List<bool> db = new List<bool>();
        private List<bool> last_db = new List<bool>();
        public List<bool> dtd = new List<bool>();
        private List<bool> last_dtd = new List<bool>();
        public const int Ce = 1000;
        public const double Tr = (double) 1/30;
        public const int Cr = 10000000;
        public const int Cs = 1000000;
        public const double vanToc = 5000;
        public const double tieuHao = 100; //cong suat tieu thu
        public SearchPosition(List<int> parent, List<double> d, List<double> w, List<int> l)
        {
            this.parent = parent;
            this.d = d;
            this.w = w;
            this.l = l;
            this.numNode = parent.Count;
            for (int i = 0; i < numNode; i++)
            {
                this.child.Add(new List<int>());
            }
            for (int i = 1; i < numNode; i++)
            {
                child[this.parent[i]].Add(i);
            }
            for (int i = 0; i < numNode; i++)
            {
                mc.Add(false);
                last_mc.Add(false);
                db.Add(false);
                last_db.Add(false);
                dtd.Add(false);
                last_dtd.Add(false);
            }
            mc[0] = true;
            last_mc[0] = true;

        }
        private int getLenChild(int parent_pos)
        {
            int dem = 0;
            for (int i = 0; i < this.parent.Count; i++)
            {
                if (this.parent[i] == parent_pos)
                {
                    dem++;
                }
            }
            return dem;
        }
        private List<int> getChildList(int parent_pos)
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < this.parent.Count; i++)
            {
                if (this.parent[i] == parent_pos)
                {
                    temp.Add(i);
                }
            }
            return temp;
        }
        private bool coDTDHoacMC(int pos)
        {
            return (this.mc[pos] || this.dtd[pos]);
        }
        private int getParent(int child_pos)
        {
            return this.parent[child_pos];
        }
        private List<int> getChild(int parent_pos)
        {
            return this.child[parent_pos];
        }
        private int getSuperParent(int child_pos)
        {
            if (coDTDHoacMC(child_pos))
            {
                return child_pos;
            }
            else
            {
                return getSuperParent(getParent(child_pos));
            }
        }
        private int getMCParent(int child_pos)
        {
            if (mc[child_pos])
            {
                return child_pos;
            }
            else
            {
                return getMCParent(getParent(child_pos));
            }
        }
        private int getDTDParent(int child_pos)
        {
            if (dtd[child_pos])
            {
                return child_pos;
            }
            else
            {
                return getDTDParent(getParent(child_pos));
            }
        }
        private List<double> tongDCayCon(int v, int u)
        {
            List<double> kq = new List<double>();
            kq.Add(this.d[v]);
            kq.Add(1);
            if (v == u)
            {
                return kq;
            }
            else
            {
                List<int> temp = getChild(v);
                List<double> tong = new List<double>();
                tong.Add(this.d[v]);
                tong.Add(0);
                foreach (int i in temp)
                {
                    if (!coDTDHoacMC(i))
                    {
                        List<double> temp1 = tongDCayCon(i, u);
                        if (temp1[1] == 1)
                        {
                            kq[0] = tong[0] + temp1[0];
                            kq[1] = 1;
                            return kq;
                        }
                        else if (temp1[1] == 0)
                        {
                            tong[0] = tong[0] + temp1[0];
                        }
                    }
                }
                tong[0] = tong[0] + d[v];
                return tong;
            }
        }
        private double currentD(int v)
        {
            List<double> tong;
            tong = tongDCayCon(getSuperParent(v), v);
            return tong[0];
        }
        private bool In_int(int v, List<int> l)
        {
            foreach (int x in l)
            {
                if (v == x)
                {
                    return true;
                }
            }
            return false;
        }
        private bool denBao(int v, List<int> l)
        {
            if (db[v] && !In_int(v, l))
            {
                return true;
            }
            return false;
        }
        private List<int> getParentList(int v)
        {
            List<int> temp = new List<int>();
            temp.Add(v);
            if (coDTDHoacMC(v))
            {
                return temp;
            }
            else
            {
                List<int> parent_list = getParentList(getParent(v));
                foreach (int i in parent_list)
                {
                    temp.Add(i);
                }
                return temp;
            }
        }
        private int getBrother(int v)
        {
            for (int i = 0; i < numNode; i++)
            {
                if (i != v && this.parent[i] == this.parent[v])
                {
                    return i;
                }
            }
            return -1;
        }
        private List<double> timDuong(int v, List<int> l)
        {
            List<int> temp = getChild(v);
            List<double> tong = new List<double>();
            tong.Add(d[v]);
            tong.Add(0);
            if (v == l[0])
            {
                tong[1] = 1;
                return tong;
            }
            if (denBao(v, l))
            {
                tong[0] = 0;
                tong[1] = 0;
                return tong;
            }
            if (getBrother(v) != -1)
            {
                int br = getBrother(v);
                if (this.db[br] && In_int(getBrother(v), l))
                {
                    tong[0] = 0;
                    tong[1] = 0;
                    return tong;
                }
            }
            if (temp.Count == 0)
            {
                tong[0] = 2 * this.d[v];
                tong[1] = 0;
                return tong;
            }
            foreach (int i in temp)
            {
                if (!coDTDHoacMC(i))
                {
                    List<double> temp1 = timDuong(i, l);
                    if (temp1[1] == 1)
                    {
                        tong[0] = tong[0] + temp1[0];
                        tong[1] = 1;
                        return tong;
                    }
                    else if (temp1[1] == 0)
                    {
                        tong[0] = tong[0] + temp1[0];
                    }
                }
            }
            tong[0] = tong[0] + this.d[v];
            return tong;
        }
        private double currentD1(int v)
        {
            List<double> temp = timDuong(getSuperParent(v), getParentList(v));
            return temp[0];
        }
        private double tongWCayCon(int v)
        {
            double tong = this.w[v];
            List<int> child1 = getChild(v);
            if (child1.Count == 0)
            {
                return tong;
            }
            foreach (int i in child1)
            {
                tong += tongWCayCon(i);
            }
            return tong;
        }
        private double currentW(int v)
        {
            int temp = getSuperParent(v);
            return tongWCayCon(temp);
        }
        private double currentWr(int v)
        {
            int temp = getMCParent(v);
            return tongWCayCon(temp);
        }
        public int currentM(int v)
        {
            int rank = 0;
            if (!this.mc[v])
            {

                if (this.dtd[v])
                {
                    rank += 1;
                }
                rank += currentM(getParent(v));
            }
            return rank;
        }
        private double currentF()
        {
            double F = 0;
            for (int i = 0; i < this.numNode; i++)
            {
                F += this.l[i] * currentD(i) * currentW(i) * tieuHao / vanToc; // Wi = Di * tieuHao, Ti = Di / vanToc, công thức: Li * Ti * Wi (currentW(i) đang bằng Di)
            }
            return F;
        }
        private double currentF1()
        {
            double F = 0;
            for (int i = 0; i < this.numNode; i++)
            {
                F += this.l[i] * currentD1(i) * currentW(i) * tieuHao / vanToc;// Li * Ti * Wi, Di // vanToc = Ti, Wi * tieuHao = Wi_thực;
            }
            return F;
        }
        private double currentF2()
        {
            double F = 0;
            for (int i = 0; i < this.numNode; i++)
            {
                F += this.l[i] * Tr * currentWr(i) * tieuHao; // * tiêu hao
            }
            return F;
        }
        private double currentF3()
        {
            double F = 0;
            for (int i = 0; i < this.numNode; i++)
            {
                F += this.l[i] * Cr * currentM(i);
            }
            return F;
        }
        private double currentF4()
        {
            double F = 0;
            for (int i = 0; i < this.numNode; i++)
            {
                F += this.l[i] * Cs * this.currentM(i);
            }
            return F;
        }
        public double sumF1()
        {
            return Ce * currentF() + Ce * currentF2() + currentF3() + currentF4();
        }
        public double sumF2()
        {
            return Ce * currentF1() + Ce * currentF2() + currentF3() + currentF4();
        }
        public void setMC(int v)
        {
            this.mc[v] = true;
        }
        public void setDB(int v)
        {
            this.db[v] = true;
        }
        public void setDTD(int v)
        {
            this.dtd[v] = true;
        }
        public bool getMC(int v)
        {
            return this.mc[v];
        }
        public bool getDB(int v)
        {
            return this.db[v];
        }
        public bool getDTD(int v)
        {
            return this.dtd[v];
        }
        public void setLastMC(int v)
        {
            this.last_mc[v] = true;
        }
        public void setLastDB(int v)
        {
            this.last_db[v] = true;
        }
        public void setLastDTD(int v)
        {
            this.last_dtd[v] = true;
        }
        public void refresh()
        {
            this.mc.Clear();
            this.db.Clear();
            this.dtd.Clear();
            for (int i = 0; i < this.last_mc.Count; i++)
            {
                this.mc.Add(this.last_mc[i]);
                this.db.Add(this.last_db[i]);
                this.dtd.Add(this.last_dtd[i]);
            }
        }
    }
}
