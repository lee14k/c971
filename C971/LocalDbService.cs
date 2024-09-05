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
        // Method to get a term by its title
    public async Task<Term> GetByTermTitle(string title)
    {
        return await _connection.Table<Term>()
                                .Where(x => x.TermTitle == title)
                                .FirstOrDefaultAsync();
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
    }
}