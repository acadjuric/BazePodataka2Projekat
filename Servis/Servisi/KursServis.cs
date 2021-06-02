using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class KursServis : IKurs
    {

        public KursServis()
        {

        }

        public bool DodajKurs(string naziv, int poeni, int nastavnik, List<NastavnaTema> nastavneTeme)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Kurs k = new Kurs()
                {
                    IdKursa = IzgenerisiID(),
                    Naziv = naziv,
                    Poeni = poeni,
                    NastavnikIdKorisnika = nastavnik,

                };

                db.Kursevi.Add(k);
                db.SaveChanges();
                //mora posle jer ce u kursNastavnaTemaservisu da pokusa da doda kurs.id u tabelu Sadrzi
                // a taj kurs nije dodat u tabelu Kursevi pa baca exception
                // a nakon svakog mapiranja u KursNasavnaTema servis se radi save changes
                MapirajUTabeluSadrziKursNaNastavnuTemu(k,nastavneTeme);
                
                return true;
            }
        }

        public bool IzmeniKurs(int id, string naziv, int poeni, int nastavnik, List<NastavnaTema> nastavneTeme)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Kurs k = null;
                k = db.Kursevi.FirstOrDefault(x => x.IdKursa == id);
                if (k == null) return false;

                k.Naziv = naziv;
                k.Poeni = poeni;
                k.NastavnikIdKorisnika = nastavnik;
                db.SaveChanges();

                //mora posle save changes zbog exception-a
                MapirajUTabeluSadrziKursNaNastavnuTemu(k, nastavneTeme);

                
                return true;
            }
        }

        public bool ObrisiKurs(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Kurs k = null;
                k = db.Kursevi.FirstOrDefault(x => x.IdKursa == id);
                if (k == null) return false;

                KursNastavnaTemaServis kursNastavnaTemaServis = new KursNastavnaTemaServis();
                var listaTema = kursNastavnaTemaServis.VratiTemeZaKurs(id);
                if(listaTema != null)
                {
                    listaTema.ForEach(item => kursNastavnaTemaServis.Obrisi(id, item.NastavnaTemaIdTeme));
                }

                TestServis testServis = new TestServis();
                testServis.ObrisiTestoveZaKurs(id);

                PrijavljenServis prijavljenServis = new PrijavljenServis();
                var listaSvihPrijavljenih = prijavljenServis.UcitajSvePrijavljene();
                if (listaSvihPrijavljenih != null)
                {
                    foreach (var item in listaSvihPrijavljenih)
                    {
                        if (item.KursIdKursa == id)
                        {
                            prijavljenServis.ObrisiPrijavljen(id, item.UcenikIdKorisnika);
                        }
                    }
                }

                db.Kursevi.Remove(k);
                db.SaveChanges();
                return true;
            }
        }

        public List<Kurs> UcitajSveKurseve()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Kursevi.ToList();
            }
        }

        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Kurs k = null;
                k = db.Kursevi.FirstOrDefault(x => x.IdKursa == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Kursevi.FirstOrDefault(x => x.IdKursa == id); ;
                }

                return id;
            }
        }

        private void MapirajUTabeluSadrziKursNaNastavnuTemu(Kurs k, List<NastavnaTema> nastavneTeme)
        {
            if (nastavneTeme.Count != 0)
            {
                KursNastavnaTemaServis servis = new KursNastavnaTemaServis();
                List<Sadrzi> sveTemeZaKurs = servis.VratiTemeZaKurs(k.IdKursa);
                foreach(var tema in sveTemeZaKurs)
                {
                    //Provera da li postoje u bazi neke koje su bile cekirane na ui
                    // pa ih je korisnik prilikom izmene uklonio -> treba da ih uklonim iz baze
                    if(!nastavneTeme.Exists(x=> x.IdTeme == tema.NastavnaTemaIdTeme))
                    {
                        servis.Obrisi(k.IdKursa, tema.NastavnaTemaIdTeme);
                    }
                }

                foreach (var tema in nastavneTeme)
                {
                    //u dodaj imas proveru da li vec postoji, tako da je obezbedjeno da ce biti svaki par jedinstven
                    servis.Dodaj(k.IdKursa, tema.IdTeme);
                }
            }
        }

        public Kurs VratiKurs(int id)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                Kurs k = null;
                k = db.Kursevi.FirstOrDefault(x => x.IdKursa == id);
                return k;
                
            }
        }

        public void ObrisiSveKurseveZaNastavnika(int idNastavnika)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                while(db.Kursevi.Any(x=> x.NastavnikIdKorisnika == idNastavnika))
                {
                    ObrisiKurs(db.Kursevi.First(x => x.NastavnikIdKorisnika == idNastavnika).IdKursa);
                }
            }
        }

        
    }
}
