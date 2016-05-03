using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections;

namespace Typeform_Attendance
{
    class Main
    {
        private static Rootobject rootObject;
        private static int emailPerPage = 50;

        public static Rootobject RootObject
        {
            get
            {
                return rootObject;
            }
        }

        public static int EmailPerPage
        {
            get
            {
                return emailPerPage;
            }

            set
            {
                emailPerPage = value;
            }
        }

        public static List<Person> getEmails(int page)
        {
            List<Person> emailList = new List<Person>();

            Respons[] responseArray = new Respons[0];

            if (rootObject != null)
                responseArray = rootObject.responses;
            if (responseArray == null)
                return emailList;

            foreach (Respons each in responseArray)
            {
                string firstName = each.answers.textfield_21284714;
                string lastName = each.answers.textfield_21284762;
                string email = each.answers.email_21284788;
                Person person = new Person(firstName, lastName, email);

                if (!emailList.Contains(person) && person.Email != "")
                    emailList.Add(person);
            }
            List<Person> subList = new List<Person>();

            int startList = (page - 1) * emailPerPage;
            int emailForThisPage = emailPerPage;
            if (startList > emailList.Count)
                return subList;
            if (emailForThisPage > emailList.Count - startList)
                emailForThisPage = emailList.Count - startList;
            subList = emailList.GetRange(startList, emailForThisPage);

            return subList;
        }

        public static int getAverageAttendance()
        {
            int totalAttendance = 0;

            ArrayList personList = new ArrayList();

            Respons[] responseArray = new Respons[0];

            if (rootObject != null)
                responseArray = rootObject.responses;
            if(responseArray == null)
                return 0;

            foreach (Respons each in responseArray)
            {
                string firstName = each.answers.textfield_21284714;
                string lastName = each.answers.textfield_21284762;
                string email = each.answers.email_21284788;
                Person person = new Person(firstName, lastName, email);

                personList.Add(person);
            }

            Dictionary<Person, int> dict = new Dictionary<Person, int>();

            foreach (Person p in personList)
            {
                Person temp = new Person(p.FirstName, p.LastName, "");
                if (dict.ContainsKey(temp))
                {
                    dict[temp] += 1;
                }
                else
                {
                    dict.Add(temp, 1);
                }
            }

            int maxValue = 0;
            foreach(int i in dict.Values)
            {
                if (i > maxValue)
                    maxValue = i;
                totalAttendance += i;
            }

            if (maxValue == 0)
                return 0;

            return totalAttendance/maxValue;
        }

        public static ArrayList getAttendance()
        {
            ArrayList personList = new ArrayList();

            Respons[] responseArray = new Respons[0];

            if(rootObject != null)
                responseArray = rootObject.responses;
            if (responseArray == null)
                return personList;

            foreach (Respons each in responseArray)
            {
                string firstName = each.answers.textfield_21284714;
                string lastName = each.answers.textfield_21284762;
                string email = each.answers.email_21284788;
                Person person = new Person(firstName, lastName, email);

                personList.Add(person);
            }

            Dictionary<Person, int> dict = new Dictionary<Person, int>();

            foreach(Person p in personList)
            {
                Person temp = new Person(p.FirstName, p.LastName, "");
                if (dict.ContainsKey(temp))
                {
                    dict[temp] += 1;
                }
                else
                {
                    dict.Add(temp, 1);
                }
            }

            personList = new ArrayList();

            responseArray = new Respons[0];

            if (rootObject != null)
                responseArray = rootObject.responses;
            foreach (Respons each in responseArray)
            {
                string firstName = each.answers.textfield_21284714;
                string lastName = each.answers.textfield_21284762;
                string email = each.answers.email_21284788;
                Person temp = new Person(firstName, lastName, "");
                Person person = new Person(firstName, lastName, email);
                person.Meeting = dict[temp];
                if (!personList.Contains(person))
                    personList.Add(person);
            }

            return personList;
        }

        public static void getRootobject(string url)
        {
            Task<Rootobject> task = DownloadData(url);
            task.Wait();
            Rootobject temp = task.Result;
            rootObject = temp;
        }

        async private static Task<Rootobject> DownloadData(string url)
        {
            using (var w = new HttpClient())
            {
                // attempt to download JSON data as a string
                string responseBody = String.Empty;
                try
                {
                    var response = await w.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false);
                   response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                System.Diagnostics.Debug.WriteLine(responseBody);
                return !string.IsNullOrEmpty(responseBody) ? JsonConvert.DeserializeObject<Rootobject>(responseBody) : new Rootobject(); ;
            }
        }
    }
}
