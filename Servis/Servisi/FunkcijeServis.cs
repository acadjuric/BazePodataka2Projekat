using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class FunkcijeServis:IFunkcije
    {
        public FunkcijeServis()
        {

        }

        public int FuncBrojSvihObavestenjaZaKorisnika(int korisnikId)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                SqlParameter rez = new SqlParameter() { ParameterName = "rezultat", Direction = System.Data.ParameterDirection.Output, SqlDbType = System.Data.SqlDbType.Int };
                SqlParameter korisnik = new SqlParameter() { ParameterName = "korisnikId", Value = korisnikId, SqlDbType = System.Data.SqlDbType.Int };
                SqlParameter[] parameters = { korisnik, rez };

                db.Database.ExecuteSqlCommand("exec @rezultat = f_BrojSvihObavestenjaZaKorisnika @korisnikId", parameters);

                return (int)rez.Value;
            }

        }

        public int FuncBrojTestovaZaKurs(int kursId)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                SqlParameter rez = new SqlParameter() { ParameterName = "rezultat", Direction = System.Data.ParameterDirection.Output, SqlDbType = System.Data.SqlDbType.Int };
                SqlParameter kurs = new SqlParameter() { ParameterName = "kursId", Value = kursId, SqlDbType = System.Data.SqlDbType.Int };
                SqlParameter[] parameters = { kurs, rez };

                db.Database.ExecuteSqlCommand("exec @rezultat = f_BrojTestovaZaKurs @kursId", parameters);

                return (int)rez.Value;
            }
        }

        public int FuncBrojUcenikaKojiPolazuIzabraniKursITest(int kursId, int testId)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                SqlParameter rez = new SqlParameter() { ParameterName = "rezultat", Direction = System.Data.ParameterDirection.Output, SqlDbType = System.Data.SqlDbType.Int };
                SqlParameter kurs = new SqlParameter() { ParameterName = "kursId", Value = kursId, SqlDbType = System.Data.SqlDbType.Int };
                SqlParameter test = new SqlParameter() { ParameterName = "testId", Value = testId, SqlDbType = System.Data.SqlDbType.Int };

                SqlParameter[] parameters = { kurs, test, rez };

                db.Database.ExecuteSqlCommand("exec @rezultat = f_BrojUcenikaKojiPolazuIzabraniKursITest @kursId, @testId", parameters);

                return (int)rez.Value;
            }
        }
    }
}
