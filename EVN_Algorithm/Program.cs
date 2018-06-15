using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Newtonsoft.Json;

namespace EVN
{


    // tìm vị trí đặt của cả mạng

    class Program
    {
        public const string ROOT_FOLDER = @"D:\evn.git\EVN_data";
        public const string FOLDER_RESULT = @"\result";
        public const string FILE_LOG_ALGORITHM = FOLDER_RESULT+@"\running";
        public const string FILE_RESULT_ALGORITHM = FOLDER_RESULT+@"\result";
        public class FILE_DATA {
            public const string SO_HIEU = @"\soHieu.txt";
            public const string MA_DOI_TUONG = @"\maDoiTuong.txt";
            public const string LO_CAP_DIEN = @"\loCapDien.txt";
            public const string MA_LIEN_KET = @"\maLienKet.txt";
            public const string TOA_DO_LIEN_KET = @"\toaDoXDaiDien.txt";
            public const string TOA_DO_DAI_DIEN = @"\toaDoYDaiDien.txt";
            public const string SO_THU_TU = @"\soThuTu.txt";
            public static void CheckFile(string rootFolder) {
                if (!File.Exists(rootFolder + SO_HIEU))
                {
                    Environment.Exit(0);
                }
                if (!File.Exists(rootFolder + MA_DOI_TUONG))
                {
                    Environment.Exit(0);
                }
                if (!File.Exists(rootFolder + LO_CAP_DIEN))
                {
                    Environment.Exit(0);
                }
                if (!File.Exists(rootFolder + MA_LIEN_KET))
                {
                    Environment.Exit(0);
                }
                if (!File.Exists(rootFolder + TOA_DO_LIEN_KET))
                {
                    Environment.Exit(0);
                }
                if (!File.Exists(rootFolder + TOA_DO_DAI_DIEN))
                {
                    Environment.Exit(0);
                }
                if (!File.Exists(rootFolder + SO_THU_TU))
                {
                    Environment.Exit(0);
                }
                
            }
        }
        
        static void Main(string[] args)
        {


            List<string> maDoiTuong = new List<string>();
            List<string> maLienKet = new List<string>();
            List<ToaDo> toaDoDaiDien = new List<ToaDo>();
            List<string> soThuTu = new List<string>();
            List<string> soHieu = new List<string>();
            List<string> loCapDien = new List<string>();
            List<List<double>> toaDo = new List<List<double>>();
            String rootFolder = ROOT_FOLDER;
             DateTime start = DateTime.Now;
            
            int nrMayCat=10, nrDen=0, nrDaoTuDong=10;
            for (int i = 0; i < args.Length; i++) {
                if (args[i].Contains("-nrMayCat")) {
                    if (args.Length > (i + 1)) {
                        Int32.TryParse(args[i+1],out nrMayCat);
                    }
                }else if (args[i].Contains("-nrDao"))
                {
                    if (args.Length > (i + 1))
                    {
                        Int32.TryParse(args[i + 1], out nrDaoTuDong);
                    }
                }
                else if (args[i].Contains("-nrDen"))
                {
                    if (args.Length > (i + 1))
                    {
                        Int32.TryParse(args[i + 1], out nrDen);
                    }
                    
                }
                else if (args[i].Contains("-folderData"))
                {
                    if (args.Length > (i + 1))
                    {
                        rootFolder = args[i + 1];
                    }

                }
                
            }
            if (!Directory.Exists(rootFolder)) {
                return;
            }
            if (!Directory.Exists(rootFolder+FOLDER_RESULT))
            {
                Directory.CreateDirectory(rootFolder+FOLDER_RESULT);
            }
            Program.fileLog = rootFolder + FILE_LOG_ALGORITHM + "_" + nrMayCat + "_" + nrDaoTuDong + "_" + nrDen + ".log";
            if (File.Exists(fileLog))
            {
                return;
            }
            Log("\nRunning with nrMayCat:" + nrMayCat + "---nrDaoTuDong:" + nrDaoTuDong + "--nrDen:" + nrDen + " --folderData:" + rootFolder);
            FILE_DATA.CheckFile(rootFolder);
            String fileResult= rootFolder+FILE_RESULT_ALGORITHM + "_" + nrMayCat + "_" + nrDaoTuDong + "_" + nrDen + ".txt";
            using (StreamReader sr1 = new StreamReader(rootFolder +FILE_DATA.SO_HIEU ))
            {
                string line;

                while ((line = sr1.ReadLine()) != null)
                {
                    soHieu.Add(line.Trim());
                }
            }
            using (StreamReader sr = new StreamReader(rootFolder + FILE_DATA.MA_DOI_TUONG))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    maDoiTuong.Add(line.Trim());
                }
            }
            using (StreamReader sr = new StreamReader(rootFolder +FILE_DATA.LO_CAP_DIEN))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    loCapDien.Add(line.Trim());
                }
            }
            using (StreamReader sr = new StreamReader(rootFolder + FILE_DATA.MA_LIEN_KET))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    maLienKet.Add(line.Trim());
                }
            }
            using (StreamReader sr = new StreamReader(rootFolder +FILE_DATA.TOA_DO_LIEN_KET))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    List<double> temp = new List<double>();
                    temp.Add(double.Parse(line.Trim()));
                    toaDo.Add(temp);
                }
            }
            using (StreamReader sr = new StreamReader(rootFolder +FILE_DATA.TOA_DO_DAI_DIEN))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    toaDo[i].Add(double.Parse(line.Trim()));
                    i++;
                }
            }
            using (StreamReader sr = new StreamReader(rootFolder +FILE_DATA.SO_THU_TU))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    soThuTu.Add(line.Trim());
                }
            }
            for (int i = 0; i < maDoiTuong.Count; i++)
            {
                //Log(toaDo[i][0].ToString() + "|" + toaDo[i][1].ToString());
                toaDoDaiDien.Add(new ToaDo(toaDo[i][0], toaDo[i][1]));
            }
            Log("\nDoc xong du lieu");
            ViTriDat v = new ViTriDat(maDoiTuong, maLienKet, toaDoDaiDien, soThuTu, soHieu, loCapDien, nrMayCat, nrDaoTuDong, nrDen);
            Log("\nChay xong");

            List<string> may_cat1 = v.layViTriDat(TYPE_OBJECT.MAY_CAT);
            List<string> dao_tu_dong1 = v.layViTriDat(TYPE_OBJECT.DAO_TU_DONG);
            List<string> den_bao1 = v.layViTriDat(TYPE_OBJECT.DEN_BAO);

            TimeSpan time = DateTime.Now - start;
            String teim = String.Format("{0}.{1}", time.Seconds, time.Milliseconds.ToString().PadLeft(3, '0'));
            Log("Total time:" + teim);
            ResultAlgorithm resultAlgorithm = new ResultAlgorithm() {
                total_time = time.TotalMilliseconds,
                param_nrDaoTuDong = nrDaoTuDong,
                param_nrDenBao = nrDen,
                param_nrMayCat = nrMayCat,
                dao_tu_dong = dao_tu_dong1,
                may_cat = may_cat1,
                den_bao = den_bao1,
                run_at =start };
            System.IO.File.WriteAllText(fileResult, JsonConvert.SerializeObject(resultAlgorithm));

            

        }
        private static string fileLog = "";
        public static void Log(string mes)
        {
            System.IO.File.AppendAllText(fileLog, "\n " + mes);
        }
    }
}

