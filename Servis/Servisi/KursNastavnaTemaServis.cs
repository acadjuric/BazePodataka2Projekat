using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class KursNastavnaTemaServis : IKursNastavnaTema
    {
        public bool Dodaj(int kurs, int tema)
        {
            
            using (var db = new BazePodataka2ModelContainer())
            {
                Sadrzi k = null;
                k = db.Sadrzis.FirstOrDefault(x => x.NastavnaTemaIdTeme == tema && x.KursIdKursa == kurs);
                if (k != null) return false; //vec postoji ne moze se dodati 
                k = new Sadrzi();
                k.KursIdKursa = kurs;
                k.NastavnaTemaIdTeme = tema;
                db.Sadrzis.Add(k);
                db.SaveChanges();
                return true;
            }
        }



        public bool Obrisi(int kurs, int tema)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Sadrzi k = null;
                k = db.Sadrzis.FirstOrDefault(x => x.NastavnaTemaIdTeme == tema && x.KursIdKursa == kurs);
                if (k == null) return false;

                db.Sadrzis.Remove(k);
                db.SaveChanges();
                return true;
            }

        }

        public List<Sadrzi> UcitajSveKurseveINastavneTeme()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Sadrzis.ToList();
            }
        }

        public List<Sadrzi> VratiTemeZaKurs(int idKursa)
        {
            using(var db= new BazePodataka2ModelContainer())
            {
                return db.Sadrzis.Where(x => x.KursIdKursa == idKursa).ToList();
            }
        }

        public List<Sadrzi> VratiKurseveZaTemu(int idTeme)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Sadrzis.Where(x => x.NastavnaTemaIdTeme == idTeme).ToList();
            }
        }
    }
}
