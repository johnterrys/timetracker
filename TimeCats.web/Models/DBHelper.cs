using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeCats.Models
{
    
    public class DBHelper
    {
        private static TimeTrackerContext _TimeTrackerContext { get; set; }
        public DBHelper()
        {
            
        }

        public static List<Dashboard> GetDashboard(int userID)
        {
            var dashboard = new List<Dashboard>();

            dashboard.Add(new Dashboard
            {
                groupID = 1,
                groupName = "Super Duper Group Thing",
                projectID = 1,
                projectName = "Super Project Name",
                courseID = 1,
                courseName = "Course2345",
                instructorID = 1,
                instructorName = "Instructor Man!"
            });

            return dashboard;
        }


        //    var group = new Group();
        //    group.users = new List<User>();
        //    var foundUser = false;


        //    using (var conn = new MySqlConnection(ConnString.ToString()))
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            //SQL and Parameters
        //            cmd.CommandText =
        //                "Select g.*, u.userID, u.firstName, u.lastName, t.groupID AS 'tgroupID', t.timeID, " +
        //                "date_format(t.timeIn, '%m/%d/%Y %l:%i %p') AS 'timeIn', date_format(t.timeOut, '%m/%d/%Y %l:%i %p') AS 'timeOut', " +
        //                "t.description, t.isEdited, t.userID AS 'tuserID', ug.isActive AS isActiveInGroup  " +
        //                "From cs4450.groups g Left Join uGroups ug On " +
        //                "ug.groupID = g.groupID " +
        //                "Left Join users u On " +
        //                "u.userID = ug.userID " +
        //                "Left Join timeCards t On " +
        //                "(u.userID = t.userID AND g.groupID = t.groupID) " +
        //                "Where g.groupID = @groupID";
        //            cmd.Parameters.AddWithValue("@groupID", groupID);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                //Runs once per record retrieved
        //                while (reader.Read())
        //                {

        //                    foundUser = false;
        //                    group.groupName = names.groupName;
        //                    group.groupID = names.groupID;
        //                    group.isActive = names.isActive;
        //                    group.projectID = reader.GetInt32("projectID");

        //                    //get each users time info
        //                    foreach (var user in group.users)
        //                        if (user.userID == reader.GetInt32("userID"))
        //                        {
        //                            foundUser = true;
        //                            //Add time slot

        //                            if (user.timecards == null) user.timecards = new List<TimeCard>();

        //                            if (!reader.IsDBNull(9))
        //                                user.timecards.Add(new TimeCard
        //                                {
        //                                    timeIn = reader.IsDBNull(10) ? "" : reader.GetString("timeIn"),
        //                                    timeOut = reader.IsDBNull(11) ? "" : reader.GetString("timeOut"),
        //                                    description = reader.GetString("description"),
        //                                    groupID = reader.GetInt32("tgroupID"),
        //                                    timeslotID = reader.GetInt32("timeID"),
        //                                    isEdited = reader.GetBoolean("isEdited"),
        //                                    userID = reader.GetInt32("tuserID")
        //                                });
        //                        }

        //                    if (!foundUser)
        //                    {
        //                        var timecardlist = new List<TimeCard>();
        //                        if (!reader.IsDBNull(9))
        //                            timecardlist.Add(new TimeCard
        //                            {
        //                                timeIn = reader.IsDBNull(10) ? "" : reader.GetString("timeIn"),
        //                                timeOut = reader.IsDBNull(11) ? "" : reader.GetString("timeOut"),
        //                                description = reader.GetString("description"),
        //                                groupID = reader.GetInt32("tgroupID"),
        //                                timeslotID = reader.GetInt32("timeID"),
        //                                isEdited = reader.GetBoolean("isEdited"),
        //                                userID = reader.GetInt32("tuserID")
        //                            });

        //                        //Add the user and then the time slot
        //                        if (!reader.IsDBNull(5))
        //                            group.users.Add(new User
        //                            {
        //                                userID = reader.GetInt32("userID"),
        //                                firstName = reader.GetString("firstName"),
        //                                lastName = reader.GetString("lastName"),
        //                                timecards = timecardlist,
        //                                isActive = reader.GetBoolean("isActiveInGroup")
        //                            });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return group;
        //}






    }
}