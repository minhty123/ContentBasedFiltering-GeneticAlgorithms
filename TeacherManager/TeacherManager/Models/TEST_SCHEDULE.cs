﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeacherManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    public partial class TEST_SCHEDULE
    {
        public int ID { get; set; }
        public Nullable<int> ID_TEACHER_CHECK { get; set; }
        public Nullable<int> ID_SUBJECT { get; set; }
        public Nullable<int> ID_ROOM { get; set; }
        [DisplayName("Số lượng")]
        public Nullable<int> NUMBER_STUDENT { get; set; }
        [DisplayName("Ngày")]
        public Nullable<System.DateTime> DATE { get; set; }
        [DisplayName("Giờ bắt đầu")]
        public string TIMESTART { get; set; }
        [DisplayName("Thời gian")]
        public string TIME { get; set; }
    
        public virtual ROOM ROOM { get; set; }
        public virtual TEACHER TEACHER { get; set; }
        public virtual SUBJECT SUBJECT { get; set; }
    }
}