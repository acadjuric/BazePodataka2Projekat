using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class PitanjeServis : IPitanje
    {
        public PitanjeServis()
        {

        }

        

        public bool DodajPitanje(string tekst, int nivo, int bodovi, List<Odgovor> odgovori, List<Test> testovi)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Pitanje p = new Pitanje()
                {
                    IdPitanja= IzgenerisiID(),
                    Tekst = tekst,
                    Nivo = nivo,
                    Bodovi = bodovi,

                };
                db.Pitanja.Add(p);
                db.SaveChanges();

                MapirajUTabeluPosedujePitanjeNaOdgovor(p, odgovori);
                MapirajUTabeluSastojiPitanjeNaTest(p, testovi);

                return true;
            }
        }

        public bool IzmeniPitanje(int id, string tekst, int nivo, int bodovi, List<Odgovor> odgovori, List<Test> testovi)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Pitanje p = null;
                p = db.Pitanja.FirstOrDefault(x => x.IdPitanja == id);
                if (p == null) return false;

                p.Tekst = tekst;
                p.Nivo = nivo;
                p.Bodovi = bodovi;
               
                db.SaveChanges();

                MapirajUTabeluPosedujePitanjeNaOdgovor(p, odgovori);
                MapirajUTabeluSastojiPitanjeNaTest(p, testovi);

                return true;
            }
        }

        public bool ObrisiPitanje(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Pitanje p = null;
                p = db.Pitanja.FirstOrDefault(x => x.IdPitanja == id);
                if (p == null) return false;

                PitanjeOdgovorServis pitanjeOdgovorServis = new PitanjeOdgovorServis();
                var listaOdgovora = pitanjeOdgovorServis.VratiOdgovoreZaPitanje(id);
                if (listaOdgovora != null)
                {
                    foreach (var item in listaOdgovora)
                    {
                        pitanjeOdgovorServis.Obrisi(id, item.OdgovorIdOdgovora);
                    }
                }

                TestPitanjeServis testPitanjeServis = new TestPitanjeServis();
                var listaTestova = testPitanjeServis.VratiTestoveZaPitanje(id);
                if (listaTestova != null)
                {
                    listaTestova.ForEach(item => testPitanjeServis.Obrisi(id, item.TestIdTesta));
                }

                db.Pitanja.Remove(p);
                db.SaveChanges();
                return true;
            }
        }

        public List<Pitanje> UcitajSvaPitanja()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Pitanja.ToList();
            }
        }


        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Pitanje k = null;
                k = db.Pitanja.FirstOrDefault(x => x.IdPitanja == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Pitanja.FirstOrDefault(x => x.IdPitanja == id); ;
                }

                return id;
            }
        }

        public Pitanje VratiPitanje(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Pitanje k = null;
                k = db.Pitanja.FirstOrDefault(x => x.IdPitanja == id);
                return k;

            }
        }

        private void MapirajUTabeluPosedujePitanjeNaOdgovor(Pitanje p, List<Odgovor> odgovori)
        {
            if (odgovori.Count != 0)
            {
                PitanjeOdgovorServis servis = new PitanjeOdgovorServis();
                List<Poseduje> sviOdgovoriZaPitanje = servis.VratiOdgovoreZaPitanje(p.IdPitanja);
                foreach (var poseduje in sviOdgovoriZaPitanje)
                {
                    //Provera da li postoje u bazi neke koje su bile cekirane na ui
                    // pa ih je korisnik prilikom izmene uklonio -> treba da ih uklonim iz baze
                    if (!odgovori.Exists(x => x.IdOdgovora == poseduje.OdgovorIdOdgovora))
                    {
                        servis.Obrisi(p.IdPitanja, poseduje.OdgovorIdOdgovora);
                    }
                }

                foreach (var odgovor in odgovori)
                {
                    //u dodaj imas proveru da li vec postoji, tako da je obezbedjeno da ce biti svaki par jedinstven
                    servis.Dodaj(p.IdPitanja, odgovor.IdOdgovora);
                }
            }
        }

        private void MapirajUTabeluSastojiPitanjeNaTest(Pitanje p, List<Test> testovi)
        {
            if (testovi.Count != 0)
            {
                TestPitanjeServis servis = new TestPitanjeServis();
                List<Sastoji> sviTestoviZaPitanje = servis.VratiTestoveZaPitanje(p.IdPitanja);
                foreach (var sastoji in sviTestoviZaPitanje)
                {
                    //Provera da li postoje u bazi neke koje su bile cekirane na ui
                    // pa ih je korisnik prilikom izmene uklonio -> treba da ih uklonim iz baze
                    if (!testovi.Exists(x => x.IdTesta == sastoji.TestIdTesta))
                    {
                        servis.Obrisi(p.IdPitanja, sastoji.TestIdTesta);
                    }
                }

                foreach (var test in testovi)
                {
                    //u dodaj imas proveru da li vec postoji, tako da je obezbedjeno da ce biti svaki par jedinstven
                    servis.Dodaj(p.IdPitanja, test.IdTesta);
                }
            }
        }
    }
}
