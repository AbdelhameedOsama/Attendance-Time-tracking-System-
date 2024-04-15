using Attendance_Time_Tracking.Data;
using Attendance_Time_Tracking.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Time_Tracking.Repos
{
    public interface ITrackRepo
    {
        public List<Track> GetAllTracks();
        public void AddTrack(Track track);
        public void DeleteTrack(int id);
        public void UpdateTrack(Track track);
    }
    public class TrackRepo:ITrackRepo
    {
        readonly AttendanceContext db;
        public TrackRepo(AttendanceContext _db)
        {
            db = _db;
        }
        public List<Track> GetAllTracks()
        {
            return db.Tracks.Include(a => a.Intake).ToList();
        }
        public void AddTrack(Track track)
        {
            //var user = db.Users.FirstOrDefault(a => a.ID == track.SupID);
            //user.Role = UserRole.Supervisor;
            db.Tracks.Add(track);
            db.SaveChanges();
        }
        public void DeleteTrack(int id)
        {
            //delete all permissions
            var track = db.Tracks.FirstOrDefault(a => a.ID==id);
            if(track != null)
            {
                var students = db.Students.Where(a => a.TrackId == track.ID).ToList();

                if (students != null && students.Any())
                {
                    var NumberOfStd = students.Count();
                    var NumberOfTracks = db.Tracks.Count();
                    var AllTracks = db.Tracks.ToList();

                    if (NumberOfTracks > 0)
                    {
                        var NumberOfStdInEachTrack = NumberOfStd / NumberOfTracks;
                        var remainingStudents = NumberOfStd % NumberOfTracks;
                        var studentCounter = 0;

                        foreach (var remainedTracks in AllTracks)
                        {
                            var studentsInThisTrack = students.Skip(studentCounter * NumberOfStdInEachTrack)
                                                               .Take(NumberOfStdInEachTrack)
                                                               .ToList();

                            foreach (var student in studentsInThisTrack)
                            {
                                student.TrackId = remainedTracks.ID;
                            }

                            studentCounter++;
                        }
                        if (remainingStudents > 0)
                        {
                            var remainingStudentsList = students.Skip(studentCounter * NumberOfStdInEachTrack)
                                                                .Take(remainingStudents)
                                                                .ToList();

                            foreach (var student in remainingStudentsList)
                            {
                                student.TrackId = AllTracks.Last().ID;
                            }
                        }
                    }
                }
                var schedules = db.Schedules.Where(a => a.TrackId==track.ID).ToList();
                foreach(var schedule in schedules)
                {
                    if(schedule != null)
                    {
                        db.Schedules.Remove(schedule);
                    }
                }
                db.Remove(track);
                // remove supervisor of this track from intructors table
                var supervisor =db.Instructors.FirstOrDefault( a => a.ID == track.SupID);
                if(supervisor != null)
                {
                    db.Instructors.Remove(supervisor);
                }
                // change userRole of the supervisor to be normal instructor :)
                var user = db.Users.FirstOrDefault(a => a.ID==supervisor.ID);
                if(user != null)
                {
                    user.Role=UserRole.Instructor;
                    db.Update(user);
                }
                var oldUser = user;
                db.SaveChanges();
            }


        }
        public void UpdateTrack(Track track)
        {
            var OldTrack = db.Tracks.FirstOrDefault(a => a.ID == track.ID);
            if (OldTrack != null)
            {
                OldTrack.SupID = track.SupID;
                OldTrack.Name = track.Name;
                OldTrack.Status= track.Status;
                db.SaveChanges();
            }
        }
    }

}
