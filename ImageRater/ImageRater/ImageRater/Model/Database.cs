using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ImageRater.Model
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Post>().Wait();
        }

        public Task<List<Post>> GetPostAsync()
        {
            return _database.Table<Post>().ToListAsync();
        }

        public Task<int> SavePostAsync(Post post)
        {
            return _database.InsertAsync(post);
        }
    }
}
