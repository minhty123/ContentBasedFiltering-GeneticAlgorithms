using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TeacherManager.Models.Class
{
    public class Schedule
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();
        public TEACHER Teacher { get; set; }

        public DateTime Day { get; set; }
        ~Schedule()
        {
        }
        public Schedule(TEACHER teacher, DateTime day)
        {
            Teacher = teacher;
            Day = day;
        }
        public Schedule()
        {
        }

        public List<SUBJECT> GetCoursesOnDayOfTeacher()
        {

            //DS MÔN GV DẠY TRONG 1 NGÀY CỤ THỂ
            var result = db.SUBJECTs
                .Join(db.ARRANGE_TIME_SLOT, subject => subject.ID, arrangeTimeSlot => arrangeTimeSlot.ID_SUBJECT, (subject, arrangeTimeSlot) => new { subject, arrangeTimeSlot })
                .Join(db.TIME_SLOT, temp => temp.arrangeTimeSlot.ID_TIME_SLOT, timeSlot => timeSlot.ID, (temp, timeSlot) => new { temp.subject, temp.arrangeTimeSlot, timeSlot })
                .Join(db.DAYs, temp => temp.timeSlot.ID_DAY, day => day.ID, (temp, day) => new { temp.subject, temp.arrangeTimeSlot, temp.timeSlot, day })
                .Where(temp => temp.subject.ID_TEACHER == Teacher.ID && temp.day.NAME.ToString() == Day.DayOfWeek.ToString() && Day > temp.subject.START_DAY && Day < temp.subject.END_DAY)
                .GroupBy(temp => temp.subject.ID)
                .Select(group => group.FirstOrDefault().subject)
                .ToList();


            return result;

        }

        public List<MAKEUP_LESSON> GetMakeupLessonOnDayOfTeacher()
        {
            //DS DẠY BÙ GV DẠY TRONG 1 NGÀY CỤ THỂ
            var result = db.MAKEUP_LESSON.Where(m => m.SUBJECT.ID_TEACHER == Teacher.ID && m.DATE == Day && m.SITUATION!="Đang chờ duyệt").ToList();
            return result;

        }

        public List<SUBJECT> GetCoursesOnDay()
        {

            //DS MÔN TRONG 1 NGÀY CỤ THỂ
            var result = db.SUBJECTs
                .Join(db.ARRANGE_TIME_SLOT, subject => subject.ID, arrangeTimeSlot => arrangeTimeSlot.ID_SUBJECT, (subject, arrangeTimeSlot) => new { subject, arrangeTimeSlot })
                .Join(db.TIME_SLOT, temp => temp.arrangeTimeSlot.ID_TIME_SLOT, timeSlot => timeSlot.ID, (temp, timeSlot) => new { temp.subject, temp.arrangeTimeSlot, timeSlot })
                .Join(db.DAYs, temp => temp.timeSlot.ID_DAY, day => day.ID, (temp, day) => new { temp.subject, temp.arrangeTimeSlot, temp.timeSlot, day })
                .Where(temp => temp.day.NAME.ToString() == Day.DayOfWeek.ToString() && Day > temp.subject.START_DAY && Day < temp.subject.END_DAY)
                .GroupBy(temp => temp.subject.ID)
                .Select(group => group.FirstOrDefault().subject)
                .ToList();


            return result;

        }
        public double GetFitness()
        {
            var courses = GetCoursesOnDayOfTeacher();
            var makeup_lessons = GetMakeupLessonOnDayOfTeacher();

            var aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Where(m => m.ID_TEACHER == Teacher.ID && m.STATUS != "Đang chờ duyệt");

            List<DateTime> dateTimes = new List<DateTime>();

            foreach (var temp in aPPLICATION_LEAVE)
            {
                List<DateTime> days = new List<DateTime>();
                if (aPPLICATION_LEAVE != null)
                {
                    DateTime startDate = (DateTime)temp.DATESTART; // Ngày bắt đầu
                    DateTime endDate = (DateTime)temp.DATEEND; // Ngày kết thúc
                    days = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                .Select(day => startDate.AddDays(day))
                                .ToList(); // Danh sách các ngày giữa startDate và endDate
                    dateTimes.AddRange(days);

                }
            }

            if (dateTimes.Any(x => x == Day) == true)
            {
                return 10;
            }
            else if (courses == null && makeup_lessons==null)               //nếu ds trống thì độ tương thích là 0
            {
                return 0;
            }
            else
            {

                var fitness = 0.0;

                int lessonDuration = 50; //mỗi tiết học có thời lượng 50 phút 
                int breakDuration = 5; //thời lượng giữa các tiết là 5 phút 

                foreach ( var makeup_lesson in makeup_lessons)
                {
                    TimeSpan timestart = TimeSpan.ParseExact(makeup_lesson.TIMESTART, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                    TimeSpan timeend = TimeSpan.ParseExact(makeup_lesson.TIMEEND, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);

                    TimeSpan duration = timeend-timestart;
                    int totalMinutes = (int)duration.TotalMinutes;

                    int timeBetweenLessons = lessonDuration + breakDuration;
                    int lessonCount = (totalMinutes + breakDuration) / timeBetweenLessons;

                    fitness += lessonCount;
                }
                foreach (var course in courses)
                {
                    var slots = db.SUBJECTs
                    .Join(db.ARRANGE_TIME_SLOT, subject => subject.ID, arrangeTimeSlot => arrangeTimeSlot.ID_SUBJECT, (subject, arrangeTimeSlot) => new { subject, arrangeTimeSlot })
                    .Join(db.TIME_SLOT, temp => temp.arrangeTimeSlot.ID_TIME_SLOT, timeSlot => timeSlot.ID, (temp, timeSlot) => new { temp.subject, temp.arrangeTimeSlot, timeSlot })
                    .Join(db.DAYs, temp => temp.timeSlot.ID_DAY, day => day.ID, (temp, day) => new { temp.subject, temp.arrangeTimeSlot, temp.timeSlot, day })
                    .Where(temp => temp.subject.ID_TEACHER == Teacher.ID && temp.day.NAME.ToString() == Day.DayOfWeek.ToString() && temp.subject.ID == course.ID)
                    .Select(temp => temp.timeSlot)
                    .ToList();



                    //Tổng số tiết đã được lên lịch dạy
                    fitness += (double)slots.Count;
                }

                //fitness dụa trên tỉ lệ số tiết đã lên lịch trên tổng số tiết 1 ngày
                //fitness càng nhỏ thì tiết trống càng nhiều, nên lựa chọn ngày có fitness nhỏ
                return Math.Round(fitness / 10, 2);
            }
        }

        //Lọc ra slot trống cho 1 ngày dựa trên lịch đã được lên trong 1 ngày
        public List<TIME_SLOT> SlotAvailible(List<TIME_SLOT> tIME_SLOTs)
        {
            var slots_all_day = db.TIME_SLOT.Where(m => m.DAY.NAME.ToString() == Day.DayOfWeek.ToString()).ToList();
            var timeAvailable = tIME_SLOTs.Where(s => s.DAY.NAME == Day.DayOfWeek.ToString()).ToList();
            if (timeAvailable == null)
            {
                return slots_all_day;
            }
            else
            {
                return slots_all_day.Except(timeAvailable, new Time_Slot_Comparer()).ToList();
            }

        }

        public double FindLongestContSubseq(List<int> ls)
        {
            int maxLen = 1; // khởi tạo độ dài dãy con lớn nhất ban đầu là 1
            int currentLen = 1; // khởi tạo độ dài dãy con hiện tại ban đầu là 1
            bool hasConsecutive = false;
            for (int i = 1; i < ls.Count; i++)
            {
                if (ls[i] == ls[i - 1] + 1 || ls[i] == ls[i - 1] - 1)
                {
                    currentLen++;
                    hasConsecutive = true;
                }
                else // nếu phần tử hiện tại không liên tiếp với phần tử trước đó
                {
                    if (currentLen > maxLen) // nếu độ dài dãy con hiện tại lớn hơn độ dài dãy con lớn nhất
                    {
                        maxLen = currentLen; // cập nhật độ dài dãy con lớn nhất bằng độ dài dãy con hiện tại
                    }

                    // reset lại độ dài dãy conện tại về 1 để bắt đầu tính dãy con mới
                    currentLen = 1;
                }
            }

            // kiểm tra lại một lần cu trước khi kết thúc hàm
            if (currentLen > maxLen)
            {
                maxLen = currentLen;
            }

            double decimalMaxLen = (double)maxLen / 10;
            decimalMaxLen = Math.Round(decimalMaxLen, 2);

            return hasConsecutive ? decimalMaxLen : 0;
        }


    }

    class Time_Slot_Comparer : IEqualityComparer<TIME_SLOT>
    {
        public bool Equals(TIME_SLOT x, TIME_SLOT y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(TIME_SLOT obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}