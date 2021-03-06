using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.LikerId == userId && x.LikeeId == recipientId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.IsMain);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            // inbox- messages recive
            var messages = _context.Messages.AsQueryable();

            //  var messages = _context.Messages.Include(x => x.Sender).ThenInclude(x => x.Photos)
            // .Include(x => x.Recipient).ThenInclude(x => x.Photos).AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(x => x.RecipientId == messageParams.UserId && x.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(x => x.SenderId == messageParams.UserId && x.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(x => x.RecipientId == messageParams.UserId && x.RecipientDeleted == false && x.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(x => x.MessageSend);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
             var messages = await _context.Messages
            .Where(x => x.RecipientId == userId && x.RecipientDeleted == false && x.SenderId == recipientId  
                  || x.RecipientId == recipientId  && x.SenderId == userId && x.SenderDeleted == false)
                   .OrderByDescending(x => x.MessageSend).ToListAsync();

            // var messages = await _context.Messages
            //  .Include(x => x.Sender).ThenInclude(x => x.Photos)
            // .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            // .Where(x => x.RecipientId == userId && x.RecipientDeleted == false && x.SenderId == recipientId  
            //       || x.RecipientId == recipientId  && x.SenderId == userId && x.SenderDeleted == false)
            //        .OrderByDescending(x => x.MessageSend).ToListAsync();

             return messages;      
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(x => x.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            // var user =await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == id);
            var user =await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            // var users =  _context.Users.Include(p => p.Photos);
            //var pagedList = new PagedList<User>(users<T>, users.Count(), userParams.PageNumber, userParams.PageSize);

            //filter
            //.OrderByDescending(x => x.LastActive)
            var users =  _context.Users.OrderByDescending(x => x.LastActive).AsQueryable();
            // var users =  _context.Users.Include(p => p.Photos).OrderByDescending(x => x.LastActive).AsQueryable();

            users = users.Where( x => x.Id != userParams.UserId);

            users = users.Where(x =>  x.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(x => userLikers.Contains(x.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                 users = users.Where(x => userLikees.Contains(x.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge !=99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);

                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(x => x.Created);
                        break;
                    default:
                    users = users.OrderByDescending(x => x.LastActive);
                    break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        // return users like
        private async Task<IEnumerable<int>>  GetUserLikes(int id, bool likers)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
            // var user = await _context.Users.Include(x => x.Likers).Include(x => x.Likees)
            // .FirstOrDefaultAsync(x => x.Id == id);


            if (likers)
            {
                return user.Likers.Where(x => x.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(x => x.LikerId == id).Select(i => i.LikeeId);
            }
        }
    }
}