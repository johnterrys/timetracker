using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

            var time = _context.TimeCards
                .FirstOrDefault(t => t.timeslotID == timecard.timeslotID);

            DateTimeOffset after = DateTimeOffset.Parse(time.createdOn.ToString());
            DateTimeOffset before = after.AddDays(-7);

            // Get the current date.
            //DateTime dateForButton = DateTime.Now.AddDays(-1);
            DateTime cutOffDay = DateTime.Today.AddDays(-14);
            int cutOffResult = DateTime.Compare(cutOffDay, timecard.timeIn);


            Console.WriteLine("Save time is being called.");


            if (after < before)
            {
                time.isEdited = true;
            }

            time.timeIn = timecard.timeIn;
            time.timeOut = timecard.timeOut;
            time.description = timecard.description;

            try
            {
                _context.TimeCards.Update(time);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

            return true;
        }


        public bool UserHasTimeInGroup(int userID, int groupID)
        {
            return _context.TimeCards
                .Any(u => u.userID == userID && u.groupID == groupID);
        }

        public long CreateTimeCard(TimeCard timeCard)
        {
            TimeCard tc = new TimeCard();

            tc.createdOn = DateTime.Now;
            tc.isEdited = false;
            tc.userID = timeCard.userID;
            tc.groupID = timeCard.groupID;

            if (timeCard.timeIn == null)
            {
                tc.timeIn = DateTime.Now;
            }
            else
            {
                tc.timeIn = timeCard.timeIn;
            }

            if (timeCard.timeOut == null)
            {
                tc.timeOut = null;
            }
            else
            {
                tc.timeOut = timeCard.timeOut;
            }

            tc.userID = timeCard.userID;

            tc.groupID = timeCard.groupID;

            if (timeCard.description == null || timeCard.description == "")
            {
                tc.description = "";
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

            return tc.timeslotID;
        }

        public long DeleteTimeCard(TimeCard timeCard)
        {
            _context.TimeCards.Remove(timeCard);
            _context.SaveChanges();

            return 0;
        }

    }
}