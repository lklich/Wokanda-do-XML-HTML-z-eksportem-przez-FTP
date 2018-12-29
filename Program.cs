using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Net;


namespace LKWokanda
{
    class Program
    {
        static void Main(string[] args)
        {
          if (args.Length == 0)
          {
                wykasuj_stary_plik();
                Console.WriteLine("LK Wokanda - wersja beta. Kompilacja 28-08-2015. autor: Leszek Klich");
                Console.WriteLine("Przełącznik /k - konfiguracja programu");
                Console.WriteLine("====================================================================");
                loguj("Uruchomienie programu");
                zapis_do_xml("<?xml version=\"1.0\" encoding=\"windows-1250\" ?>");
                zapis_do_xml("<Rozprawy>");

                if (Properties.Settings.Default.do_html == true)  // Czy generować HTML?
                {
                    zapis_do_html("<html><head><META http-equiv=Content-Type content=\"text/html; charset=utf8\"><title>E-Wokanda</title><style type=\"text/css\">");
                    zapis_do_html("table.stats { text-align: center; font-family: Verdana, Geneva, Arial, Helvetica, sans-serif ; font-weight: normal; font-size: 11px;");
                    zapis_do_html("color: #fff; width: 500px; background-color: #666; border: 0px; border-collapse: collapse; border-spacing: 0px;}");
                    zapis_do_html("table.stats td {background-color: #CCC; color: #000; padding: 4px; text-align: left; border: 1px #fff solid;}");
                    zapis_do_html("table.stats td.hed {background-color: #666; color: #fff; padding: 4px; text-align: center; border-bottom: 2px #fff solid; font-size: 10px; } ");
                    zapis_do_html("</style></head><body>");
                }

                if (Properties.Settings.Default.czycywilny == true) { loguj("Generowanie Cywilny"); poczatek_tabeli_HTML();  Generuj(1, 0); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czykarny == true) { loguj("Generowanie Karny"); poczatek_tabeli_HTML(); Generuj(2, 0); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czyrodzinny == true) { loguj("Generowanie Rodzinny"); poczatek_tabeli_HTML(); Generuj(3, 0); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czypracy == true) { loguj("Generowanie Pracy"); poczatek_tabeli_HTML(); Generuj(4, 0); koniec_tabeli_HTML(); }

                if (Properties.Settings.Default.czy_cywilnyII == true) { loguj("Generowanie Cywilny II"); poczatek_tabeli_HTML(); Generuj(5, 2); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czy_rodzinnyII == true) { loguj("Generowanie Rodzinny II"); poczatek_tabeli_HTML(); Generuj(6, 2); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czy_karnyII == true) { loguj("Generowanie Karny II"); poczatek_tabeli_HTML(); Generuj(7, 2); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czy_pracyII == true) { loguj("Generowanie Pracy II"); poczatek_tabeli_HTML(); Generuj(8, 2); koniec_tabeli_HTML(); }
                if (Properties.Settings.Default.czy_penitencjarny == true) { loguj("Generowanie Penitencjarny"); poczatek_tabeli_HTML(); Generuj(9, 0); koniec_tabeli_HTML(); }
                    zapis_do_xml("</Rozprawy>"); // Zakończenie XMLa
                    if(Properties.Settings.Default.do_html==true) zapis_do_html("</script></body></html>");
                    //if (Properties.Settings.Default.do_html == true) zapis_do_html("</table></script></body></html>");
                if (Properties.Settings.Default.send_ftp == true) Send_FTP();
          }
          if (args.Length == 1)
          {
              Console.WriteLine("Wywołanie programu konfiguracyjnego...");
           if(args[0]=="/k") 
            {
                loguj("Uruchomienie konfiguracji programu.");
             konfiguracja konfig = new konfiguracja();
             konfig.ShowDialog();
          }
            

          }
        }

        static void poczatek_tabeli_HTML()
        {
            if (Properties.Settings.Default.do_html == true)
            {
                zapis_do_html("<table class='stats' cellspacing='0' border='1'>");
                zapis_do_html("<tr><td class='hed' colspan='4'><strong>Sygnatura</strong></td><td class='hed' colspan='4'><strong>Data i godzina</strong></td><td class='hed' colspan='4'><strong>Sala</strong></td><td class='hed' colspan='4'><strong>Wydział</strong>");
            }
        }

        static void koniec_tabeli_HTML()
        {
            if (Properties.Settings.Default.do_html == true)
            {
                zapis_do_html("</table><br>");
            }
        }

        static void wykasuj_stary_plik()
          {
         if (File.Exists(Properties.Settings.Default.plikXML))
              {
                  File.Delete(Properties.Settings.Default.plikXML);
                  File.Delete("iwokanda.html");
              }
         if (File.Exists("iwokanda.html"))
         {
             File.Delete("iwokanda.html");
         }
          }

        static void Generuj(int wydzial, int inst)
        {
            string instancja = "";
            if (inst == 0) instancja = "";
            if (inst == 2) instancja = " II inst.";
            if (inst == 3) instancja = " III inst.";
            try
            {
                string polacz = "Data Source=db;Initial Catalog=xxx;Integrated Security=True";
                if (wydzial == 1) polacz = Properties.Settings.Default.cywilny;
                if (wydzial == 2) polacz = Properties.Settings.Default.Karny;
                if (wydzial == 3) polacz = Properties.Settings.Default.Rodzinny;
                if (wydzial == 4) polacz = Properties.Settings.Default.Pracy;
                if (wydzial == 5) polacz = Properties.Settings.Default.cywilnyII;
                if (wydzial == 6) polacz = Properties.Settings.Default.rodzinnyII;
                if (wydzial == 7) polacz = Properties.Settings.Default.karnyII;
                if (wydzial == 8) polacz = Properties.Settings.Default.pracyII;
                if (wydzial == 9) polacz = Properties.Settings.Default.penitencjarny;

                var connectionString = polacz;
                SqlConnection polaczenie = new SqlConnection(connectionString);
                polaczenie.Open();
                SqlCommand komendaSQL = polaczenie.CreateCommand();

                komendaSQL.CommandText = "SELECT * FROM konfig WHERE czyus=0";
                SqlDataReader czytnik = komendaSQL.ExecuteReader();
                czytnik.Read();
                string symbol1 = czytnik["oznaczenie"].ToString();
                string wydzial1 = czytnik["wydzial"].ToString();
                string sad1 = czytnik["sad"].ToString();
                string temp1 = "";

                komendaSQL.CommandText = "SELECT RTRIM(termin.sala) as sala, convert(char(5), rozprawa.d_rozprawy, 108) [time], RTRIM(repertorium.symbol) as symbol, " +
                "sprawa.numer, sprawa.rok, termin.d_posiedz, RTRIM(ISNULL(dane_strony.nazwisko, '')) + ' '+RTRIM(ISNULL(dane_strony.imie,'')) AS nazwstr, strona.ident as id_strony, db_name() as baza " +
                "FROM repertorium INNER JOIN dane_strony INNER JOIN strona ON dane_strony.ident = strona.id_danych INNER JOIN sprawa ON strona.id_sprawy = sprawa.ident INNER JOIN rozprawa INNER JOIN termin ON " +
                "rozprawa.id_terminu = termin.ident ON sprawa.ident = rozprawa.id_sprawy ON repertorium.numer = sprawa.repertorium WHERE (strona.czyus = 0) AND (LEN(termin.sala) > 0) AND (repertorium.rodzaj = 0) AND (rozprawa.czyus = 0) AND " +
                "(termin.d_posiedz >= cast(convert(varchar(8),getdate(),1) as datetime)) ORDER BY rozprawa.d_rozprawy, termin.sala, sprawa.rok, sprawa.numer, dane_strony.nazwisko";
                czytnik.Close();
                czytnik = komendaSQL.ExecuteReader();

                while (czytnik.Read())
                {
                    if (temp1 != "<Sygnatura>" + symbol1.Trim() + " " + czytnik["symbol"].ToString() + " " + czytnik["numer"].ToString() + "/" + czytnik["rok"].ToString().Substring(2, 2) + "</Sygnatura>")
                    { 
                        zapis_do_xml("<Rozprawa>");
                        zapis_do_xml("<Sygnatura>" + symbol1.Trim()+" " + czytnik["symbol"].ToString() + " " + czytnik["numer"].ToString() + "/" + czytnik["rok"].ToString().Substring(2,2) + "</Sygnatura>");
                        zapis_do_xml("<Data>" + string.Format("{0:yyyy-MM-dd}", czytnik["d_posiedz"]) + "</Data>");
                        zapis_do_xml("<Strony> </Strony>");
                        zapis_do_xml("<Sala>" + czytnik["sala"].ToString() + "</Sala>");
                        zapis_do_xml("<Wydzial>" + symbol1.Trim() +" "+wydzial1.Trim() +instancja+ "</Wydzial>");
                        zapis_do_xml("<Godzina>" + string.Format("{0:yyyy-MM-dd}", czytnik["d_posiedz"])+" "+czytnik["time"] + "</Godzina>");
                        zapis_do_xml("<SygnaturaProkuratury> </SygnaturaProkuratury>");
                        zapis_do_xml("</Rozprawa>");
                        zapis_new_line();
                        //Zapis do html
                        if (Properties.Settings.Default.do_html == true)
                        {
                            zapis_do_html("<tr><td class=\"hed\" colspan=\"4\">"+symbol1.Trim()+" " + czytnik["symbol"].ToString() + " " + czytnik["numer"].ToString() + "/" + czytnik["rok"].ToString().Substring(2,2)+"</td>");
                            zapis_do_html("<td class=\"hed\" colspan=\"4\">"+string.Format("{0:yyyy-MM-dd}", czytnik["d_posiedz"])+"</td>");
                            zapis_do_html("<td class=\"hed\" colspan=\"4\">"+czytnik["sala"].ToString()+"</td>");
                            zapis_do_html("<td class=\"hed\" colspan=\"4\">"+symbol1.Trim()+" "+wydzial1.Trim() + " "+instancja+"</td></tr>");
                        }                        
                    }
                    else temp1 = "";
                    //temp1 = "<Sygnatura>" + czytnik["symbol"].ToString() + " " + czytnik["numer"].ToString() + "/" + czytnik["rok"].ToString().Substring(2, 2) + "</Sygnatura>";
                     temp1 = "<Sygnatura>" + symbol1.Trim() + " " + czytnik["symbol"].ToString() + " " + czytnik["numer"].ToString() + "/" + czytnik["rok"].ToString().Substring(2, 2) + "</Sygnatura>";
                }
                czytnik.Close();
                polaczenie.Close();
            }
            catch (SqlException e)
            {
                loguj("Wystąpił błąd: " + e.Message);
            }
        }


        private static String getUnicodeValue(string string2Encode) //
{
    Encoding srcEncoding = Encoding.GetEncoding("Windows-1250");
    UnicodeEncoding dstEncoding = new UnicodeEncoding();
    byte[] srcBytes =  srcEncoding.GetBytes(string2Encode);
    byte[] dstBytes = dstEncoding.GetBytes(string2Encode);
    return dstEncoding.GetString(dstBytes);
}


        static void zapis_do_html(string co)
        {
            Encoding u8 = Encoding.Default;
            string co2 = getUnicodeValue(co);
            string path = "iwokanda.html";
            if (!File.Exists(path))
            {
                FileStream f = new FileStream(path, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(f, Encoding.GetEncoding("windows-1250"));

                {
                    streamWriter.Write(co2 + Environment.NewLine);
                    streamWriter.Close();
                    f.Close();
                }
            }
            else
            {
                FileStream f = new FileStream(path, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(f, Encoding.GetEncoding("windows-1250"));
                {
                    streamWriter.Write(co+Environment.NewLine);
                    streamWriter.Close();
                    f.Close();
                }
            }

        }


        static void zapis_do_xml(string co)
        {
            Encoding u8 = Encoding.Default;

                string path = Properties.Settings.Default.plikXML;
                if (!File.Exists(path))
                {
                    //using (StreamWriter sw = File.CreateText(path))
                    FileStream f=new FileStream(path,FileMode.Create);
                    StreamWriter streamWriter = new StreamWriter(f, Encoding.GetEncoding("windows-1250"));                     
                    {
                        streamWriter.Write(co);
                        streamWriter.Close();
                        f.Close();
                    }
                }
                else
                {
                    //using (StreamWriter sw = File.AppendText(path))
                    FileStream f = new FileStream(path, FileMode.Append);
                    StreamWriter streamWriter = new StreamWriter(f, Encoding.GetEncoding("windows-1250"));                      
                    {
                        streamWriter.Write(co);
                        streamWriter.Close();
                        f.Close();
                    }
            }
             
        }

        static void zapis_new_line()
        {
            string path = Properties.Settings.Default.plikXML;
            if (!File.Exists(path))
            {
                //using (StreamWriter sw = File.CreateText(path))
                FileStream f = new FileStream(path, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(f, Encoding.GetEncoding("windows-1250"));  

                {
                    streamWriter.Write(Environment.NewLine);
                    streamWriter.Close();
                    f.Close();
                }
            }
            else
            {
                //using (StreamWriter sw = File.AppendText(path))
                FileStream f = new FileStream(path, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(f, Encoding.GetEncoding("windows-1250"));  
                {
                    streamWriter.Write(Environment.NewLine);
                    streamWriter.Close();
                    f.Close();
                }
            }
        }

        static void Send_FTP()
        {
            try
            {
                loguj("Wysyłanie pliku na FTP");
                string serwer, user, haslo, katalog, plik = "";
                serwer = Properties.Settings.Default.ftp;
                user = Properties.Settings.Default.user;
                haslo = Properties.Settings.Default.password;
                katalog = Properties.Settings.Default.ftp_katalog;

              plik = Properties.Settings.Default.plikXML; 
                UploadFile(plik, "ftp://" + serwer + "/" + katalog + "/" + plik, user, haslo);
              plik = Properties.Settings.Default.plikHTML;
                UploadFile(plik, "ftp://" + serwer + "/" + katalog + "/" + plik, user, haslo);
            }
            catch (SqlException e)
            {
                loguj("Wystąpił błąd FTP: " + e.Message);
            }
       }

      static void UploadFile(string _FileName, string _UploadPath, string _FTPUser, string _FTPPass)
	{
	    System.IO.FileInfo _FileInfo = new System.IO.FileInfo(_FileName);
	    System.Net.FtpWebRequest _FtpWebRequest = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(new Uri(_UploadPath));
	    _FtpWebRequest.Credentials = new System.Net.NetworkCredential(_FTPUser, _FTPPass);
	    _FtpWebRequest.KeepAlive = false;
	    _FtpWebRequest.Timeout = 20000;
	    _FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
	    _FtpWebRequest.UseBinary = true;
	    _FtpWebRequest.ContentLength = _FileInfo.Length;
	    int buffLength = 2048;
	    byte[] buff = new byte[buffLength];
	    System.IO.FileStream _FileStream = _FileInfo.OpenRead();
	 
	    try
	    {
	        System.IO.Stream _Stream = _FtpWebRequest.GetRequestStream();
	        int contentLen = _FileStream.Read(buff, 0, buffLength);
	        while (contentLen != 0)
	        {
	            _Stream.Write(buff, 0, contentLen);
	            contentLen = _FileStream.Read(buff, 0, buffLength);
	        }
	        _Stream.Close();
	        _Stream.Dispose();
	        _FileStream.Close();
	        _FileStream.Dispose();
	    }
	    catch (Exception ex)
	    {
         loguj("Błąd FTP: "+ ex.Message.ToString());
	    }
	}

        static void loguj(string co)
        {
            string czas = DateTime.Now.ToString();
            if (Properties.Settings.Default.debug == true)
            {
                string path = @"log.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        Console.WriteLine(czas+" : "+co);
                        sw.WriteLine(czas + " : " + co);
                    }
                }
                else
                {
                using (StreamWriter sw = File.AppendText(path))
                    {
                        Console.WriteLine(czas + " : " + co);
                        sw.WriteLine(czas + " : " + co);
                    }
                }
            }
        }

 
    }
}

