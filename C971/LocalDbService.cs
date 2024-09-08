using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;


namespace C971
{
    public class LocalDbService
    {
        private const string DB_NAME = "c971_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;
        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            InitializeDatabaseAsync();
        }
            private async Task InitializeDatabaseAsync() { 
            try
            {
                await _connection.CreateTableAsync<Term>();
                await _connection.CreateTableAsync<Course>();
                await _connection.CreateTableAsync<Instructor>();
                await _connection.CreateTableAsync<Assessment>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Initialization Error: {ex.Message}");
            }
        }
        public async Task CreateTerm(Term term)
        {
            try
            {
                await _connection.InsertAsync(term);
                Console.WriteLine("Term successfully inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting term: {ex.Message}");
            }
        }
        public async Task CreateAssessment(int courseId, Assessment assessment)
        {
            try
            {
                var assessmentsForCourse = await _connection.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();

                if (assessmentsForCourse.Count >= 2)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "A course can have a maximum of two assessments.", "OK");
                    return;
                }
                if (assessmentsForCourse.Any(a => a.AssessmentType == assessment.AssessmentType))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"An assessment of type {assessment.AssessmentType} already exists for this course.", "OK");
                    return;
                }

                await _connection.InsertAsync(assessment);
                Console.WriteLine("Assessment successfully inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting assessment: {ex.Message}");
            }
        }
        public async Task UpdateAssessment(Assessment assessment)
        {
            try
            {
                await _connection.UpdateAsync(assessment);
                Console.WriteLine("Assessment successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating assessment: {ex.Message}");
            }
        }
        public async Task <List<Assessment>> GetAllAssessments()
        {
            try
            {
                return await _connection.Table<Assessment>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving assessments: {ex.Message}");
                return new List<Assessment>();
            }
        }
        public async Task<List<Assessment>> GetAssessmentByCourseId(int courseId)

        {
            try
            {
                return await _connection.Table<Assessment>().Where(c => c.CourseId == courseId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving courses for term ID {courseId}: {ex.Message}");
                return new List<Assessment>();
            }
        }
        public async Task DeleteAssessment(Assessment assessment)
        {
            try
            {
                await _connection.DeleteAsync(assessment);
                Console.WriteLine("Assessment successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting assessment: {ex.Message}");
            }
        }
        public async Task<List<Term>> GetTerms()
        {
            try
            {
                return await _connection.Table<Term>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving terms: {ex.Message}");
                return new List<Term>();
            }
        }
        public async Task<Term> GetByTermId(int id)
        {
            try
            {
                return await _connection.Table<Term>().Where(x => x.TermId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving term by ID: {ex.Message}");
                return null;
            }
        }
        public async Task<Term> GetByTermTitle(string title)
        {
            try
            {
                return await _connection.Table<Term>().Where(x => x.TermTitle == title).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving term by title: {ex.Message}");
                return null;
            }
        }
        public async Task UpdateTerm(Term term)
        {
            try
            {
                await _connection.UpdateAsync(term);
                Console.WriteLine("Term successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating term: {ex.Message}");
            }
        }
        public async Task DeleteTerm(Term term)
        {
            try
            {
                await _connection.DeleteAsync(term);
                Console.WriteLine("Term successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting term: {ex.Message}");
            }
        }
        public async Task DeleteCourse(Course course)
        {
            try {
                await _connection.DeleteAsync(course);
                Console.WriteLine("Term successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting term: {ex.Message}");
            }
        }
        public async Task<List<Course>> GetCourses()
        {
            try
            {
                return await _connection.Table<Course>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving terms: {ex.Message}");
                return new List<Course>();
            }
        }
        public async Task<List<Course>> GetCoursesByTermId(int termId)
        {
            try
            {
                return await _connection.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving courses for term ID {termId}: {ex.Message}");
                return new List<Course>();
            }
        }
        public async Task CreateCourse(int termId, Course course)
        {
            try
            {
                var coursesForTerm = await _connection.Table<Course>().Where(c => c.TermId == termId).ToListAsync();

                if (coursesForTerm.Count >= 6)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "A term can have a maximum of six courses.", "OK");
                    return;
                }

                await _connection.InsertAsync(course);
                Console.WriteLine("Course successfully inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting course: {ex.Message}");
            }
        }
        public async Task UpdateCourse(Course course)
        {
            try
            {
                await _connection.UpdateAsync(course);
                Console.WriteLine("Course successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating course: {ex.Message}");
            }
        }
        public async Task<Course> GetCourseById(int id)
        {
            try
            {
                return await _connection.Table<Course>().Where(x => x.CourseId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving course by ID: {ex.Message}");
                return null;
            }
        }
        public async Task CreateInstructor(Instructor instructor)
        {
            try
            {
                await _connection.InsertAsync(instructor);
                Console.WriteLine("Instructor successfully inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting instructor: {ex.Message}");
            }
        }
        public async Task UpdateInstructor(Instructor instructor)
        {
            try
            {
                await _connection.UpdateAsync(instructor);
                Console.WriteLine("Instructor successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating instructor: {ex.Message}");
            }
        }
        public async Task<Instructor> GetInstructorByCourseId(int courseId)
        {
            return await _connection.Table<Instructor>()
                                    .FirstOrDefaultAsync(i => i.CourseId == courseId); 
        }
        public async Task<List<Instructor>> GetInstructorsByIds(List<int> instructorIds)
        {
            try
            {
                return await _connection.Table<Instructor>()
                                        .Where(i => instructorIds.Contains(i.InstructorId))
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving instructors: {ex.Message}");
                return new List<Instructor>();
            }
        }
        public async Task<Instructor> GetInstructorById(int instructorId)
        {
            try
            {
                return await _connection.Table<Instructor>()
                                        .Where(i => i.InstructorId == instructorId)
                                        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving instructor by ID: {ex.Message}");
                return null;
            }
        }
        public async Task<Instructor> GetInstructorByName(string instructorName)
        {
            try
            {
                return await _connection.Table<Instructor>()
                                        .Where(i => i.InstructorName == instructorName)
                                        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving instructor by name: {ex.Message}");
                return null;
            }
        }
    }
}