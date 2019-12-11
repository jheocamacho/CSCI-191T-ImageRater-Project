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

        public Task<Post> GetPostAsync(int id)
        {
            return _database.Table<Post>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SavePostAsync(Post post)
        {
            if (post.ID != 0)
            {
                return _database.UpdateAsync(post);
            }
            else
            {
                return _database.InsertAsync(post);
            }
        }

        public Task<int> RemovePostAsync(Post post)
        {
            return _database.DeleteAsync(post);
        }
    }
}
