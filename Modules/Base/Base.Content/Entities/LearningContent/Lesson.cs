﻿using System;
using System.Collections.Generic;
using Base.Utils.Common.Maybe;

namespace Base.Content.Entities
{
    public class Lesson
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public Guid ImageID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? LastExerciseID { get; set; }
        public List<ExerciseUI> Exercises { get; set; }
        public bool Complete { get; set; }
        public int Pct { get; set; }
        public Lesson(CourseCategory source)
        {
            this.ID = source.ID;
            this.CourseID = (int)source.ParentID;
            this.ImageID = source.Image.WithStruct(x => x.FileID, Guid.Empty);
            this.Title = source.Name;
            this.Description = source.Description;
        }
    }
}
