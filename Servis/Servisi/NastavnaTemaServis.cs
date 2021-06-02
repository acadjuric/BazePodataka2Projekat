using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class NastavnaTemaServis : INastavnaTema
    {
        public NastavnaTemaServis()
        {

        }
        public bool DodajNastavnuTemu(string naziv, List<Kurs> kursevi)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                NastavnaTema n = new NastavnaTema()
                {
                    IdTeme = IzgenerisiID(),
                    Naziv = naziv,
                };

                db.NastavneTeme.Add(n);

                db.SaveChanges();

                MapirajUTabeluSadrziNastavnuTemuNaKurs(n, kursevi);
                
                return true;
            }
        }

        public bool IzmeniNastavnuTemu(int id, string naziv, List<Kurs> kursevi)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                NastavnaTema n = null;
                n = db.NastavneTeme.FirstOrDefault(x => x.IdTeme == id);
                if (n == null) return false;

                n.Naziv = naziv;
                db.SaveChanges();

                MapirajUTabeluSadrziNastavnuTemuNaKurs(n, kursevi);
                
                return true;
            }
        }

        public bool ObrisiNastavnuTemu(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                NastavnaTema n = null;
                n = db.NastavneTeme.FirstOrDefault(x => x.IdTeme == id);
                if (n == null) return false;

                KursNastavnaTemaServis kursNastavnaTemaServis = new KursNastavnaTemaServis();
                var lista = kursNastavnaTemaServis.VratiKurseveZaTemu(id);
                if(lista != null)
                {
                    lista.ForEach(item => kursNastavnaTemaServis.Obrisi(item.KursIdKursa, id));
                }


                db.NastavneTeme.Remove(n);
                db.SaveChanges();
                return true;
            }

        }

        public List<NastavnaTema> UcitajSveNastavneTeme()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.NastavneTeme.ToList();
            }
        }

        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                NastavnaTema k = null;
                k = db.NastavneTeme.FirstOrDefault(x => x.IdTeme == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.NastavneTeme.FirstOrDefault(x => x.IdTeme == id); ;
                }

                return id;
            }
        }

        private void MapirajUTabeluSadrziNastavnuTemuNaKurs(NastavnaTema tema, List<Kurs> kursevi)
        {
            if (kursevi.Count != 0)
            {
                KursNastavnaTemaServis servis = new KursNastavnaTemaServis();

                List<Sadrzi> sviKurseviZaTemu = servis.VratiKurseveZaTemu(tema.IdTeme);

                foreach (var kurs in sviKurseviZaTemu)
                {
                    //Provera da li postoje u bazi neke koje su bile cekirane na ui
                    // pa ih je korisnik prilikom izmene uklonio -> treba da ih uklonim iz baze
                    if (!kursevi.Exists(x => x.IdKursa == kurs.KursIdKursa))
                    {
                        servis.Obrisi(kurs.KursIdKursa, tema.IdTeme);
                    }
                }

                foreach (var kurs in kursevi)
                {
                    //u dodaj imas proveru da li vec postoji, tako da je obezbedjeno da ce biti svaki par jedinstven
                    servis.Dodaj(kurs.IdKursa, tema.IdTeme);
                }
            }
        }

        public NastavnaTema VratiNastavnuTemu(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                NastavnaTema k = null;
                k = db.NastavneTeme.FirstOrDefault(x => x.IdTeme == id);
                return k;

            }
        }
    }
}
