using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class TestPitanjeServis : ITestPitanje
    {
        public TestPitanjeServis()
        {

        }
        public bool Dodaj(int pitanje, int test)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Sastoji s = null;
                s = db.Sastojis.FirstOrDefault(x => x.PitanjeIdPitanja == pitanje && x.TestIdTesta == test);
                if (s != null) return false;

                s = new Sastoji();
                s.PitanjeIdPitanja = pitanje;
                s.TestIdTesta = test;
                db.Sastojis.Add(s);
                db.SaveChanges();
                return true;
            }
        }

        public bool Obrisi(int pitanje, int test)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Sastoji s = null;
                s = db.Sastojis.FirstOrDefault(x => x.PitanjeIdPitanja == pitanje && x.TestIdTesta == test);
                if (s == null) return false;

                db.Sastojis.Remove(s);
                db.SaveChanges();
                return true;
            }
        }

        public List<Sastoji> UcitajSvaPitanjaITestove()
        {
            using(var db= new BazePodataka2ModelContainer())
            {
                return db.Sastojis.ToList();
            }
        }

        public List<Sastoji> VratiPitanjaZaTest(int test)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Sastojis.Where(x => x.TestIdTesta == test).ToList();
            }
        }

        public List<Sastoji> VratiTestoveZaPitanje(int pitanje)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Sastojis.Where(x => x.PitanjeIdPitanja == pitanje).ToList();
            }
        }
    }
}
