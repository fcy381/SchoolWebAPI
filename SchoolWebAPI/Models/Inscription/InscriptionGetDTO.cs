﻿using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Student;
using SchoolWebAPI.Models.Teacher;

namespace SchoolWebAPI.Models.Inscription
{
    public class InscriptionGetDTO
    {
        public CourseGetDTO Course { get; set; } = null!;

        public TeacherGetDTO Teacher { get; set; } = null!;

        public StudentGetDTO Student { get; set; } = null!;
    }
}
