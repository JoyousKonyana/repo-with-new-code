﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public partial class LessonContent
    {
        [Key]
        [Column("LessonConentID")]
        public int LessonConentId { get; set; }
        [Column("LessonContenetTypeID")]
        public int? LessonContenetTypeId { get; set; }
        [Column("LessonOutcomeID")]
        public int? LessonOutcomeId { get; set; }
        [Column("ArchiveStatusID")]
        public int? ArchiveStatusId { get; set; }
        [StringLength(50)]
        public string LessonContentDescription { get; set; }
        [Column("LessonContent")]
        public string LessonContent1 { get; set; }
        public int? LessonContentTypeId { get; set; }

        [ForeignKey(nameof(ArchiveStatusId))]
        [InverseProperty("LessonContent")]
        public virtual ArchiveStatus ArchiveStatus { get; set; }
        [ForeignKey(nameof(LessonContentTypeId))]
        [InverseProperty("LessonContent")]
        public virtual LessonContentType LessonContentType { get; set; }
    }
}