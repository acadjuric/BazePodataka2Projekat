using Servis.Interfejsi;
using Servis.Pomocne_Klase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class ProcedureServis:IProcedure
    {
        public ProcedureServis()
        {

        }

        public List<ProcImePrezimeZvanjeNastavnikaZaKurs> ProcImePrezimeNastavnikaZaKurs(int idKurs)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Database.SqlQuery<ProcImePrezimeZvanjeNastavnikaZaKurs>("p_ImePrezimeZvanjeNastavnikaZaKurs @kursId",
                 new SqlParameter() { ParameterName = "kursId", Value = idKurs, }).ToList(); ;
            }
        }

        public List<ProcNastavnikoveNastavneTeme> ProcNastavnikoveNastavneTeme(int idNastavnika)
        {
            using(var db  = new BazePodataka2ModelContainer())
            {
                return db.Database.SqlQuery<ProcNastavnikoveNastavneTeme>("p_NastavnikoveNastavneTeme @nastavnikId",
                    new SqlParameter() { ParameterName = "nastavnikId", Value = idNastavnika }).ToList();
            }
        }

        public List<ProcUcenikVidiPitanjaZaTest> ProcUcenikVidiPitanjaZaTest(int idKursa, int idTesta)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                //ova ima cursor
                SqlParameter kurs = new SqlParameter() { ParameterName = "kursId", Value = idKursa };
                SqlParameter test = new SqlParameter() { ParameterName = "testId", Value = idTesta };
                SqlParameter[] parameters = { kurs, test };
                return db.Database.SqlQuery<ProcUcenikVidiPitanjaZaTest>("p_UcenikVidiPitanjaZaTest @kursId, @testId",
                    parameters).ToList();
            }
        }
    }
}
