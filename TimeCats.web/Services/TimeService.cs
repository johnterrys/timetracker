using Microsoft.EntityFrameworkCore;
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
            TimeCard tc = new TimeCard();

            if (timeCard.timeIn == null || timeCard.timeIn == "")
            {
                tc.timeIn = null;
            }
            else
            {
                tc.timeIn = Convert.ToDateTime(timeCard.timeIn).ToString(); ;
            }

            if (timeCard.timeOut == null || timeCard.timeOut == "")
            {
                tc.timeOut = null;
            }
            else
            {
                tc.timeOut = Convert.ToDateTime(timeCard.timeOut).ToString();
            }

            tc.isEdited = false;
            tc.userID = timeCard.userID;
            tc.groupID = timeCard.groupID;

            if (timeCard.description == null)
            {
                tc.description = null;
            }
            else
            {
                tc.description = timeCard.description;
            }

            
            try
            {
                _context.TimeCards.Add(tc);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return 0;
            }

            return tc.userID;
        }


        public long DeleteTimeCard(TimeCard timeCard)
        {
            _context.TimeCards.Remove(timeCard);
            _context.SaveChanges();

            return 0;
        }







    }
}
