using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCats.Models;


namespace TimeCats.Services
{
    public class EvalService
    {

        private readonly TimeTrackerContext _context;

        public EvalService(TimeTrackerContext context)
        {
            _context = context;
        }

        public bool CompleteEval(int evalID)
        {
            throw new NotImplementedException();

            //var names = _TimeTrackerContext.evals
            //              .FirstOrDefault(u => u.evalID == evalID);

            //names.isComplete = 1;
            //_TimeTrackerContext.evals.Update(names);
            //_TimeTrackerContext.SaveChanges();
            //return true;
        }


        public long CreateTemplate(int userID)
        {
            throw new NotImplementedException();

            //var names = new EvalTemplate();

            //names.userID = userID;
            //names.templateName = "New Template";

            //_TimeTrackerContext.evalTemplates.Add(names);
            //_TimeTrackerContext.SaveChanges();
            //return 0;
        }


        public bool SaveTemplateName(EvalTemplate evalTemplate)
        {
            throw new NotImplementedException();

            //_TimeTrackerContext.EvalTemplate.Update(evalTemplate);
            //_TimeTrackerContext.SaveChanges();
            //return true;
        }

        public bool SaveEval(AdminEval eval)
        {
            throw new NotImplementedException();

            //_TimeTrackerContext.AdminEval.Update(eval);
            //_TimeTrackerContext.SaveChanges();
            //return true;
        }


        public bool DeleteQuestion(int evalTemplateQuestionID)
        {
            throw new NotImplementedException();

            //var name = _TimeTrackerContext.evalTemplateQuestions.Find(evalTemplateQuestionID);
            //_TimeTrackerContext.evalTemplateQuestions.Remove(name);
            //_TimeTrackerContext.SaveChanges();

            //return false;
        }


        public bool SaveCategory(EvalTemplateQuestionCategory category)
        {
            throw new NotImplementedException();

            //_TimeTrackerContext.EvalTemplateQuestionCategory.Add(category);
            //_TimeTrackerContext.SaveChanges();
            //return false;
        }


        public bool DeleteCategory(int evalTemplateQuestionCategoryID)
        {
            throw new NotImplementedException();

            //var names = _TimeTrackerContext.evalTemplateQuestionCategories
            //    .FirstOrDefault(u => u.evalTemplateQuestionCategoryID = evalTemplateQuestionCategoryID);

            //_TimeTrackerContext.evalTemplateQuestionCategories.Remove(names);
            //_TimeTrackerContext.SaveChanges();
            //return false;
        }


        public bool SaveQuestion(EvalTemplateQuestion question)
        {
            throw new NotImplementedException();

            //_TimeTrackerContext.Questions.Add(question);
            //_TimeTrackerContext.SaveChanges();

            //return true;
        }



        public List<EvalTemplate> GetTemplates(int instructorID)
        {
            throw new NotImplementedException();

            //return _TimeTrackerContext.evalTemplates
            //    .ToList(u => u.instructorID == instructorID);
        }

        //var templates = new List<EvalTemplate>();

        //using (var conn = new MySqlConnection(ConnString.ToString()))
        //{
        //    conn.Open();
        //    using (var cmd = conn.CreateCommand())
        //    {
        //        //SQL and Parameters
        //        cmd.CommandText = "Select * From evalTemplates e Where e.userID = @userID ";

        //        cmd.Parameters.AddWithValue("@userID", instructorID);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            //Runs once per record retrieved
        //            while (reader.Read())
        //                templates.Add(new EvalTemplate
        //                {
        //                    evalTemplateID = reader.GetInt32("evalTemplateID"),
        //                    templateName = reader.GetString("templateName"),
        //                    inUse = reader.GetBoolean("inUse"),
        //                    userID = reader.GetInt32("userID")
        //                });
        //        }
        //    }
        //}

        //return templates;


        public List<EvalTemplate> GetFullTemplatesForInstructor(int instructorID)
        {
            throw new NotImplementedException();
            //    var templates = new List<EvalTemplate>();
            //    using (var conn = new MySqlConnection(ConnString.ToString()))
            //    {
            //        conn.Open();
            //        using (var cmd = conn.CreateCommand())
            //        {
            //            //SQL and Parameters
            //            cmd.CommandText =
            //                "SELECT eT.*, eTQC.evalTemplateQuestionCategoryID, eTQC.categoryName, eTQC.number AS categoryNumber, " +
            //                "eTQ.evalTemplateQuestionID, eTQ.evalTemplateQuestionCategoryID AS qevalTemplateQuestionCategoryID, " +
            //                "eTQ.questionType, eTQ.questionText, eTQ.number AS questionNumber " +
            //                "FROM evalTemplates eT " +
            //                "LEFT JOIN evalTemplateQuestionCategories eTQC on eT.evalTemplateID = eTQC.evalTemplateID " +
            //                "LEFT JOIN evalTemplateQuestions eTQ on eT.evalTemplateID = eTQ.evalTemplateID " +
            //                "WHERE eT.userID = @userID ";
            //            cmd.Parameters.AddWithValue("@userID", instructorID);

            //            using (var reader = cmd.ExecuteReader())
            //            {
            //                var template = new EvalTemplate();
            //                //Runs once per record retrieved
            //                while (reader.Read())
            //                {
            //                    if (template.evalTemplateID != reader.GetInt32("evalTemplateID"))
            //                    {
            //                        if (template.evalTemplateID > 0)
            //                            templates.Add(template); //Adds the previous template before making a new one

            //                        template = new EvalTemplate
            //                        {
            //                            evalTemplateID = reader.GetInt32("evalTemplateID"),
            //                            templateName = reader.GetString("templateName"),
            //                            inUse = reader.GetBoolean("inUse"),
            //                            userID = reader.GetInt32("userID"),
            //                            categories = new List<EvalTemplateQuestionCategory>(),
            //                            templateQuestions = new List<EvalTemplateQuestion>()
            //                        };
            //                    }

            //                    if (!reader.IsDBNull(4)) //column 4 = evalTemplateQuestionCategoryID
            //                        template.categories.Add(new EvalTemplateQuestionCategory
            //                        {
            //                            evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                            evalTemplateID = reader.GetInt32("evalTemplateID"),
            //                            categoryName = reader.GetString("categoryName"),
            //                            number = reader.GetInt32("categoryNumber")
            //                        });

            //                    if (!reader.IsDBNull(7)) //column 8 = 
            //                        template.templateQuestions.Add(new EvalTemplateQuestion
            //                        {
            //                            evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                            evalTemplateID = reader.GetInt32("evalTemplateID"),
            //                            evalTemplateQuestionCategoryID = reader.GetInt32("qevalTemplateQuestionCategoryID"),
            //                            questionType = reader.GetChar("questionType"),
            //                            questionText = reader.GetString("questionText"),
            //                            number = reader.GetInt32("questionNumber")
            //                        });
            //                }

            //                if (template.evalTemplateID > 0)
            //                    templates.Add(
            //                        template); //Adds the last template because it wouldn't have been added previously
            //            }
            //        }
            //    }

            //    return templates;
        }

        public bool AssignEvals(List<int> projectIDs, int evalTemplateID)
        {
            throw new NotImplementedException();

            //    var temp = 0;
            //    foreach (var projectID in projectIDs)
            //    {
            //        var tempGroup = new Group();

            //        using (var conn = new MySqlConnection(ConnString.ToString()))
            //        {
            //            conn.Open();
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                //SQL and Parameters
            //                cmd.CommandText =
            //                    "Select g.groupID From cs4450.groups g Inner Join uGroups ug On g.groupID = ug.groupID " +
            //                    "INNER Join users u On ug.userID = u.userID Where projectID = @projectID AND g.isActive = 1 AND ug.isActive = 1 " +
            //                    "GROUP BY g.groupID";

            //                cmd.Parameters.AddWithValue("@projectID", projectID);

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    //Runs once per record retrieved
            //                    while (reader.Read())
            //                    {
            //                        tempGroup.groupID = reader.GetInt32("groupID");

            //                        tempGroup = GetGroup(tempGroup.groupID); //get all the users in group

            //                        if (AssignEvalsForGroup(tempGroup, evalTemplateID,
            //                            GetLastEvalNumber(tempGroup.groupID) + 1))
            //                            temp++;
            //                    }
            //                }
            //            }
            //        }
            //    } //end foreach

            //    return temp > 0;
        }

        public bool AssignEvalsForGroup(Group group, int evalTemplateID, int number)
        {
            throw new NotImplementedException();

            //    var temp = 0;
            //    foreach (var user in group.users)
            //    {
            //        var userID = user.userID;
            //        var groupID = group.groupID;

            //        using (var conn = new MySqlConnection(ConnString.ToString()))
            //        {
            //            conn.Open();
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                //SQL and Parameters
            //                cmd.CommandText = "INSERT INTO evals (evalTemplateID, groupID, userID, number, isComplete) " +
            //                                  "VALUES (@evalTemplateID, @groupID, @userID, @number, 0) ";

            //                cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
            //                cmd.Parameters.AddWithValue("@groupID", groupID);
            //                cmd.Parameters.AddWithValue("@userID", userID);
            //                cmd.Parameters.AddWithValue("@number", number);

            //                //Return the last inserted ID if successful
            //                if (cmd.ExecuteNonQuery() > 0) temp++;
            //            }
            //        }
            //    }

            //    return temp > 0;
        }

        public int GetLastEvalNumber(int groupID)
        {
            throw new NotImplementedException();
            //    var number = 0;
            //    using (var conn = new MySqlConnection(ConnString.ToString()))
            //    {
            //        conn.Open();
            //        using (var cmd = conn.CreateCommand())
            //        {
            //            //SQL and Parameters
            //            cmd.CommandText = "Select MAX(number) AS number From evals e WHERE groupID = @groupID";

            //            cmd.Parameters.AddWithValue("@groupID", groupID);

            //            using (var reader = cmd.ExecuteReader())
            //            {
            //                //Runs once per record retrieved
            //                while (reader.Read())
            //                    if (!reader.IsDBNull(0))
            //                        number = reader.GetInt32("number");
            //            }
            //        }
            //    }

            //    return number;
        }

        public long CreateCategory(int evalTemplateID)
        {
            throw new NotImplementedException();

            //var names = new EvalTemplateQuestionCategory();

            //names.evalTemplateID = evalTemplateID;
            //names.categoryName = "New Category";

            //_TimeTrackerContext.evalTemplateQuestionCategories.Add(names);
            //_TimeTrackerContext.SaveChanges();

            //return 0;
        }




        public int GetInstructorForEval(int evalTemplateID)
        {
            throw new NotImplementedException();

            //return _TimeTrackerContext.evalTemplates
            //          .Where(u => u.evalTemplateID == evalTemplateID)
            //          .Select(u => u.instructorID)
            //          .FirstOrDefault();
        }


        public long CreateTemplateQuestion(int evalTemplateQuestionCategoryID, int evalTemplateID)
        {
            throw new NotImplementedException();

            //var names = new EvalTemplateQuestion();

            //names.evalTemplateID = evalTemplateID;
            //names.evalTemplateQuestionCategoryID = evalTemplateQuestionCategoryID;
            //names.questionType = 'N';
            //names.questionText = null;
            //names.number = 0;

            //_TimeTrackerContext.evalTemplateQuestions.Add(names);
            //_TimeTrackerContext.SaveChanges();

            //return 0;
        }

        public bool SetInUse(int evalTemplateID)
        {
            throw new NotImplementedException();
            //    using (var conn = new MySqlConnection(ConnString.ToString()))
            //    {
            //        conn.Open();
            //        using (var cmd = conn.CreateCommand())
            //        {
            //            //SQL and Parameters
            //            cmd.CommandText = "Update evalTemplates Set inUse = 1 Where evalTemplateID = @evalTemplateID";

            //            // cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
            //            cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

            //            //Return the last inserted ID if successful
            //            if (cmd.ExecuteNonQuery() > 0) return true;
            //            return false;
            //        }
            //    }
        }

        public int GetLatestIncompleteEvaluationID(int groupID, int userID)
        {

            throw new NotImplementedException();

            //        var evalID = 0;
            //        using (var conn = new MySqlConnection(ConnString.ToString()))
            //        {
            //            conn.Open();
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                //SQL and Parameters
            //                cmd.CommandText =
            //                    "Select evalID From evals WHERE groupID = @groupID AND userID = @userID AND isComplete = 0 ORDER BY number DESC";

            //                cmd.Parameters.AddWithValue("@groupID", groupID);
            //                cmd.Parameters.AddWithValue("@userID", userID);

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    //Runs once per record retrieved
            //                    while (reader.Read())
            //                        if (evalID == 0)
            //                        {
            //                            evalID = reader.GetInt32("evalID");
            //                            break;
            //                        }
            //                }
            //            }
            //        }

            //        return evalID;
        }

        public Eval GetEvaluation(int evalID)
        {
            throw new NotImplementedException();
            //        using (var conn = new MySqlConnection(ConnString.ToString()))
            //        {
            //            var eval = new Eval();
            //            eval.templateQuestions = new List<EvalTemplateQuestion>();
            //            eval.categories = new List<EvalTemplateQuestionCategory>();
            //            eval.responses = new List<EvalResponse>();
            //            eval.users = new List<User>();

            //            conn.Open();
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                //SQL and Parameters
            //                cmd.CommandText = "Select * From evals Where evalID = @evalID";

            //                // cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
            //                cmd.Parameters.AddWithValue("@evalID", evalID);

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    //Runs once per record retrieved
            //                    while (reader.Read())
            //                    {
            //                        eval.number = reader.GetInt32("number");
            //                        eval.evalID = reader.GetInt32("evalID");
            //                        eval.evalTemplateID = reader.GetInt32("evalTemplateID");
            //                        eval.groupID = reader.GetInt32("groupID");
            //                        eval.isComplete = reader.GetBoolean("isComplete");
            //                    }
            //                }


            //                //SQL and Parameters
            //                cmd.CommandText = "Select * From evalTemplateQuestions Where evalTemplateID = @evalTemplateID";
            //                cmd.Parameters.AddWithValue("@evalTemplateID", eval.evalTemplateID);

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                        eval.templateQuestions.Add(new EvalTemplateQuestion
            //                        {
            //                            questionText = reader.GetString("questionText"),
            //                            questionType = reader.GetChar("questionType"),
            //                            evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                            evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                            number = reader.GetInt32("number")
            //                        });
            //                }

            //                cmd.CommandText =
            //                    "Select * From evalTemplateQuestionCategories Where evalTemplateID = @evalTemplateID";

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                        eval.categories.Add(new EvalTemplateQuestionCategory
            //                        {
            //                            categoryName = reader.GetString("categoryName"),
            //                            evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID")
            //                        });
            //                }

            //                cmd.CommandText = "Select * From evalResponses Where evalID = @evalID";

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                        eval.responses.Add(new EvalResponse
            //                        {
            //                            evalResponseID = reader.GetInt32("evalResponseID"),
            //                            evalID = reader.GetInt32("evalID"),
            //                            evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                            userID = reader.GetInt32("userID"),
            //                            response = reader.GetString("response")
            //                        });
            //                }

            //                cmd.CommandText =
            //                    "Select * From uGroups uG LEFT JOIN users u on uG.userID = u.userID WHERE uG.groupID = @groupID AND uG.isActive = 1 AND u.isActive";
            //                cmd.Parameters.AddWithValue("@groupID", eval.groupID);

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read())
            //                        eval.users.Add(new User
            //                        {
            //                            firstName = reader.GetString("firstName"),
            //                            lastName = reader.GetString("lastName"),
            //                            userID = reader.GetInt32("userID")
            //                        });
            //                }

            //                return eval;
            //            }
            //        }
        }

        public List<Eval> RandomizeEvaluations(int groupID, int userID)
        {
            throw new NotImplementedException();
            //        var randNum = new Random();
            //        var userEvalResponses = EvalResponses(groupID, userID);
            //        var evalIDs = new List<int>();
            //        var temp = -1;
            //        var arr = new int[100];
            //        arr[0] = temp;
            //        var count = 0;
            //        var repeat = false;

            //        foreach (var eval in userEvalResponses)
            //        {
            //            var tempEvalColumns = eval.evals;
            //            foreach (var evalColumn in tempEvalColumns)
            //            {
            //                count++;
            //                do
            //                {
            //                    repeat = false;
            //                    temp = randNum.Next(1, 1000);
            //                    for (var i = 0; i < 99; i++)
            //                        if (arr[i] == temp)
            //                            repeat = true;
            //                } while (repeat);

            //                evalColumn.originalID = evalColumn.evalID;
            //                evalColumn.evalID = temp;
            //                arr[count] = temp;
            //            }

            //            //puts each evalID in list
            //            foreach (var evalColumn in eval.evals)
            //            foreach (var tempEvalColumn in tempEvalColumns)
            //                if (evalColumn.evalID == tempEvalColumn.originalID)
            //                    evalColumn.evalID = tempEvalColumn.evalID;

            //            foreach (var evalResponse in eval.responses)
            //            foreach (var tempEvalColumn in tempEvalColumns)
            //                if (evalResponse.evalID == tempEvalColumn.originalID)
            //                    evalResponse.evalID = tempEvalColumn.evalID;
            //        }

            //        return userEvalResponses;
        }

        public List<AdminEval> GetAllEvals()
        {
            throw new NotImplementedException();

            //var evals = new List<AdminEval>();
            //using (var conn = new MySqlConnection(ConnString.ToString()))
            //{
            //    conn.Open();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        //SQL and Parameters
            //        cmd.CommandText =
            //            "SELECT e.*, CONCAT(u.firstName, ' ', u.lastName) AS usersName, g.groupName, p.projectID, p.projectName, " +
            //            "c.courseID, c.courseName, et.templateName, c.instructorID, CONCAT(ui.firstName, ' ', ui.lastName) AS instructorName " +
            //            "FROM evals e " +
            //            "LEFT JOIN cs4450.groups g on e.groupID = g.groupID " +
            //            "LEFT JOIN users u on e.userID = u.userID " +
            //            "LEFT JOIN projects p on g.projectID = p.projectID " +
            //            "LEFT JOIN courses c on p.courseID = c.courseID " +
            //            "LEFT JOIN evalTemplates et on e.evalTemplateID = et.evalTemplateID " +
            //            "LEFT JOIN users ui on c.instructorID = ui.userID";

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            //Runs once per record retrieved
            //            while (reader.Read())
            //                evals.Add(new AdminEval
            //                {
            //                    evalID = reader.GetInt32("evalID"),
            //                    evalTemplateID = reader.GetInt32("evalTemplateID"),
            //                    groupID = reader.GetInt32("groupID"),
            //                    userID = reader.GetInt32("userID"),
            //                    number = reader.GetInt32("number"),
            //                    isComplete = reader.GetBoolean("isComplete"),
            //                    usersName = reader.GetString("usersName"),
            //                    groupName = reader.GetString("groupName"),
            //                    projectID = reader.GetInt32("projectID"),
            //                    projectName = reader.GetString("projectName"),
            //                    courseID = reader.GetInt32("courseID"),
            //                    courseName = reader.GetString("courseName"),
            //                    templateName = reader.GetString("templateName"),
            //                    instructorID = reader.GetInt32("instructorID"),
            //                    instructorName = reader.GetString("instructorName")
            //                });
            //        }
            //    }
            //}

            //return evals;
        }

        public List<Eval> EvalResponsesA(int groupID, int userID)
        {
            throw new NotImplementedException();
            //var evals = new List<Eval>();

            //using (var conn = new MySqlConnection(ConnString.ToString()))
            //{
            //    conn.Open();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText =
            //            "SELECT er.*, u.firstName, e.number AS 'evalNumber', etq.number AS 'questionNumber', " +
            //            "u.lastName, etq.questionText, etq.evalTemplateID, etq.questionType, etq.evalTemplateQuestionCategoryID, " +
            //            "etqc.categoryName, etqc.number AS 'categoryNumber' " +
            //            "FROM evalResponses er " +
            //            "  INNER JOIN evals e ON er.evalID = e.evalID " +
            //            "  INNER JOIN users u ON u.userID = e.userID " +
            //            "  INNER JOIN evalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
            //            "  LEFT JOIN evalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
            //            "WHERE groupID = @groupID AND er.userID = @userID";
            //        cmd.Parameters.AddWithValue("@groupID", groupID);
            //        cmd.Parameters.AddWithValue("@userID", userID);

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var foundEval = false;
            //                foreach (var eval in evals)
            //                {
            //                    if (eval.number != reader.GetInt32("evalNumber")) continue;
            //                    foundEval = true;

            //                    //Adding Eval entries
            //                    var foundEvalColumn = false;
            //                    foreach (var evalColumn in eval.evals)
            //                        if (evalColumn.evalID == reader.GetInt32("evalID"))
            //                        {
            //                            foundEvalColumn = true;
            //                            break;
            //                        }

            //                    if (!foundEvalColumn)
            //                        eval.evals.Add(new EvalColumn
            //                        {
            //                            evalID = reader.GetInt32("evalID"),
            //                            firstName = reader.GetString("firstName"), //Name is Team Member for anonymity
            //                            lastName = reader.GetString("lastName")
            //                        });

            //                    //Adding Template Questions
            //                    var foundTemplateQuestion = false;
            //                    foreach (var tq in eval.templateQuestions)
            //                        if (tq.evalTemplateQuestionID == reader.GetInt32("evalTemplateQuestionID"))
            //                        {
            //                            foundTemplateQuestion = true;
            //                            break;
            //                        }

            //                    if (!foundTemplateQuestion)
            //                        eval.templateQuestions.Add(new EvalTemplateQuestion
            //                        {
            //                            questionText = reader.GetString("questionText"),
            //                            evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                            questionType = reader.GetChar("questionType"),
            //                            evalTemplateQuestionCategoryID =
            //                                reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                            number = reader.GetInt32("questionNumber")
            //                        });

            //                    //Adding Categories if they're there
            //                    if (!reader.IsDBNull(13)) //column 13 = categoryName
            //                    {
            //                        var foundCategory = false;
            //                        foreach (var tqc in eval.categories)
            //                            if (tqc.evalTemplateQuestionCategoryID ==
            //                                reader.GetInt32("evalTemplateQuestionCategoryID"))
            //                            {
            //                                foundCategory = true;
            //                                break;
            //                            }

            //                        if (!foundCategory)
            //                            eval.categories.Add(new EvalTemplateQuestionCategory
            //                            {
            //                                evalTemplateQuestionCategoryID =
            //                                    reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                                categoryName = reader.GetString("categoryName"),
            //                                number = reader.GetInt32("categoryNumber")
            //                            });
            //                    }

            //                    //Every row is a unique response, so we don't need to worry about existing ones
            //                    eval.responses.Add(new EvalResponse
            //                    {
            //                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                        evalID = reader.GetInt32("evalID"),
            //                        response = reader.GetString("response"),
            //                        evalResponseID = reader.GetInt32("evalResponseID"),
            //                        userID = reader.GetInt32("userID")
            //                    });
            //                }

            //                if (!foundEval)
            //                {
            //                    var eval = new Eval();
            //                    eval.evalTemplateID = reader.GetInt32("evalTemplateID");
            //                    eval.userID = reader.GetInt32("userID");
            //                    eval.groupID = groupID;
            //                    eval.number = reader.GetInt32("evalNumber");

            //                    //Adding evalColumn
            //                    eval.evals = new List<EvalColumn>();
            //                    eval.evals.Add(new EvalColumn
            //                    {
            //                        evalID = reader.GetInt32("evalID"),
            //                        firstName = reader.GetString("firstName"), //Name is Team Member for anonymity
            //                        lastName = reader.GetString("lastName")
            //                    });

            //                    //Adding templateQuestion
            //                    eval.templateQuestions = new List<EvalTemplateQuestion>();
            //                    eval.templateQuestions.Add(new EvalTemplateQuestion
            //                    {
            //                        questionText = reader.GetString("questionText"),
            //                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                        questionType = reader.GetChar("questionType"),
            //                        evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                        number = reader.GetInt32("questionNumber")
            //                    });

            //                    //Adding Categories if they're there
            //                    eval.categories = new List<EvalTemplateQuestionCategory>();
            //                    if (!reader.IsDBNull(13)) //column 13 = categoryName
            //                        eval.categories.Add(new EvalTemplateQuestionCategory
            //                        {
            //                            evalTemplateQuestionCategoryID =
            //                                reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                            categoryName = reader.GetString("categoryName"),
            //                            number = reader.GetInt32("categoryNumber")
            //                        });

            //                    //Adding Response
            //                    eval.responses = new List<EvalResponse>();
            //                    eval.responses.Add(new EvalResponse
            //                    {
            //                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                        evalID = reader.GetInt32("evalID"),
            //                        response = reader.GetString("response"),
            //                        evalResponseID = reader.GetInt32("evalResponseID"),
            //                        userID = reader.GetInt32("userID")
            //                    });

            //                    evals.Add(eval); //Adding new eval to the list
            //                }
            //            }
            //        }
            //    }
            //}

            //TeammateStats(ref evals);
            //return evals;
        }

        public List<Eval> EvalResponses(int groupID, int userID)
        {
            throw new NotImplementedException();

            //var evals = new List<Eval>();

            //using (var conn = new MySqlConnection(ConnString.ToString()))
            //{
            //    conn.Open();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText =
            //            "SELECT er.*, u.firstName, e.number AS 'evalNumber', etq.number AS 'questionNumber', " +
            //            "u.lastName, etq.questionText, etq.evalTemplateID, etq.questionType, etq.evalTemplateQuestionCategoryID, " +
            //            "etqc.categoryName, etqc.number AS 'categoryNumber' " +
            //            "FROM evalResponses er " +
            //            "  INNER JOIN evals e ON er.evalID = e.evalID " +
            //            "  INNER JOIN users u ON u.userID = e.userID " +
            //            "  INNER JOIN evalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
            //            "  LEFT JOIN evalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
            //            "WHERE groupID = @groupID AND er.userID = @userID";
            //        cmd.Parameters.AddWithValue("@groupID", groupID);
            //        cmd.Parameters.AddWithValue("@userID", userID);

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var foundEval = false;
            //                foreach (var eval in evals)
            //                {
            //                    if (eval.number != reader.GetInt32("evalNumber")) continue;
            //                    foundEval = true;

            //                    //Adding Eval entries
            //                    var foundEvalColumn = false;
            //                    foreach (var evalColumn in eval.evals)
            //                        if (evalColumn.evalID == reader.GetInt32("evalID"))
            //                        {
            //                            foundEvalColumn = true;
            //                            break;
            //                        }

            //                    if (!foundEvalColumn)
            //                        eval.evals.Add(new EvalColumn
            //                        {
            //                            evalID = reader.GetInt32("evalID"),
            //                            firstName = "Team", //Name is Team Member for anonymity
            //                            lastName = "Member"
            //                        });

            //                    //Adding Template Questions
            //                    var foundTemplateQuestion = false;
            //                    foreach (var tq in eval.templateQuestions)
            //                        if (tq.evalTemplateQuestionID == reader.GetInt32("evalTemplateQuestionID"))
            //                        {
            //                            foundTemplateQuestion = true;
            //                            break;
            //                        }

            //                    if (!foundTemplateQuestion)
            //                        eval.templateQuestions.Add(new EvalTemplateQuestion
            //                        {
            //                            questionText = reader.GetString("questionText"),
            //                            evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                            questionType = reader.GetChar("questionType"),
            //                            evalTemplateQuestionCategoryID =
            //                                reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                            number = reader.GetInt32("questionNumber")
            //                        });

            //                    //Adding Categories if they're there
            //                    if (!reader.IsDBNull(13)) //column 13 = categoryName
            //                    {
            //                        var foundCategory = false;
            //                        foreach (var tqc in eval.categories)
            //                            if (tqc.evalTemplateQuestionCategoryID ==
            //                                reader.GetInt32("evalTemplateQuestionCategoryID"))
            //                            {
            //                                foundCategory = true;
            //                                break;
            //                            }

            //                        if (!foundCategory)
            //                            eval.categories.Add(new EvalTemplateQuestionCategory
            //                            {
            //                                evalTemplateQuestionCategoryID =
            //                                    reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                                categoryName = reader.GetString("categoryName"),
            //                                number = reader.GetInt32("categoryNumber")
            //                            });
            //                    }

            //                    //Every row is a unique response, so we don't need to worry about existing ones
            //                    eval.responses.Add(new EvalResponse
            //                    {
            //                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                        evalID = reader.GetInt32("evalID"),
            //                        response = reader.GetString("response"),
            //                        evalResponseID = reader.GetInt32("evalResponseID"),
            //                        userID = reader.GetInt32("userID")
            //                    });
            //                }

            //                if (!foundEval)
            //                {
            //                    var eval = new Eval();
            //                    eval.evalTemplateID = reader.GetInt32("evalTemplateID");
            //                    eval.userID = reader.GetInt32("userID");
            //                    eval.groupID = groupID;
            //                    eval.number = reader.GetInt32("evalNumber");

            //                    //Adding evalColumn
            //                    eval.evals = new List<EvalColumn>();
            //                    eval.evals.Add(new EvalColumn
            //                    {
            //                        evalID = reader.GetInt32("evalID"),
            //                        firstName = "Team", //Name is Team Member for anonymity
            //                        lastName = "Member"
            //                    });

            //                    //Adding templateQuestion
            //                    eval.templateQuestions = new List<EvalTemplateQuestion>();
            //                    eval.templateQuestions.Add(new EvalTemplateQuestion
            //                    {
            //                        questionText = reader.GetString("questionText"),
            //                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                        questionType = reader.GetChar("questionType"),
            //                        evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                        number = reader.GetInt32("questionNumber")
            //                    });

            //                    //Adding Categories if they're there
            //                    eval.categories = new List<EvalTemplateQuestionCategory>();
            //                    if (!reader.IsDBNull(13)) //column 13 = categoryName
            //                        eval.categories.Add(new EvalTemplateQuestionCategory
            //                        {
            //                            evalTemplateQuestionCategoryID =
            //                                reader.GetInt32("evalTemplateQuestionCategoryID"),
            //                            categoryName = reader.GetString("categoryName"),
            //                            number = reader.GetInt32("categoryNumber")
            //                        });

            //                    //Adding Response
            //                    eval.responses = new List<EvalResponse>();
            //                    eval.responses.Add(new EvalResponse
            //                    {
            //                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
            //                        evalID = reader.GetInt32("evalID"),
            //                        response = reader.GetString("response"),
            //                        evalResponseID = reader.GetInt32("evalResponseID"),
            //                        userID = reader.GetInt32("userID")
            //                    });

            //                    evals.Add(eval); //Adding new eval to the list
            //                }
            //            }
            //        }
            //    }
            //}

            //TeammateStats(ref evals);
            //return evals;
        }

        //  Helper function that will assist with total a score per each teammate
        private void TeammateStats(ref List<Eval> eval)
        {
            throw new NotImplementedException();

            //using (var conn = new MySqlConnection(ConnString.ToString()))
            //{
            //    foreach (var e in eval)
            //        foreach (var r in e.responses)
            //        {
            //            conn.Open();
            //            //  Get avg score student gave for the specific eval
            //            using (var cmd = conn.CreateCommand())
            //            {
            //                cmd.CommandText =
            //                    "SELECT u.userID, u.firstName, u.lastName, AVG(er.response) AS avg " +
            //                    "FROM evalResponses er INNER JOIN evals e ON er.evalID = e.evalID " +
            //                    "INNER JOIN users u ON u.userID = e.userID " +
            //                    "INNER JOIN evalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
            //                    "LEFT JOIN evalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
            //                    "WHERE groupID = @groupID " +
            //                    "AND e.evalID = @evalID " +
            //                    "GROUP BY u.userID;";

            //                cmd.Parameters.AddWithValue("@evalID", r.evalID);
            //                cmd.Parameters.AddWithValue("@groupID", e.groupID);

            //                using (var reader = cmd.ExecuteReader())
            //                {
            //                    while (reader.Read()) r.userAvgerage = reader.GetDouble("avg");
            //                }
            //            }

            //            conn.Close();
            //        }
            //}
        }

        public bool SaveResponse(int userID, int evalID, int evalTemplateQuestionID, string response)
        {
            throw new NotImplementedException();

            //using (var conn = new MySqlConnection(ConnString.ToString()))
            //{
            //    conn.Open();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        //SQL and Parameters
            //        cmd.CommandText = "INSERT INTO evalResponses (evalID, evalTemplateQuestionID, userID, response) " +
            //                          "VALUES (@evalID, @evalTemplateQuestionID, @userID, @response)";
            //        cmd.Parameters.AddWithValue("@userID", userID);
            //        cmd.Parameters.AddWithValue("@evalID", evalID);
            //        cmd.Parameters.AddWithValue("@evalTemplateQuestionID", evalTemplateQuestionID);
            //        cmd.Parameters.AddWithValue("@response", response);

            //        //Return the last inserted ID if successful
            //        if (cmd.ExecuteNonQuery() > 0) return true;
            //        return false;
            //    }
            //}
        }

        public bool CreateTemplateCopy(int userID, int evalTemplateID)
        {
            throw new NotImplementedException();

            //var templateName = "";
            //var catName = "";
            //var temp = "";

            //using (var conn = new MySqlConnection(ConnString.ToString()))
            //{
            //    conn.Open();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "SELECT templateName FROM evalTemplates WHERE evalTemplateID = @evalTemplateID";
            //        cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            //Runs once per record retrieved
            //            while (reader.Read()) templateName = reader.GetString("templateName");
            //        }

            //        //SQL and Parameters
            //        cmd.CommandText = "INSERT INTO evalTemplates (userID, templateName) " +
            //                          "VALUES (@userID, '@TempName')";
            //        cmd.Parameters.AddWithValue("@userID", userID);
            //        cmd.Parameters.AddWithValue("@TempName", templateName);

            //        //Return the last inserted ID if successful
            //        cmd.ExecuteNonQuery();
            //        var higher = "0";
            //        cmd.Parameters.AddWithValue("@questionType", higher);
            //        cmd.Parameters.AddWithValue("@questionText", higher);
            //        cmd.Parameters.AddWithValue("@number", higher);
            //        cmd.CommandText =
            //            "SELECT * FROM evalTemplateQuestions AS 'ETQ' INNER JOIN evalTemplateQuestionCategories AS 'ETC' "
            //            + "ON ETC.evalTemplateQuestionCategoryID = ETQ.evalTemplateQuestionCategoryID WHERE ETQ.evalTemplateID = "
            //            + "@evalTemplateID ORDER BY ETC.categoryName";
            //        cmd.Parameters.AddWithValue("@evalTemplateID", cmd.LastInsertedId);
            //        cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", higher);

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            var catNum = 0;
            //            while (reader.Read())
            //            {
            //                catName = reader.GetString("ETC.categoryName");
            //                if (catName != temp)
            //                {
            //                    cmd.CommandText =
            //                        "INSERT INTO evalTemplateQuestionCategory (evalTemplateID, categoryName) "
            //                        + "VALUES (@evalTemplateID, @categoryName)";
            //                    cmd.ExecuteNonQuery();
            //                    cmd.Parameters["@evalTemplateQuestionCategoryID"].Value = cmd.LastInsertedId;
            //                }

            //                temp = catName;
            //                catNum = reader.GetInt32("ETQ.evalTemplateQuestionCategoryID");

            //                cmd.CommandText =
            //                    "INSERT INTO evalTemplateQuestions (evalTemplateID, evalTemplateQuestionCategoryID, "
            //                    + "questionType, questionText, number) VALUES (@evalTemplateID, @evalTemplateQuestionCategoryID, "
            //                    + "@questionType, @questionText, @number)";
            //                cmd.Parameters["@questionType"].Value = reader.GetString("ETQ.questionType");
            //                cmd.Parameters["@questionText"].Value = reader.GetString("ETQ.questionText");
            //                cmd.Parameters["@number"].Value = reader.GetString("ETQ.number");

            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //}

            //return true;
        }


        //Implemented elsewhere
        //public Project GetProject(int projectID)
        //{
        //    //TODO
        //    var names = (from p in _TimeTrackerContext.Projects
        //                join g in _TimeTrackerContext.Groups on p.projectID equals g.projectID
        //                join ug in _TimeTrackerContext.UserGroups on g.groupID equals ug.groupID
        //                join u in _TimeTrackerContext.Users on ug.userID equals u.userID
        //                join t in _TimeTrackerContext.TimeCards on
        //                    new
        //                    {
        //                        key1 = u.userID,
        //                        key2 = g.groupID
        //                    }
        //                    equals
        //                    new
        //                    {
        //                        key1 = t.userID,
        //                        key2 = t.groupID
        //                    }
        //                where (p.projectID == projectID)
        //                select new
        //                {
        //                    p.projectID,
        //                    p.projectName,
        //                    p.isActive,
        //                    p.description,
        //                    p.CourseID,
        //                    p.Course,
        //                    g.groupID,
        //                    g.groupName,
        //                    groupIsActive = g.isActive,
        //                    u.userID,
        //                    u.firstName,
        //                    u.lastName,
        //                    tgroupID = t.groupID,
        //                    t.timeslotID,
        //                    timeDescription = t.description,
        //                    t.isEdited,
        //                    tuserID = t.userID,
        //                    isActiveInGroup = ug.isActive
        //                })
        //                .ToList();




        //    var project = new Project
        //    {
        //        groups = new List<Group>()
        //    };

        //    using (var conn = new mysqlconnection(connstring.tostring()))
        //    {
        //        conn.open();
        //        using (var cmd = conn.createcommand())
        //        {
        //            //sql and parameters
        //            cmd.commandtext =
        //                "select p.*, g.groupid, g.groupname, g.isactive as groupisactive, u.userid, u.firstname, u.lastname, t.groupid as 'tgroupid', t.timeid, " +
        //                "date_format(t.timein, '%m/%d/%y %l:%i %p') as 'timein', date_format(t.timeout, '%m/%d/%y %l:%i %p') as 'timeout', " +
        //                "t.description as 'timedescription', t.isedited, t.userid as 'tuserid', ug.isactive as isactiveingroup " +
        //                "from projects p " +
        //                "left join cs4450.groups g on p.projectid = g.projectid " +
        //                "left join ugroups ug on ug.groupid = g.groupid " +
        //                "left join users u on u.userid = ug.userid " +
        //                "left join timecards t on (u.userid = t.userid and g.groupid = t.groupid) " +
        //                "where p.projectid = @projectid";
        //            cmd.parameters.addwithvalue("@projectid", projectid);

        //            using (var reader = cmd.executereader())
        //            {
        //                //runs once per record retrieved
        //                while (reader.read())
        //                {
        //                    if (project.projectID == 0)
        //                    {
        //                        project.projectID = reader.GetInt32("projectID");
        //                        project.projectName = reader.GetString("projectName");
        //                        project.isActive = reader.GetBoolean("isActive");
        //                        project.description = reader.GetString("description");
        //                        project.CourseID = reader.GetInt32("courseID");
        //                    }

        //                    var foundGroup = false;

        //                    foreach (var group in project.groups)
        //                        if (group.groupID == reader.GetInt32("groupID"))
        //                        {
        //                            foundGroup = true;

        //                            var foundUser = false;

        //                            if (group.users == null) group.users = new List<User>();

        //                            if (group.groupID == 0)
        //                            {
        //                                group.groupName = reader.GetString("groupName");
        //                                group.groupID = reader.GetInt32("groupID");
        //                                group.isActive = reader.GetBoolean("groupIsActive");
        //                            }

        //                            //get each users time info
        //                            foreach (var user in group.users)
        //                                if (user.userID == reader.GetInt32("userID"))
        //                                {
        //                                    foundUser = true;
        //                                    //Add time slot

        //                                    if (user.timecards == null) user.timecards = new List<TimeCard>();

        //                                    if (!reader.IsDBNull(12))
        //                                        user.timecards.Add(new TimeCard
        //                                        {
        //                                            timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
        //                                            timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
        //                                            description = reader.GetString("description"),
        //                                            groupID = reader.GetInt32("tgroupID"),
        //                                            timeslotID = reader.GetInt32("timeID"),
        //                                            isEdited = reader.GetBoolean("isEdited"),
        //                                            userID = reader.GetInt32("tuserID")
        //                                        });
        //                                }

        //                            if (!foundUser)
        //                            {
        //                                var timecardlist = new List<TimeCard>();
        //                                if (!reader.IsDBNull(12))
        //                                    timecardlist.Add(new TimeCard
        //                                    {
        //                                        timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
        //                                        timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
        //                                        description = reader.GetString("description"),
        //                                        groupID = reader.GetInt32("tgroupID"),
        //                                        timeslotID = reader.GetInt32("timeID"),
        //                                        isEdited = reader.GetBoolean("isEdited"),
        //                                        userID = reader.GetInt32("tuserID")
        //                                    });

        //                                //Add the user and then the time slot
        //                                if (!reader.IsDBNull(8))
        //                                    group.users.Add(new User
        //                                    {
        //                                        userID = reader.GetInt32("userID"),
        //                                        firstName = reader.GetString("firstName"),
        //                                        lastName = reader.GetString("lastName"),
        //                                        timecards = timecardlist,
        //                                        isActive = reader.GetBoolean("isActiveInGroup")
        //                                    });
        //                            }
        //                        }

        //                    if (!foundGroup)
        //                    {
        //                        var timecardlist = new List<TimeCard>();
        //                        if (!reader.IsDBNull(12))
        //                            timecardlist.Add(new TimeCard
        //                            {
        //                                timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
        //                                timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
        //                                description = reader.GetString("timeDescription"),
        //                                groupID = reader.GetInt32("tgroupID"),
        //                                timeslotID = reader.GetInt32("timeID"),
        //                                isEdited = reader.GetBoolean("isEdited")
        //                            });

        //                        var users = new List<User>();
        //                        if (!reader.IsDBNull(8))
        //                            users.Add(new User
        //                            {
        //                                userID = reader.GetInt32("userID"),
        //                                firstName = reader.GetString("firstName"),
        //                                lastName = reader.GetString("lastName"),
        //                                timecards = timecardlist,
        //                                isActive = reader.GetBoolean("isActiveInGroup")
        //                            });

        //                        if (!reader.IsDBNull(5))
        //                            project.groups.Add(new Group
        //                            {
        //                                groupID = reader.GetInt32("groupID"),
        //                                groupName = reader.GetString("groupName"),
        //                                isActive = reader.GetBoolean("groupIsActive"),
        //                                users = users
        //                            });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return project;
        //}













    }
}
