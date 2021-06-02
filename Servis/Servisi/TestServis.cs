using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class TestServis : ITest
    {
        public TestServis()
        {

        }

        public bool DodajTest(string naziv, DateTime datum, int bodovi, int kurs, List<Pitanje> pitanja)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Test t = new Test()
                {
                    IdTesta = IzgenerisiID(),
                    Naziv = naziv,
                    Datum = datum,
                    Bodovi = bodovi,
                    KursIdKursa = kurs,
                };
                db.Testovi.Add(t);
                db.SaveChanges();

                MapirajUTabeluSastojiTestNaPitanje(t, pitanja);

                return true;
            }
        }

        public bool IzmeniTest(int id, string naziv, DateTime datum, int bodovi, int kurs, List<Pitanje> pitanja)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Test t = null;
                t = db.Testovi.FirstOrDefault(x => x.IdTesta == id);
                if (t == null) return false;

                t.Naziv = naziv;
                t.Bodovi = bodovi;
                t.Datum = datum;
                t.KursIdKursa = kurs;

                db.SaveChanges();

                MapirajUTabeluSastojiTestNaPitanje(t, pitanja);

                return true;

            }
        }

        public bool ObrisiTest(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Test t = null;
                t = db.Testovi.FirstOrDefault(x => x.IdTesta == id);
                if (t == null) return false;

                TestPitanjeServis testPitanjeServis = new TestPitanjeServis();
                var listaPitanja = testPitanjeServis.VratiPitanjaZaTest(id);
                if(listaPitanja != null)
                {
                    listaPitanja.ForEach(item => testPitanjeServis.Obrisi(item.PitanjeIdPitanja, id));
                }

                PrijavljenTestServis prijavljenTestServis = new PrijavljenTestServis();
                var listaPrijavljenih = prijavljenTestServis.VratiPrijavljeneZaTest(id);
                if(listaPrijavljenih != null)
                {
                    listaPrijavljenih.ForEach(item => prijavljenTestServis.Obrisi(item.PrijavljenKursIdKursa, item.PrijavljenUcenikIdKorisnika, id));
                }

                db.Testovi.Remove(t);
                db.SaveChanges();
                return true;
            }
        }

        public List<Test> UcitajSveTestove()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Testovi.ToList();
            }
        }

        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Test k = null;
                k = db.Testovi.FirstOrDefault(x => x.IdTesta == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Testovi.FirstOrDefault(x => x.IdTesta == id); ;
                }

                return id;
            }
        }

        public Test VratiTest(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Test k = null;
                k = db.Testovi.FirstOrDefault(x => x.IdTesta == id);
                return k;

            }
        }

        private void MapirajUTabeluSastojiTestNaPitanje(Test t, List<Pitanje> pitanja)
        {
            if (pitanja.Count != 0)
            {
                TestPitanjeServis servis = new TestPitanjeServis();
                List<Sastoji> svaPitanjaZaTest = servis.VratiPitanjaZaTest(t.IdTesta);
                foreach (var sastoji in svaPitanjaZaTest)
                {
                    //Provera da li postoje u bazi neke koje su bile cekirane na ui
                    // pa ih je korisnik prilikom izmene uklonio -> treba da ih uklonim iz baze
                    if (!pitanja.Exists(x => x.IdPitanja == sastoji.PitanjeIdPitanja))
                    {
                        servis.Obrisi(sastoji.PitanjeIdPitanja, t.IdTesta);
                    }
                }

                foreach (var pitanje in pitanja)
                {
                    //u dodaj imas proveru da li vec postoji, tako da je obezbedjeno da ce biti svaki par jedinstven
                    servis.Dodaj(pitanje.IdPitanja, t.IdTesta);
                }
            }
        }

        public void ObrisiTestoveZaKurs(int idKursa)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                while(db.Testovi.Any(x=> x.KursIdKursa == idKursa))
                {
                    ObrisiTest(db.Testovi.First(x => x.KursIdKursa == idKursa).IdTesta);
                }
            }
        }
    }
}
