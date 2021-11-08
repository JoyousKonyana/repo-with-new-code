﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public partial class OnboarderEquipment
    {
        [Key]
        [Column("EquipmentID")]
        public int EquipmentId { get; set; }
       
        [Column(TypeName = "datetime")]
        public DateTime? EquipmentCheckOutDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EquipmentCheckInDate { get; set; }
        [StringLength(50)]
        public string EquipmentCheckInCondition { get; set; }

        //checked
        //public Boolean? ischecked { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        [InverseProperty("OnboarderEquipment")]
        public virtual Equipment Equipment { get; set; }

        public virtual Onboarder Onboarder { get; set; }
        public int OnboarderID { get; set; }

    }
}
