//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeddingApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class MENU
    {
        public int MENUID { get; set; }
        public int DISHID { get; set; }
        public int WEDDINGID { get; set; }
        public Nullable<decimal> DISHCOST { get; set; }
        public string NOTE { get; set; }
    
        public virtual DISH DISH { get; set; }
        public virtual WEDDING WEDDING { get; set; }
    }
}
