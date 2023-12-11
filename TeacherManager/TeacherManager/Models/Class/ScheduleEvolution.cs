using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherManager.Models.Class
{
    public class ScheduleEvolution
    {
        private TeacherWorkEntities db = new TeacherWorkEntities();

        private readonly Random _random;

        List<TIME_SLOT> tIME_SLOTs = new List<TIME_SLOT>();
        
        ~ScheduleEvolution()
        {

        }
        public List<TIME_SLOT> GetTIME_SLOTs()
        {
            return tIME_SLOTs;
        }
        public ScheduleEvolution()
        {
            _random = new Random(Guid.NewGuid().GetHashCode());
        }

        public SchedulePopulation GetOptimalSchedulePopulation(SchedulePopulation population, int generationsCount, CLASSROOM classrooms)
        {
            var currentPopulation = population;

            for (int i = 0; i < generationsCount; i++)
            {
                //sx lịch tăng dần theo độ tương thích
                var orderedPopulation = currentPopulation.Schedules.OrderBy(p => p.GetFitness());
                //lấy 10 tương thích cao nhất
                var bestSchedules = orderedPopulation.Take(7).ToList();

                //tạo quần thể mới 
                var newPopulation = new SchedulePopulation(currentPopulation.Schedules.Count, bestSchedules.First().Teacher, bestSchedules.First().Day);
                foreach (var schedule in newPopulation.Schedules)
                {                  
                    var courses = schedule.GetCoursesOnDayOfTeacher();

                    //Tìm slot vướng lịch dạy của giảng viên
                    foreach (var course in courses)
                    {

                        var slots = db.SUBJECTs
                           .Join(db.ARRANGE_TIME_SLOT, subject => subject.ID, arrangeTimeSlot => arrangeTimeSlot.ID_SUBJECT, (subject, arrangeTimeSlot) => new { subject, arrangeTimeSlot })
                           .Join(db.TIME_SLOT, temp => temp.arrangeTimeSlot.ID_TIME_SLOT, timeSlot => timeSlot.ID, (temp, timeSlot) => new { temp.subject, temp.arrangeTimeSlot, timeSlot })
                           .Join(db.DAYs, temp => temp.timeSlot.ID_DAY, day => day.ID, (temp, day) => new { temp.subject, temp.arrangeTimeSlot, temp.timeSlot, day })
                           .Where(temp => temp.day.NAME.ToString() == schedule.Day.DayOfWeek.ToString() && temp.subject.ID == course.ID)
                           .Select(temp => temp.timeSlot).ToList();

                        foreach (var item in slots)
                        {
                            if (tIME_SLOTs.All(m => m.ID != item.ID))
                                this.tIME_SLOTs.Add(item);

                        }                    
                    }


                    //Tìm slot lớp có lịch học
                    var a = db.CLASSROOMs
                  .Join(db.SUBJECTs, classroom => classroom.ID, subject => subject.ID_CLASSROOM, (classroom, subject) => new { classroom, subject })
                  .Join(db.ARRANGE_TIME_SLOT, cs => cs.subject.ID, arrangeTimeSlot => arrangeTimeSlot.ID_SUBJECT, (cs, arrangeTimeSlot) => new { cs.classroom, cs.subject, arrangeTimeSlot })
                  .Join(db.TIME_SLOT, ca => ca.arrangeTimeSlot.ID_TIME_SLOT, timeSlot => timeSlot.ID, (ca, timeSlot) => new { ca.classroom, ca.subject, ca.arrangeTimeSlot, timeSlot })
                  .Join(db.DAYs, cdat => cdat.timeSlot.ID_DAY, day => day.ID, (cdat, day) => new
                  {
                      cdat.subject,
                      cdat.arrangeTimeSlot,
                      cdat.timeSlot,
                      day
                  }).Where(m => m.subject.CLASSROOM.ID == classrooms.ID && m.day.NAME.ToString() == schedule.Day.DayOfWeek.ToString() && schedule.Day > m.subject.START_DAY && schedule.Day < m.subject.END_DAY)
                 .GroupBy(temp => temp.subject.ID)
                  .Select(group => group.FirstOrDefault().subject)
                  .ToList();

                    foreach (var course in a)
                    {
                        var FullSlotsOfClass = db.SUBJECTs
                       .Join(db.ARRANGE_TIME_SLOT, subject => subject.ID, arrangeTimeSlot => arrangeTimeSlot.ID_SUBJECT, (subject, arrangeTimeSlot) => new { subject, arrangeTimeSlot })
                       .Join(db.TIME_SLOT, temp => temp.arrangeTimeSlot.ID_TIME_SLOT, timeSlot => timeSlot.ID, (temp, timeSlot) => new { temp.subject, temp.arrangeTimeSlot, timeSlot })
                       .Join(db.DAYs, temp => temp.timeSlot.ID_DAY, day => day.ID, (temp, day) => new { temp.subject, temp.arrangeTimeSlot, temp.timeSlot, day })
                       .Where(temp => temp.day.NAME.ToString() == schedule.Day.DayOfWeek.ToString() && temp.arrangeTimeSlot.ID_SUBJECT == course.ID && schedule.Day > temp.subject.START_DAY && schedule.Day < temp.subject.END_DAY)
                       .GroupBy(temp => temp.timeSlot.ID)
                       .Select(group => group.FirstOrDefault().timeSlot)
                       .ToList();

                        foreach (var item in FullSlotsOfClass)
                        {
                            if (tIME_SLOTs.All(m => m.ID != item.ID))
                                this.tIME_SLOTs.Add(item);

                        }

                    }
                }

                currentPopulation = newPopulation;
            }

            return currentPopulation;
        }

    }
}
