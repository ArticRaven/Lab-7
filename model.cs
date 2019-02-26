using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Take_home_Lab_7
{
    public class AppDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(s => new {s.StudentId, s.CourseId});
        }
        public DbSet<Student> Students {get; set;}
        public DbSet<Course> Courses {get; set;}
        public DbSet<StudentCourse> StudentCourses {get; set;}
    }

    public class Student
    {
        public int StudentId {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string StudentGPA{get; set;}
        public List<StudentCourse> StudentCourses {get; set;} // Navigation Course. Students can have MANY Studentrojects
    }

    public class Course
    {
        public int CourseId {get; set;}
        public string Description {get; set;}
        public decimal Budget {get; set;}
        public List<StudentCourse> StudentCourses {get; set;} // Navigation Property. Course can have MANY Studentrojects
    }

    public class StudentCourse
    {
        public int StudentId {get; set;} // Composite Primary Key, Foreign Key 1
        public int CourseId {get; set;} // Composite Primary Key, Foreign Key 2
        public Student Student {get; set;} // Navigation Property. One employee per StudentCourse
        public Course Course {get; set;} // Navigation Property. One Student per StudentCourse
        public string StudentGPA {get; set;}
        public DateTime EmployeeJoinDate {get; set;}
    }
}

