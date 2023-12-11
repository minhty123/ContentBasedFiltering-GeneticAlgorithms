using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherManager.Models.Class
{
    public class Event
    {
        public string title { get; set; } // Tên môn học
        public string name { get; set; }
        public string start { get; set; } // Thời gian bắt đầu, định dạng là yyyy-MM-ddThh:mm:ss
        public string end { get; set; } // Thời gian kết thúc, định dạng là yyyy-MM-ddThh:mm:ss
        public string backgroundColor { get; set; } // Tên class áp dụng cho sự kiện, để tùy chỉnh màu sắc của sự kiện
        public string borderColor { get; set; }
        public string textColor { get; set; }
    }
}