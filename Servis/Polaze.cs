//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Servis
{
    using System;
    using System.Collections.Generic;
    
    public partial class Polaze
    {
        public int TestIdTesta { get; set; }
        public int PrijavljenUcenikIdKorisnika { get; set; }
        public int PrijavljenKursIdKursa { get; set; }
    
        public virtual Test Test { get; set; }
        public virtual Prijavljen Prijavljen { get; set; }
    }
}
