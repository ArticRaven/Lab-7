using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace Take_home_Lab_7
{
        class Program
    {
        static void List()
        {
            using (var db = new AppDbContext())
            {
                var allStuff = db.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student);

                foreach (var course in allStuff)
                {
                    Console.WriteLine($"{course.Description} -");
                    foreach (var student in course.StudentCourses)
                    {
                        Console.WriteLine($"\t{student.Student.FirstName} {student.Student.LastName} {student.StudentGPA}");
                    }
                    Console.WriteLine();
                }
            }
        }
        static void Main(string[] args)
        {

             using (var db = new AppDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                List<Student> students = new List<Student>()
                {
                    new Student {FirstName = "Rick", LastName = "James"},
                    new Student {FirstName = "John", LastName = "Belushi"},
                    new Student {FirstName = "Tom", LastName = "Jones"},
                    new Student {FirstName = "Bill", LastName = "Murray"},
                    new Student {FirstName = "Snoop", LastName = "Dogg"},
                };

                List<Course> courses = new List<Course>() 
                {
                    new Course {Description = "College Algebra"},
                    new Course {Description = "Underwater Basket Weaving"},
                };

                List<StudentCourse> joinTable = new List<StudentCourse>() 
                {
                    new StudentCourse {Student = students[0], Course = courses[0], StudentGPA = "2.0"},
                    new StudentCourse {Student = students[1], Course = courses[0], StudentGPA = "4.0"},
                    new StudentCourse {Student = students[2], Course = courses[0], StudentGPA = "3.25"},
                    new StudentCourse {Student = students[3], Course = courses[0], StudentGPA = "2.75"},
                    new StudentCourse {Student = students[0], Course = courses[1], StudentGPA = "1.75"},
                    new StudentCourse {Student = students[1], Course = courses[1], StudentGPA = "3.33"},
                    new StudentCourse {Student = students[2], Course = courses[1], StudentGPA = "2.0"},
                    new StudentCourse {Student = students[3], Course = courses[1], StudentGPA = "1.75"},
                };

                db.AddRange(students);
                db.AddRange(courses);
                db.AddRange(joinTable);
                db.SaveChanges();
            }
            List();

            using (var db = new AppDbContext())
            {
                // Move Student "C" from Course 1 to Course 2
                // Here's what you need
                // Student id of student moving
                // Course id of OLD course
                // Course id of NEW course
                int studentId = 3;
                int oldCourseId = 1;
                int newCourseId = 2;

                StudentCourse  scToRemove = db.StudentCourses.Find(studentId, oldCourseId);
                Student s = db.Students.Find(studentId);
                Course c = db.Courses.Find(newCourseId);
                db.Remove(scToRemove);
                db.SaveChanges();

                StudentCourse epSecondOne = new StudentCourse {
                    Student = db.Students.Find(5), // This is Student named Snoop Dogg
                    Course = db.Courses.Find(2), // This is Course 1
                    StudentGPA = "4.0"
                };

                db.Add(epSecondOne);
                db.SaveChanges();
            }
            List();

            using (var db = new AppDbContext())
            {
                var query = db.StudentCourses.Include(sc => sc.Course).Include(sc=> sc.Student)
                                    .Where(sc => sc.StudentGPA == "3.0");
                
                foreach (var sc in query)
                {
                    Console.WriteLine($"{sc.Course.Description} - {sc.Student.FirstName}");
                }

                var projectionQuery = db.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student)
                                        .Select(
                                            c => new {
                                                Description = c.Description,
                                                StudentCourse = c.StudentCourses.Where(sc2 => sc2.StudentGPA == "1.5")
                                            }
                                        );
                foreach (var blah in projectionQuery)
                {
                    Console.WriteLine($"{blah.Description}");
                    foreach (var blah2 in blah.StudentCourse)
                    {
                        Console.WriteLine($"{blah2.Student.FirstName}");
                    }
                }
            }
        }
    }
}
