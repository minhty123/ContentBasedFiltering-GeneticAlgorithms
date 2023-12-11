using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherManager.Models.Class
{
    public class SchedulePopulation
    {
        public List<Schedule> Schedules { get; set; }
        ~SchedulePopulation()
        {

        }
        //Tạo quần thể lịch học
        public SchedulePopulation(int size, TEACHER teacher, DateTime startDay)
        {
            Schedules = new List<Schedule>();

            for (int i = 0; i < size; i++)
            {
                Schedules.Add(new Schedule(teacher, startDay.AddDays(i)));

                //ngày tăng dần, lich học có mỗi ngày, đem tính độ tương thích với lịch dạy mỗi ngày của giáo viên
            }
        }

    }
}