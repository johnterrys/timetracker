using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCats.Models;


namespace TimeCats.Services
{
    public class TimeService
    {

        private readonly TimeTrackerContext _context;

        public TimeService(TimeTrackerContext context)
        {
            _context = context;
        }

        public bool SaveTime(TimeCard timecard)
        {
            var edited = "";
            DateTime before;
            DateTime after;

            after = Convert.ToDateTime(edited);
            before = after.AddDays(-7);

            if (after < before)
            {
                timecard.isEdited = true;
                _context.TimeCards.Update(timecard);
                _context.SaveChanges();
            }
            else
            {
                timecard.isEdited = false;
                _context.TimeCards.Update(timecard);
                _context.SaveChanges();
            }

            return timecard.isEdited;
        }

        public bool UserHasTimeInGroup(int userID, int groupID)
        {
            return _context.TimeCards
                .Any(u => u.userID == userID && u.groupID == groupID);
        }

        public long CreateTimeCard(TimeCard timeCard)
        {
            _context.TimeCards.Add(timeCard);
            _context.SaveChanges();

            return timeCard.userID;
        }


        public long DeleteTimeCard(TimeCard timeCard)
        {
            _context.TimeCards.Remove(timeCard);
            _context.SaveChanges();

            return 0;
        }







    }
}
