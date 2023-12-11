using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherManager.Models;
using TeacherManager.Models.Class;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace TeacherManager.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeachingScheduleController : Controller
    {
      
        TeacherWorkEntities db = new TeacherWorkEntities();
        
        // GET: TeachingSchedule
        public ActionResult Timetable()
        {
            return View();
        }

        public ActionResult GetEventsTimetable(DateTime startOfWeek)
        {
            string ID_USER = User.Identity.GetUserId();
            TEACHER teacher = db.TEACHERs.FirstOrDefault(t => t.ID_USER == ID_USER);

            List<Event> scheduleList = new List<Event>();

            DateTime endOfWeek = startOfWeek.AddDays(6);

            var aPPLICATION_LEAVE = db.APPLICATION_LEAVE.Where(m => m.ID_TEACHER==teacher.ID && m.STATUS!="Đang chờ duyệt");

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

            for (var date = startOfWeek.Date; date <= endOfWeek.Date; date = date.AddDays(1))
            {
                if(dateTimes.Any(x => x == date) == true)
                {
                    continue;
                }
                List<SUBJECT> subjectList = teacher.GetSUBJECTs(date);
                List<MAKEUP_LESSON> mAKEUP_LESSONs = db.MAKEUP_LESSON.Where(m => m.DATE == date && m.SUBJECT.TEACHER.ID == teacher.ID && m.SITUATION != "Đang chờ duyệt").ToList();
                foreach (var item in mAKEUP_LESSONs)
                {
                    Event schedule = new Event();
                    schedule.title = "LỊCH BÙ\nMôn: " + item.SUBJECT.NAME + "\nLớp: " + item.CLASSROOM.NAME + "\nPhòng: " + item.ROOM.NAME_ROM;
                    schedule.start = date.ToString("yyyy-MM-dd") + 'T' + item.TIMESTART;
                    schedule.end = date.ToString("yyyy-MM-dd") + 'T' + item.TIMEEND;

                    schedule.backgroundColor = "#f56954";
                    schedule.borderColor = "#f56954";
                    schedule.textColor = "#191970";

                    scheduleList.Add(schedule);
                }
                foreach (var subject in subjectList)
                {
                    List<TIME_SLOT> timeSlotList = subject.GetTIME_SLOTs(date);

                    Event schedule = new Event();
                    schedule.title = "Môn: " + subject.NAME + "\nLớp: " + subject.CLASSROOM.NAME + "\nPhòng: " + subject.ROOM.NAME_ROM;
                    schedule.start = date.ToString("yyyy-MM-dd") + 'T' + timeSlotList.FirstOrDefault().NAME;

                    TimeSpan timestartlast = TimeSpan.ParseExact(timeSlotList.LastOrDefault().NAME, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                    TimeSpan time = new TimeSpan(0, 50, 0);
                    TimeSpan timeend = timestartlast.Add(time);

                    schedule.end = date.ToString("yyyy-MM-dd") + 'T' + timeend.ToString();
                    schedule.backgroundColor = "#FFA500";
                    schedule.borderColor = "#FFA500";
                    schedule.textColor = "#191970";
                    scheduleList.Add(schedule);
                }
            }

            if (HttpContext.Cache["scheduleList"] != null)
            {
                HttpContext.Cache.Remove("scheduleList"); // Xóa danh sách sự kiện cũ khỏi cache
            }

            HttpContext.Cache.Insert("scheduleList", scheduleList, null, DateTime.Now.AddDays(1), TimeSpan.Zero); // Lưu danh sách sự kiện mới vào cache

            return Json(scheduleList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExamTimetable()
        {
            return View();
        }

        public ActionResult GetEventsExamsTimetable(DateTime startOfWeek)
        {
            string ID_USER = User.Identity.GetUserId();
            TEACHER teacher = db.TEACHERs.FirstOrDefault(t => t.ID_USER == ID_USER);

            List<Event> scheduleList = new List<Event>();

            DateTime endOfWeek = startOfWeek.AddDays(6);
            for (var date = startOfWeek.Date; date <= endOfWeek.Date; date = date.AddDays(1))
            {
                // Thêm điều kiện kiểm tra tEST_SCHEDULEs.Count > 0 để đảm bảo chỉ lấy sự kiện lịch thi khi có ít nhất một kỳ thi được sắp xếp
                List<TEST_SCHEDULE> tEST_SCHEDULEs = teacher.GetExamsSUBJECTs(date);
                if (tEST_SCHEDULEs.Count > 0)
                {
                    foreach (var item in tEST_SCHEDULEs)
                    {
                        Event schedule = new Event();
                        schedule.title = "Môn: " + item.SUBJECT.NAME + " - "+item.ROOM.NAME_ROM;
                        schedule.start = date.ToString("yyyy-MM-dd") + 'T' + item.TIMESTART;

                        TimeSpan timestart = TimeSpan.ParseExact(item.TIMESTART, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                        TimeSpan time = TimeSpan.ParseExact(item.TIME, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                        TimeSpan timeend = timestart.Add(time);

                        schedule.end = date.ToString("yyyy-MM-dd") + 'T' + timeend.ToString();
                        schedule.backgroundColor = "#FFA500";
                        schedule.borderColor = "#FFA500";
                        schedule.textColor = "#191970";
                        scheduleList.Add(schedule);
                    }
                }
            }

            if (HttpContext.Cache["scheduleList"] != null)
            {
                HttpContext.Cache.Remove("scheduleList"); // Xóa danh sách sự kiện cũ khỏi cache
            }

            HttpContext.Cache.Insert("scheduleList", scheduleList, null, DateTime.Now.AddDays(1), TimeSpan.Zero); // Lưu danh sách sự kiện mới vào cache

            return Json(scheduleList, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult RegisterApplicationForLeave()
        {
            ViewBag.ID_TEACHER = new SelectList(db.TEACHERs, "ID", "NAME");
            return View();
        }

        // POST: APPLICATION_LEAVE/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterApplicationForLeave([Bind(Include = "ID,ID_TEACHER,DATESTART,REASON,STATUS,DATEEND,TYPELEAVE")] APPLICATION_LEAVE aPPLICATION_LEAVE)
        {
           
            if (ModelState.IsValid)
            {

                string ID_USER = User.Identity.GetUserId();
                TEACHER teacher = db.TEACHERs.FirstOrDefault(t => t.ID_USER == ID_USER);
                aPPLICATION_LEAVE.ID_TEACHER = teacher.ID;
                aPPLICATION_LEAVE.STATUS = "Đang chờ duyệt";

                db.APPLICATION_LEAVE.Add(aPPLICATION_LEAVE);
                db.SaveChanges();
                return RedirectToAction("Index", "Home", new { areas = "" });
            }

            ViewBag.ID_TEACHER = new SelectList(db.TEACHERs, "ID", "NAME", aPPLICATION_LEAVE.ID_TEACHER);
            return View(aPPLICATION_LEAVE);
        }


        public ActionResult Register()
        {

            string ID_USER = User.Identity.GetUserId();
            TEACHER teacher = db.TEACHERs.FirstOrDefault(t => t.ID_USER == ID_USER);

            var result = db.CLASSROOMs
            .Join(db.SUBJECTs, classroom => classroom.ID, subject => subject.ID_CLASSROOM, (classroom, subject) => new { classroom, subject })
            .Where(temp => temp.subject.ID_TEACHER == teacher.ID)
            .GroupBy(temp => temp.classroom.ID)
            .Select(group => group.FirstOrDefault().classroom)
            .ToList();
            ViewBag.Classrooms = new SelectList(result, "ID", "NAME", "Chọn lớp");
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {

            string ID_USER = User.Identity.GetUserId();
            TEACHER tEACHER = db.TEACHERs.FirstOrDefault(t => t.ID_USER == ID_USER);
            // Lấy các giá trị từ FormCollection (form) bằng phương thức TryGetValue,
            // và thực hiện các xử lý tương ứng,
            // ví dụ: lấy giá trị của dropdownlist "Classrooms"
            int Idclassrooms;
            int.TryParse(form["Classrooms"], out Idclassrooms);

            // giá trị của dropdownlist "Subjects"
            int Idsubjects;
            int.TryParse(form["Subjects"], out Idsubjects);

            // giá trị của trường nhập "number_lesson"
            int numberLesson;
            int.TryParse(form["number_lesson"], out numberLesson);

            // xử lý các giá trị này theo yêu cầu
            var cLASSROOM = db.CLASSROOMs.Where( m=> m.ID==Idclassrooms).First();
            Schedule schedule = new Schedule(tEACHER, DateTime.Now);

            // Khởi tạo lịch dạy bù cho giảng viên
            var startDate = DateTime.Today.AddDays(7); // bắt đầu tính từ tuần sau
            var schedulePopulation = new SchedulePopulation(7, tEACHER, startDate);

            //Tối ưu hóa với thuật toán di truyền
            var evolution = new ScheduleEvolution();
            var optimalSchedules = evolution.GetOptimalSchedulePopulation(schedulePopulation, 10, cLASSROOM);

            var timeslotsls = evolution.GetTIME_SLOTs().ToList();
            var list = optimalSchedules.Schedules.OrderBy(m => m.GetFitness());
            Schedule temp = new Schedule();
            foreach (var item in list)
            {
                if (item.SlotAvailible(timeslotsls).Count >= numberLesson)
                {
                  temp = item;
                    break;
                }

            }

            //Tim phong

            List<SUBJECT> SubOnDay = temp.GetCoursesOnDay();
            List<ROOM> rOOMs = new List<ROOM>();
            foreach (var item in SubOnDay)
            {
                ROOM rOOM = db.ROOMs.Find(item.ID_ROOM);
                rOOMs.Add(rOOM);
            }
            ViewBag.Room = db.ROOMs.ToList().Except(rOOMs, new Room_Comparer()).First();
            //Tim slot
            List<TIME_SLOT> lsTimeSlot = temp.SlotAvailible(timeslotsls);
            ViewBag.a = lsTimeSlot;
            ViewBag.timeStart = lsTimeSlot.OrderBy(s => s.ID).FirstOrDefault().NAME;
            var timeTemp = lsTimeSlot.ElementAt(numberLesson-1).NAME;
            TimeSpan time = TimeSpan.Parse("00:50:00");
            TimeSpan timeend = TimeSpan.ParseExact(timeTemp, "hh\\:mm\\:ss", CultureInfo.InvariantCulture).Add(time);
            ViewBag.timeEnd = timeend.ToString();
            ViewBag.Schedule = temp;
            ViewBag.ClassName = db.CLASSROOMs.Find(Idclassrooms);
            ViewBag.SubjectName = db.SUBJECTs.Find(Idsubjects);
            return View("Show");
        }



        public ActionResult Show()
        {          
            return View();
        }
        [HttpPost]
        public ActionResult AddMakeupLesson(string classname,string  subjectname, DateTime day, string timestart, string timeend,string room)
        {
            int IdClass = db.CLASSROOMs.Where(m => m.NAME == classname).First().ID;
            int IdSubject = db.SUBJECTs.Where(m => m.NAME == subjectname).First().ID;
            int IdRoom = db.ROOMs.Where(m => m.NAME_ROM == room).First().ID;
            MAKEUP_LESSON mAKEUP_LESSON = new MAKEUP_LESSON();
            mAKEUP_LESSON.ID_CLASS = IdClass;
            mAKEUP_LESSON.ID_SUBJECT = IdSubject;
            mAKEUP_LESSON.DATE = day;
            mAKEUP_LESSON.TIMESTART = timestart;
            mAKEUP_LESSON.TIMEEND = timeend;
            mAKEUP_LESSON.ID_ROOM=IdRoom;
            mAKEUP_LESSON.SITUATION = "Đang chờ duyệt";
            db.MAKEUP_LESSON.Add(mAKEUP_LESSON);
            db.SaveChanges();

            return RedirectToAction("Index", "Home", new { areas = "" });
        }



        public JsonResult LoadSubjects(int classroomId)
        {
            string ID_USER = User.Identity.GetUserId();
            TEACHER tEACHER = db.TEACHERs.FirstOrDefault(t => t.ID_USER == ID_USER);

            var subjects = db.SUBJECTs
                .Where(s => s.ID_CLASSROOM == classroomId && s.TEACHER.ID == tEACHER.ID && s.START_DAY < DateTime.Now && s.END_DAY > DateTime.Now)
                .Select(s => new { Value = s.ID, Text = s.NAME })
                .ToList();
            return Json(subjects, JsonRequestBehavior.AllowGet);
        }

        // Nhập vào một ngày có trong tuần và trả về danh sách các ngày trong tuần đó
        public List<DateTime> GetWeek(DateTime date)
        {
            // Tìm ngày đầu tiên trong tuần chứa ngày đó
            DateTime startOfWeek = date.AddDays(-(int)date.DayOfWeek);

            // Tạo danh sách các ngày trong tuần chứa ngày đó
            List<DateTime> weekDays = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                weekDays.Add(startOfWeek.AddDays(i));
            }
            return weekDays;
        }

        public bool HasConsecutiveNumbers(List<int> numbers, int lesson)
        {
            int count = 1;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1] - 1)
                {
                    count++;
                    if (count == lesson)
                    {
                        // Tìm thấy 5 số liên tiếp
                        return true;
                    }
                }
                else
                {
                    count = 1;
                }
            }

            // Không tìm thấy 5 số liên tiếp
            return false;
        }

    }

    class Room_Comparer : IEqualityComparer<ROOM>
    {
        public bool Equals(ROOM x, ROOM y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(ROOM obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}