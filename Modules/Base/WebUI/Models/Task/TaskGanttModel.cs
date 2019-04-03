﻿using System;

namespace WebUI.Models.Task
{
    public class TaskGanttModel
    {
        public string ID { get; set; }
        public string ParentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public double OrderId { get; set; }
        public bool Summary { get; set; }
        public string Title { get; set; }
        public bool Expanded { get; set; }
    }
}