//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitTestArticle
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int ID { get; set; }
        public int Account_ID { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Transaction_Date { get; set; }
        public Nullable<decimal> New_Balance { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
