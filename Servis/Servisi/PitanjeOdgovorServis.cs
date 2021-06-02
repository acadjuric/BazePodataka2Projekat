using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class PitanjeOdgovorServis : IPitanjeOdgovor
    {
        public PitanjeOdgovorServis()
        {

        }

        public bool Dodaj(int pitanje, int odgovor)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Poseduje p = null;
                p = db.Posedujes.FirstOrDefault(x => x.OdgovorIdOdgovora == odgovor && x.PitanjeIdPitanja == pitanje);
                if (p != null) return false;

                p = new Poseduje();
                p.OdgovorIdOdgovora = odgovor;
                p.PitanjeIdPitanja = pitanje;
                db.Posedujes.Add(p);
                db.SaveChanges();
                return true;

            }
        }

        public bool Obrisi(int pitanje, int odgovor)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Poseduje p = null;
                p = db.Posedujes.FirstOrDefault(x => x.OdgovorIdOdgovora == odgovor && x.PitanjeIdPitanja == pitanje);
                if (p == null) return false;

                db.Posedujes.Remove(p);
                db.SaveChanges();
                return true;

            }
        }

        public List<Poseduje> UcitajSvaPitanjaIOdgovore()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Posedujes.ToList();
            }
        }

        public List<Poseduje> VratiPitanjaZaOdgovor(int odgovor)
        {
            using(var db =new BazePodataka2ModelContainer())
            {
                return db.Posedujes.Where(x => x.OdgovorIdOdgovora == odgovor).ToList();
            }
        }

        public List<Poseduje> VratiOdgovoreZaPitanje(int pitanje)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Posedujes.Where(x => x.PitanjeIdPitanja == pitanje).ToList();
            }
        }
    }
}
