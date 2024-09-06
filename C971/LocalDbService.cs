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
            try
            {
                _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
                _connection.CreateTableAsync<Term>().Wait();
                _connection.CreateTableAsync<Course>().Wait();
                _connection.CreateTableAsync<Instructor>().Wait(); 
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

        public async Task CreateCourse(Course course)
        {
            try
            {
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
                                    .FirstOrDefaultAsync(i => i.CourseId == courseId); // Returns the first instructor associated with the course
        }
        public async Task<List<Instructor>> GetAllInstructors()
        {
            try
            {
                return await _connection.Table<Instructor>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all instructors: {ex.Message}");
                return new List<Instructor>();
            }
        }

        public async Task<List<Instructor>> GetInstructorById(int instructorId)
        {
            try
            {
                return await _connection.Table<Instructor>().Where(i => i.InstructorId == instructorId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving instructors by ID: {ex.Message}");
                return new List<Instructor>();
            }
        }
    }
}